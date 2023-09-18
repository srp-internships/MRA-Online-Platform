using Domain.Entities;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Teachers.Queries.CourseQuery;
using Application.Teachers.Queries.ExerciseQuery;
using Application.Teachers.Queries.ThemeQuery;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Teachers;

public class GetTeacherQueryTest
{
    [Test]
    public async Task GetCourse_ShouldReturnCourseOfTeacher()
    {
        var teacherId = Guid.NewGuid();
        var course = CreateCourse(teacherId);
        await AddAsync(course);

        var query = new GetTeacherCoursesQuery(teacherId);
        var courses = await SendAsync(query);
        Assert.That(courses.Any(c => c.Id == course.Id), Is.True);
    }

    [Test]
    public async Task GetThemes_ShouldReturnThemesOfCourseOfTeacher()
    {
        var teacherId = Guid.NewGuid();
        var course = CreateCourse(teacherId);
        await AddAsync(course);
        var theme1 = CreateTheme(course);
        await AddAsync(theme1);
        var theme2 = CreateTheme(course);
        await AddAsync(theme2);

        var query = new GetTeacherThemesQuery(course.Id, teacherId);
        var themes = await SendAsync(query);
        Assert.That(themes.Any(t => t.Id == theme1.Id), Is.True);
        Assert.That(themes.Any(t => t.Id == theme2.Id), Is.True);
    }

    [Test]
    public async Task GetExercises_ShouldReturnExercisesOfThemeOfCourseOfTeacher()
    {
        var teacherId = Guid.NewGuid();
        var course = CreateCourse(teacherId);
        await AddAsync(course);
        var theme = CreateTheme(course);
        await AddAsync(theme);
        var exercise = CreateExercise(theme);
        await AddAsync(exercise);

        var query = new GetExercisesTeacherQuery(theme.Id, teacherId);
        var exercises = await SendAsync(query);
        Assert.That(exercises.Any(e => e.Id == exercise.Id), Is.True);
    }

    #region Test Data

    Course CreateCourse(Guid teacherId)
    {
        return new Course()
        {
            Id = Guid.NewGuid(),
            Name = "C# Threading",
            LearningLanguage = "Tajik",
            TeacherId = teacherId
        };
    }

    Theme CreateTheme(Course course)
    {
        return new Theme()
        {
            Id = Guid.NewGuid(),
            Name = "Class Task",
            Content =
                "Одна задача может запускать другую - вложенную задачу. При этом эти задачи выполняются независимо друг от друга.",
            StartDate = new DateTime(2022, 07, 15),
            EndDate = new DateTime(2022, 07, 22),
            CourseId = course.Id
        };
    }

    Exercise CreateExercise(Theme theme)
    {
        return new Exercise()
        {
            Id = Guid.NewGuid(),
            Name = $"Task.WaitAny(tasks) {theme.Id}",
            ThemeId = theme.Id,
            Description = "Возвращение результатов из задач",
            Test = "",
            Template = $"{theme.Id}"
        };
    }

    #endregion
}