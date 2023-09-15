using System;
using System.Threading.Tasks;
using Application.Students.Queries;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Students.Courses.Themes.Exercises;

public class GetExerciseQueryTest
{
    [Test]
    public async Task GetExercise_ShouldReturnExercise_FromDataBaseTest()
    {
        await RunAsStudentAsync();

        // Create Students

        var alish = new Student { Id = new Guid(), Birthdate = DateTime.Now };
        await AddAsync(alish);

        var lion = new Student { Id = new Guid(), Birthdate = DateTime.Now };
        await AddAsync(lion);

        //Create Course for made up Students
        var course = CreateCourse();
        await AddAsync(course);

        //Create Theme for Course
        var theme = CreateTheme(course.Id, DateTime.Now.AddDays(-1));
        await AddAsync(theme);

        //Create Exercises for this Course and Students
        var variable = CreateExercise("Variables", 1, theme.Id);
        await AddAsync(variable);
        var dataType = CreateExercise("Data Types", 5, theme.Id);
        await AddAsync(dataType);

        //Link the Course to the Students and the Exercises to the StudentCourse for Alish
        var studentCourse = CreateStudentCourse(alish.Id, course.Id);
        await AddAsync(studentCourse);
        var studentCourseExercise =
            CreateStudentCourseExercise(studentCourse.Id, variable.Id, Status.Passed, DateTime.Now.AddDays(-5));
        await AddAsync(studentCourseExercise);
        studentCourseExercise =
            CreateStudentCourseExercise(studentCourse.Id, dataType.Id, Status.Failed, DateTime.Now);
        await AddAsync(studentCourseExercise);

        //Link the Course to the Students and the Exercises to the StudentCourse for Lion
        studentCourse = CreateStudentCourse(lion.Id, course.Id);
        await AddAsync(studentCourse);
        studentCourseExercise =
            CreateStudentCourseExercise(studentCourse.Id, variable.Id, Status.Failed, DateTime.Now.AddDays(-3));
        await AddAsync(studentCourseExercise);
        studentCourseExercise =
            CreateStudentCourseExercise(studentCourse.Id, dataType.Id, Status.Passed, DateTime.Now.AddDays(-10));
        await AddAsync(studentCourseExercise);

        //Testing
        GetExercisesQuery query = new GetExercisesQuery(theme.Id, alish.Id);
        var exerciseDTO = await SendAsync(query);

        exerciseDTO[0].Status.Should().Be(Status.Passed);
        exerciseDTO[1].Status.Should().Be(Status.Failed);

        query = new GetExercisesQuery(theme.Id, lion.Id);
        var exerciseDTO1 = await SendAsync(query);

        exerciseDTO1[0].Status.Should().Be(Status.Failed);
        exerciseDTO1[1].Status.Should().Be(Status.Passed);
    }

    [Test]
    public async Task GetExerciseWithStartDate_ShouldReturnExercise_FromDataBaseTest()
    {
        await RunAsStudentAsync();

        // Create Students
        var alish = new Student { Id = new Guid(), Birthdate = DateTime.Now };
        await AddAsync(alish);
            
        //Create Course for made up Students
        var course = CreateCourse();
        await AddAsync(course);

        //Create Theme for Course
        var theme = CreateTheme(course.Id, DateTime.Now.AddDays(1));
        await AddAsync(theme);

        //Create Exercises for this Course and Students
        var variable = CreateExercise("Variables", 1, theme.Id);
        await AddAsync(variable);
        var dataType = CreateExercise("Data Types", 5, theme.Id);
        await AddAsync(dataType);

        //Testing
        GetExercisesQuery query = new GetExercisesQuery(theme.Id, alish.Id);
        var exerciseDTO = await SendAsync(query);

        exerciseDTO.Count.Should().Be(0);
    }

    [Test]
    public async Task GetExerciseStatus_ShouldReturnExercise_FromDataBaseTest()
    {
        //arrange
        await RunAsStudentAsync();
        var student = await GetAuthenticatedUser<Student>();

        var course = CreateCourse();
        await AddAsync(course);
        var theme = CreateTheme(course.Id, DateTime.Now.AddDays(-1));
        await AddAsync(theme);

        var variable = CreateExercise("Variables", 1, theme.Id);
        await AddAsync(variable);

        var studentCourse = CreateStudentCourse(student.Id, course.Id);
        await AddAsync(studentCourse);

        var studentCourseExercise =
            CreateStudentCourseExercise(studentCourse.Id, variable.Id, Status.Passed, DateTime.Now);
        await AddAsync(studentCourseExercise);

        studentCourseExercise =
            CreateStudentCourseExercise(studentCourse.Id, variable.Id, Status.Failed, DateTime.Now);
        await AddAsync(studentCourseExercise);

        //act
        var query = new GetExercisesQuery(theme.Id, student.Id);
        var exerciseDTO = await SendAsync(query);

        //assert
        exerciseDTO[0].Status.Should().Be(Status.Passed);
    }

    #region TestData

    StudentCourseExercise CreateStudentCourseExercise(Guid studentCourseId, Guid exerciseId, Status status,
        DateTime date)
    {
        return new StudentCourseExercise()
        {
            Id = Guid.NewGuid(),
            StudentCourseId = studentCourseId,
            ExerciseId = exerciseId,
            Status = status,
            Date = date,
            Code = ""
        };
    }

    Exercise CreateExercise(string name, int rate, Guid themeId)
    {
        return new Exercise()
        {
            Id = Guid.NewGuid(),
            ThemeId = themeId,
            Rating = rate,
            Name = name,
            Description = "For test",
            Template = "Template",
            Test = "Test"
        };
    }

    Theme CreateTheme(Guid courseId, DateTime startDate)
    {
        return new Theme()
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            Name = $"Chapter 1",
            Content = "Test",
            StartDate = startDate
        };
    }

    StudentCourse CreateStudentCourse(Guid studentId, Guid courseID)
    {
        return new StudentCourse()
        {
            Id = Guid.NewGuid(),
            CourseId = courseID,
            StudentId = studentId
        };
    }

    Course CreateCourse()
    {
        return new Course()
        {
            Id = Guid.NewGuid(),
            Name = "C# Basic",
            LearningLanguage = "Tajik"
        };
    }
        
    #endregion
}