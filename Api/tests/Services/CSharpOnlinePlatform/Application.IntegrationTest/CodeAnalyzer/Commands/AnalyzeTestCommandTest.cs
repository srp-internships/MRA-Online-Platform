using Application.Admin.Commands.StudentCommand;
using Domain.Entities;
using NUnit.Framework;
using System;
using FluentAssertions;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;
using Application.CodeAnalyzer.Commands;
using Application.Teachers.Commands.TestCommand;
using System.Collections.Generic;
using System.Linq;
using Application.Exercises.DTO;

namespace Application.IntegrationTest.CodeAnalyzer.Commands
{
    public class AnalyzeTestCommandTest
    {
        [Test]
        public async Task AnalyzeTest_StudentAsynTest()
        {
            await RunAsStudentAsync();

            var student = GetStudentCommand("Dali", "dali@mail.ru");
            await SendAsync(student);
            var dali = await GetAsync<Student>(s => s.Email == student.Email);

            var course = CreateCourse();
            await AddAsync(course);

            var theme = CreateTheme(course.Id, DateTime.Now.AddDays(-1));
            await AddAsync(theme);

            var test = CreateTest("Test 1", 10, theme.Id);
            await AddAsync(test);

            var v1Id = Guid.NewGuid();
            var v2Id = Guid.NewGuid();
            var variant1 = CreateVariant(test, v1Id, v2Id);
            await SendAsync(variant1);

            var studentCourse = CreateStudentCourse(dali.Id, course.Id);
            await AddAsync(studentCourse);

            var analyzeTestCommandParameter = new AnalyzeTestCommandParameter() { TestId = test.Id, VariantId = v1Id };
            var analyzeTest = new AnalyzeTestCommand(dali.Id, analyzeTestCommandParameter.TestId, analyzeTestCommandParameter.VariantId);
            var analyzeDTO = await SendAsync(analyzeTest);
            analyzeDTO.Success.Should().BeTrue();
        }

        #region TestData 
        UpdateVariantCommand CreateVariant(Test test, Guid variantId1, Guid variantId2)
        {
            List<VariantTestDTO> variants = new List<VariantTestDTO>();
            variants.Add(new VariantTestDTO
            {
                Id = variantId1,
                Value = "Correct",
                TestId = test.Id,
                IsCorrect = true
            });
            variants.Add(new VariantTestDTO
            {
                Id = variantId2,
                Value = "Not Correct 1",
                TestId = test.Id,
                IsCorrect = false
            });
            return new UpdateVariantCommand(variants.ToArray());
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
