using Application.Admin.Commands.TeacherCommand;
using Application.Students.Commands;
using Application.Teachers.Commands.CourseCommand;
using Application.Teachers.Commands.ProjectExerciseCommand.CreateProjectExercise;
using Application.Teachers.Commands.ThemeCommands;
using Core.Exceptions;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Students;

public class SubmitProjectExerciseCommandTest
{
    [Test]
    public void SubmitProjectExerciseCommand_NullFile_NotNullException()
    {
        SubmitProjectExerciseCommand command = new SubmitProjectExerciseCommand(Guid.NewGuid(), Guid.NewGuid(), null);

        ValidationFailureException validationException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
        var fileMustNotBeNullErrorShown = IsErrorExists("File", ValidationMessages.GetNotNullMessage("File"), validationException);

        fileMustNotBeNullErrorShown.Should().BeTrue();
    }
    
    [Test]
    public void SubmitProjectExerciseCommand_NotExistingProjectId_NotFoundException()
    {
        SubmitProjectExerciseCommand command = new SubmitProjectExerciseCommand(Guid.NewGuid(), Guid.NewGuid(), IFormFileFake(RandomString(5)));

        ValidationFailureException validationException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
        var projectNotFoundExceptionShown = IsErrorExists("ProjectExerciseId", "Проект не найден.", validationException);

        projectNotFoundExceptionShown.Should().BeTrue();
    }

    [Test]
    public async Task SubmitProjectExerciseCommand_NotExistingStudentId_NotFoundException()
    {
        var projectExerciseId = await GetProjectExerciseId();
        SubmitProjectExerciseCommand command = new SubmitProjectExerciseCommand(projectExerciseId, Guid.NewGuid(), IFormFileFake(RandomString(5)));

        ValidationFailureException validationException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
        var studentNotFoundExceptionShown = IsErrorExists("StudentId", "Студент не найден.", validationException);

        studentNotFoundExceptionShown.Should().BeTrue();
    }

    [Test]
    public async Task SubmitProjectExerciseCommand_LargeSizeFile_LargeFileSizeException()
    {
        var projectExerciseId = await GetProjectExerciseId();
        var studentId = await AddStudent("TestingUploading", "TestingUploading", "testingUploading@gmail.com");
        SubmitProjectExerciseCommand command =
            new SubmitProjectExerciseCommand(projectExerciseId, studentId, IFormFileFake(RandomString(5000005)));

        ValidationFailureException validationException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
        var largeFileSizeExceptionShown =
            IsErrorExists("File", "Размер загружаемого проекта не должен превышать 5 мегабайтов.", validationException);

        largeFileSizeExceptionShown.Should().BeTrue();
    }

    [Test]
    public async Task SubmitProjectExerciseCommand_UploadProjectError_False()
    {
        var studentId = await AddStudent("TestUploading", "TestUploading", "testUploading@gmail.com");
        var courseId = await AddCourse();
        await AddStudentCourse(courseId, studentId);
        var themeId = await AddTheme(courseId);
        var projectExerciseId = await AddProjectExcercise(themeId);

        var testSubmit = new SubmitProjectExerciseCommand(projectExerciseId, studentId, IFormFileFake(RandomString(5)));
        var serverResponseDTO = await SendAsync(testSubmit);

        serverResponseDTO.Success.Should().BeFalse();
    }

    [Test]
    public async Task SubmitProjectExerciseCommand_UploadProjectSuccess_True()
    {
        var studentId = await AddStudent("UploadingTest", "UploadingTest", "UploadingTest@gmail.com");
        var courseId = await AddCourse();
        await AddStudentCourse(courseId, studentId);
        var themeId = await AddTheme(courseId);
        var projectExerciseId = await AddProjectExcercise(themeId);

        var testSubmit = new SubmitProjectExerciseCommand(projectExerciseId, studentId, IFormFileFake(RandomString(5)));
        var serverResponseDTO = await SendAsync(testSubmit);

        serverResponseDTO.Success.Should().BeTrue();
    }

    IFormFile IFormFileFake(string content)
    {
        var fileName = "test.zip";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;

        var file = new FormFile(stream, 0, stream.Length, "from_form", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/x-zip-compressed"
        };
        file.ContentType = "application/x-zip-compressed";
        return file;
    }

    async Task<Guid> AddStudent(string firstName, string lastName, string email)
    {
        var student = new Student()
        {
            Id = Guid.NewGuid(),
            // FirstName = firstName,// "TestingUploading",
            // LastName = lastName,// "TestingUploading",
            // Email = email,// "testingUploading@gmail.com",
            Occupation = "Student"
        };
        await AddAsync(student);
        return student.Id;
    }

    async Task<Guid> AddCourse()
    {
        await RunAsTeacherAsync();
        var teacher = await GetAuthenticatedUser<Teacher>();

        var course = new Course()
        {
            Id = Guid.NewGuid(),
            LearningLanguage = "Tajik",
            Name = "C# Basics",
            TeacherId = teacher.Id
        };
        await AddAsync(course);
        return course.Id;


    }

    async Task<Guid> AddTheme(Guid courseId)
    {
        var theme = new Theme()
        {
            Id = Guid.NewGuid(),
            Name = "TestingTheme",
            Content = "Content",
            CourseId = courseId,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };
        await AddAsync(theme);
        return theme.Id;
    }

    async Task<Guid> AddProjectExcercise(Guid themeId)
    {
        var projectExercise = new ProjectExercise()
        {
            Id = Guid.NewGuid(),
            Description = "Description",
            Name = "Name",
            Rating = 5,
            ThemeId = themeId
        };
        await AddAsync(projectExercise);
        return projectExercise.Id;
    }

    async Task AddStudentCourse(Guid courseId, Guid studentId)
    {
        var studentCourse = new StudentCourse()
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            StudentId = studentId,
        };
        await AddAsync(studentCourse);
    }

    async Task<Guid> GetProjectExerciseId()
    {
        var courseId = await AddCourse();
        var themeId = await AddTheme(courseId);
        var projectExerciseId = await AddProjectExcercise(themeId);
        return projectExerciseId;
    }

    string RandomString(int size, bool lowerCase = false)
    {
        var builder = new StringBuilder(size);

        char offset = lowerCase ? 'a' : 'A';
        const int lettersOffset = 26;

        for (var i = 0; i < size; i++)
        {
            var @char = (char)(new Random()).Next(offset, offset + lettersOffset);
            builder.Append(@char);
        }

        return lowerCase ? builder.ToString().ToLower() : builder.ToString();
    }
}
