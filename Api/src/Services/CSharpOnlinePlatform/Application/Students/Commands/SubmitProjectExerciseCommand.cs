using Application.Common.Interfaces;
using Application.Services;
using Core.DTO;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Students.Commands
{
    public class SubmitProjectExerciseCommand : IRequest<ServerResponseDTO>
    {
        public Guid ProjectExerciseId { get; }
        public Guid StudentId { get; }
        public IFormFile File { get; }

        public SubmitProjectExerciseCommand(Guid projectExerciseId, Guid studentId, IFormFile file)
        {
            ProjectExerciseId = projectExerciseId;
            StudentId = studentId;
            File = file;
        }

        public override string ToString()
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            return JsonSerializer.Serialize(new { StudentId, ProjectExerciseId }, jso);
        }
    }
    public class SubmitProjectExerciseCommandHandler : IRequestHandler<SubmitProjectExerciseCommand, ServerResponseDTO>
    {
        private readonly IApplicationDbContext _dbContext;
        ILogger<SubmitProjectExerciseCommand> _logger;
        private IGoogleDriveService _driveService;

        public SubmitProjectExerciseCommandHandler(IApplicationDbContext _dbContext, ILogger<SubmitProjectExerciseCommand> _logger, IGoogleDriveService driveService)
        {
            this._dbContext = _dbContext;
            this._logger = _logger;
            _driveService = driveService;
        }

        public async Task<ServerResponseDTO> Handle(SubmitProjectExerciseCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CSharpOnlinePlatform Request submit project: ProjectExerciseId:{request.ToString()}");
            var projectExercise = await _dbContext.GetEntities<ProjectExercise>()
                .Include(t => t.Theme)
                .ThenInclude(t => t.Course)
                .SingleAsync(t => t.Id == request.ProjectExerciseId);

            string uploadedProjectName = request.File.FileName;
            using (var stream = new FileStream(uploadedProjectName, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }
            var buffer = 1024 * 1024;
            var fileStream = new FileStream(uploadedProjectName, FileMode.Open, FileAccess.Read, FileShare.Read, buffer, useAsync: true);

            var studentCourseProjectExerciseId = Guid.NewGuid();
            var student = await _dbContext.GetEntities<Student>().SingleAsync(s => s.Id == request.StudentId);
            var fileName = $"{studentCourseProjectExerciseId}_{student.FullName}";

            var uploadedFileLink = await _driveService.TryUpload(fileName, fileStream);

            var response = new ServerResponseDTO();
            if (uploadedFileLink.status)
            {
                response.Success = true;
                response.Message = "Проект загружен. Ожидайте ответа учителя";
                await CreateNewStudentCourseProjectExercise(projectExercise, uploadedFileLink.linkToProject, student.Id, projectExercise.Theme.CourseId);
            }
            else
            {
                response.Success = false;
                response.Message = "Не удалось загрузить проект. Повторите попытку позже.";
            }

            fileStream.Dispose();
            fileStream.Close();
            FileInfo fileDate = new FileInfo(uploadedProjectName);
            if (fileDate.Exists)
            {
                fileDate.Delete();
            }

            return response;
        }

        private async Task CreateNewStudentCourseProjectExercise(ProjectExercise projectExercise, string linkToProject, Guid studentId, Guid courseId)
        {
            var studentCourse = await _dbContext.GetEntities<StudentCourse>().FirstOrDefaultAsync(s => s.StudentId == studentId && s.CourseId == courseId);
            if (studentCourse != null)
            {
                var newStudentCourseProjectExercise = new StudentCourseProjectExercise
                {
                    ProjectExerciseId = projectExercise.Id,
                    StudentCourseId = studentCourse.Id,
                    Status = Status.WaitForTeacher,
                    LinkToProject = linkToProject,
                    Date = DateTime.Now,
                    Comment = ""
                };

                _dbContext.Add(newStudentCourseProjectExercise);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
