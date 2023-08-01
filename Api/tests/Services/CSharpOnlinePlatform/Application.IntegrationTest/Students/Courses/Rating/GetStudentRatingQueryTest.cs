using Application.Admin.Commands.StudentCommand;
using Application.Students.Queries;
using Domain.Entities;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Students.Courses.Rating
{
    public class GetStudentRatingQueryTest
    {
        [Test]
        public async Task GetStudentRating_TestAsync()
        {
            await RunAsStudentAsync();

            // Create Students
            var student = GetStudentCommand("Alex", "alex@mail.ru");
            await SendAsync(student);
            var alex = await GetAsync<Student>(s => s.Email == student.Email);
            student = GetStudentCommand("Baha", "baha@mail.ru");
            await SendAsync(student);
            var baha = await GetAsync<Student>(s => s.Email == student.Email);
            student = GetStudentCommand("Alik", "alik@mail.ru");
            await SendAsync(student);
            var alik = await GetAsync<Student>(s => s.Email == student.Email);

            //Create Course for made up Students
            var course = CreateCourse();
            await AddAsync(course);

            //Create Theme for Course
            var theme = CreateTheme(course.Id, DateTime.Now.AddDays(-2));
            await AddAsync(theme);

            //Create Exercises for this Course and Students
            var variable = await CreateExercise("Variables", 1, theme.Id);
            var typeCasting = await CreateExercise("Type Casting", 10, theme.Id);
            var dataType = await CreateExercise("Data Types", 5, theme.Id);

            //Link the Course to the Students and the Exercises to the StudentCourse for Alex
            var studentCourse = await CreateStudentCourse(alex.Id, course.Id);
            await CreateStudentCourseExercises(variable, Status.Passed, typeCasting, Status.Passed, dataType, Status.Passed, studentCourse);

            //Link the Course to the Students and the Exercises to the StudentCourse for Baha
            studentCourse = await CreateStudentCourse(baha.Id, course.Id);
            await CreateStudentCourseExercises(variable, Status.Passed, typeCasting, Status.Failed, dataType, Status.Passed, studentCourse);

            //Link the Course to the Students and the Exercises to the StudentCourse for Alik
            studentCourse = await CreateStudentCourse(alik.Id, course.Id);
            await CreateStudentCourseExercises(variable, Status.Failed, typeCasting, Status.Passed, dataType, Status.Passed, studentCourse);

            //Testing
            GetStudentRatingQuery alexRate = new GetStudentRatingQuery(course.Id, alex.Id);
            var ratingDTO = await SendAsync(alexRate);

            ratingDTO.TotalRate.Should().Be(16);

            ratingDTO.CompletedRate.Should().Be(16);
            ratingDTO.Position.Should().Be(1);

            var bahaRate = new GetStudentRatingQuery(course.Id, baha.Id);
            ratingDTO = await SendAsync(bahaRate);

            ratingDTO.CompletedRate.Should().Be(6);
            ratingDTO.Position.Should().Be(3);

            var alikRate = new GetStudentRatingQuery(course.Id, alik.Id);
            ratingDTO = await SendAsync(alikRate);

            ratingDTO.CompletedRate.Should().Be(15);
            ratingDTO.Position.Should().Be(2);
        }

        private async Task CreateStudentCourseExercises(Exercise variable, Status statusVariable, Exercise typeCasting, Status statusTypeCasting, Exercise dataType, Status statusDataType, StudentCourse studentCourse)
        {
            await CreateStudentCourseExercise(studentCourse.Id, variable.Id, statusVariable);
            await CreateStudentCourseExercise(studentCourse.Id, typeCasting.Id, statusTypeCasting);
            await CreateStudentCourseExercise(studentCourse.Id, dataType.Id, statusDataType);
        }

        #region TestData 
        async Task<StudentCourseExercise> CreateStudentCourseExercise(Guid studentCourseId, Guid exerciseId, Status status)
        {
            var studentCourseExercise = new StudentCourseExercise()
            {
                StudentCourseId = studentCourseId,
                ExerciseId = exerciseId,
                Status = status,
                Code = "string"
            };
            await AddAsync(studentCourseExercise);
            return studentCourseExercise;
        }

        async Task<Exercise> CreateExercise(string name, int rate, Guid themId)
        {
            var exercise = new Exercise()
            {
                Id = Guid.NewGuid(),
                ThemeId = themId,
                Rating = rate,
                Name = name,
                Description = "For test",
                Template = "Template",
                Test = "Test"
            };
            await AddAsync(exercise);
            return exercise;
        }

        Theme CreateTheme(Guid courseId, DateTime startDate)
        {
            return new Theme()
            {
                Id = Guid.NewGuid(),
                CourseId = courseId,
                Name = $"Chapter 1",
                Content = "Test",
                StartDate = startDate,
                EndDate = startDate.AddDays(1)
            };
        }

        async Task<StudentCourse> CreateStudentCourse(Guid studentId, Guid courseID)
        {
            var studentCourse = new StudentCourse()
            {
                Id = Guid.NewGuid(),
                CourseId = courseID,
                StudentId = studentId
            };
            await AddAsync(studentCourse);
            return studentCourse;
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
