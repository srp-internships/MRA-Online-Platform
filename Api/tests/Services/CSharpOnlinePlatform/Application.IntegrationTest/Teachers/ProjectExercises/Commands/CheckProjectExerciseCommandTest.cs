using Application.Teachers.Commands.ProjectExerciseCommand.CheckProjectExercise;
using Core.Exceptions;
using Domain.Entities;
using NUnit.Framework;
using System;
using FluentAssertions;
using static Application.IntegrationTest.TestHelper;
using Core.ValidationsBehaviours;
using System.Threading.Tasks;

namespace Application.IntegrationTest.Teachers.ProjectExercises.Commands;

public class CheckProjectExerciseCommandTests
{
    [Test]
    public async Task CheckProjectExerciseCommand_NotExistingProjectId_NotFoundException()
    {
        await GetProjectExerciseId();
        var command = GetCheckProjectExerciseCommand(Guid.NewGuid(), string.Empty, Status.Passed, Guid.NewGuid());

        ValidationFailureException validationException =
            Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
        var projectExerciseNotFoundExceptionShown =
            IsErrorExists("ProjectExerciseId", "Проект не найден.", validationException);

        projectExerciseNotFoundExceptionShown.Should().BeTrue();
    }

    [Test]
    public async Task CheckProjectExerciseCommand_EmptyComment_NotEmptyException()
    {
        var projectExerciseId = await GetProjectExerciseId();
        var command = GetCheckProjectExerciseCommand(projectExerciseId, string.Empty, Status.Failed, Guid.NewGuid());

        ValidationFailureException validationException =
            Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
        var commentEmptyExceptionShown = IsErrorExists("Comment", ValidationMessages.GetNotEmptyMessage("Comment"),
            validationException);

        commentEmptyExceptionShown.Should().BeTrue();
    }

    [Test]
    public async Task CheckProjectExerciseCommand_NotExistingTeacherId_ForbiddenException()
    {
        var projectExerciseId = await GetProjectExerciseId();
        var command = GetCheckProjectExerciseCommand(projectExerciseId, "Comment1", Status.Failed, Guid.NewGuid());

        ValidationFailureException validationException =
            Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
        var forbiddenExceptionShown = IsErrorExists("TeacherId", "Ваш доступ ограничен.", validationException);

        forbiddenExceptionShown.Should().BeTrue();
    }

    [Test]
    public async Task CheckProjectExerciseCommand_CorrectData_NotNull()
    {
        var teacherId = Guid.NewGuid();
        var courseId = await AddCourse(teacherId);
        var themeId = await AddTheme(courseId);
        var projectExerciseId = await AddProjectExcercise(themeId);
        
        
        
        await AddStudentProjectExercise(courseId, projectExerciseId);
        var command = GetCheckProjectExerciseCommand(projectExerciseId,
            "Comment", Status.Failed, teacherId);

        var projectExerciseIdReturn = await SendAsync(command);
        var databaseProjectExercise = await FindAsync<StudentCourseProjectExercise>(projectExerciseIdReturn);

        databaseProjectExercise.Should().NotBeNull();
        databaseProjectExercise.Id.Should().Be(projectExerciseIdReturn);
    }


    async Task AddStudentProjectExercise(Guid courseId, Guid projectExerciseId)
    {
        var studentId = Guid.NewGuid();
        var studentCourse = new StudentCourse()
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            StudentId = studentId,
        };
        await AddAsync(studentCourse);
        var newStudentCourseProjectExercise = new StudentCourseProjectExercise
        {
            ProjectExerciseId = projectExerciseId,
            StudentCourseId = studentCourse.Id,
            Status = Status.WaitForTeacher,
            LinkToProject = "",
            Date = DateTime.Now,
            Comment = ""
        };
        await AddAsync(newStudentCourseProjectExercise);
    }

    async Task<Guid> AddCourse(Guid teacherId)
    {
        var course = new Course()
        {
            Id = Guid.NewGuid(),
            LearningLanguage = "Tajik",
            Name = "C# Basics",
            TeacherId = teacherId
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

    async Task<Guid> GetProjectExerciseId()
    {
        var teacherId = Guid.NewGuid();
        var courseId = await AddCourse(teacherId);
        var themeId = await AddTheme(courseId);
        var projectExerciseId = await AddProjectExcercise(themeId);
        return projectExerciseId;
    }

    CheckProjectExerciseCommand GetCheckProjectExerciseCommand(Guid projectExerciseId, string comment, Status status,
        Guid teacherId)
    {
        var checkProjectExerciseCommandDTO = new CheckProjectExerciseCommandDTO()
        {
            ProjectExerciseId = projectExerciseId,
            Comment = comment,
            Status = status
        };
        return new CheckProjectExerciseCommand(checkProjectExerciseCommandDTO, teacherId);
    }
}