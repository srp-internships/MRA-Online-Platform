using Domain.Entities;
using NUnit.Framework;
using System;
using FluentAssertions;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;
using Application.CodeAnalyzer.Commands;
using Application.Teachers.Commands.TestCommand;
using System.Collections.Generic;
using Application.Exercises.DTO;

namespace Application.IntegrationTest.CodeAnalyzer.Commands
{
    public class AnalyzeTestCommandTest
    {
        [Test]
        public async Task AnalyzeTest_StudentAsyncTest()
        {
            await RunAsStudentAsync();

            var daliId = Guid.NewGuid();

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

            var studentCourse = CreateStudentCourse(daliId, course.Id);
            await AddAsync(studentCourse);

            var analyzeTestCommandParameter = new AnalyzeTestCommandParameter() { TestId = test.Id, VariantId = v1Id };
            var analyzeTest = new AnalyzeTestCommand(daliId, analyzeTestCommandParameter.TestId, analyzeTestCommandParameter.VariantId);
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
        #endregion
    }
}
