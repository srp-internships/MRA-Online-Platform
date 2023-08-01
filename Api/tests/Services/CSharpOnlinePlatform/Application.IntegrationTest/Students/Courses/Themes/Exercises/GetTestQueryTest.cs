using Application.Admin.Commands.StudentCommand;
using Application.Students.Queries;
using Domain.Entities;
using NUnit.Framework;
using System;
using FluentAssertions;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Students.Courses.Themes.Exercises
{
    public class GetTestQueryTest
    {
        [Test]
        public async Task GetTest_StudentAsyncTest()
        {
            await RunAsStudentAsync();

            var student = GetStudentCommand("Iyu", "iyu@mail.ru");
            await SendAsync(student);
            var iyu = await GetAsync<Student>(s => s.Email == student.Email);

            var course = CreateCourse();
            await AddAsync(course);

            var theme = CreateTheme(course.Id, DateTime.Now.AddDays(-1));
            await AddAsync(theme);

            var test = CreateTest("Test 1", 12, theme.Id);
            await AddAsync(test);

            var variant1 = CreateVariant(test.Id, "Correct");
            await AddAsync(variant1);

            var variant2 = CreateVariant(test.Id, "Not Correct 1");
            await AddAsync(variant2);

            var studentCourse = CreateStudentCourse(iyu.Id, course.Id);
            await AddAsync(studentCourse);

            var studentCourseTest = CreateStudentCourseTest(test.Id, studentCourse.Id);
            await AddAsync(studentCourseTest);

            GetTestsQuery query = new GetTestsQuery(theme.Id, iyu.Id);
            var testDTO = await SendAsync(query);
            testDTO[0].Status.Should().Be(Status.Passed);
            testDTO[0].Variants.Count.Should().Be(2);
        }

        #region TestData 
        VariantTest CreateVariant(Guid testId, string value)
        {
            return new VariantTest()
            {
                Id = Guid.NewGuid(),
                TestId = testId,
                Value = value
            };
        }

        Test CreateTest(string name, int rate, Guid themId)
        {
            return new Test()
            {
                Id = Guid.NewGuid(),
                ThemeId = themId,
                Rating = rate,
                Name = name,
                Description = "For test"
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

        StudentCourseTest CreateStudentCourseTest(Guid testId, Guid studentCourseId)
        {
            return new StudentCourseTest()
            {
                Id = Guid.NewGuid(),
                TestId = testId,
                StudentCourseId = studentCourseId,
                Date = DateTime.Now,
                Status = Status.Passed,
                Answer = ""
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

        CreateStudentCommand GetStudentCommand(string name, string email)
        {
            return new CreateStudentCommand()
            {
                FirstName = name,
                LastName = "Glick",
                Address = "PA, Lancaster",
                BirthDate = System.DateTime.Today,
                PhoneNumber = "992927770000",
                City = "Khujand",
                Country = "Tajikistan",
                Email = email,
                Occupation = "student",
                Password = "Pw12345@",
                Region = "Sogd",
                CourseName = "C# for beginners"
            };
        }
        #endregion
    }
}
