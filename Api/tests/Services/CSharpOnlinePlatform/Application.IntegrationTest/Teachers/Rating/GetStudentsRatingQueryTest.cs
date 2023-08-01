using Application.Admin.Commands.StudentCommand;
using Application.Students.Queries;
using Domain.Entities;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using static Application.IntegrationTest.TestHelper;
using Application.Teachers.Queries.RatingQuery;
using System.Linq;

namespace Application.IntegrationTest.Teachers.Rating
{
    public class GetStudentsRatingQueryTest
    {
        [Test]
        public async Task GetRatingTeacher_TestAsync()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();

            // Create Students
            var student = GetStudentCommand("Karim", "karim@mail.ru");
            await SendAsync(student);
            var karim = await GetAsync<Student>(s => s.Email == student.Email);
            student = GetStudentCommand("Ibrohim", "ibrohim@mail.ru");
            await SendAsync(student);
            var ibrohim = await GetAsync<Student>(s => s.Email == student.Email);
            student = GetStudentCommand("Akmal", "akmal@mail.ru");
            await SendAsync(student);
            var akmal = await GetAsync<Student>(s => s.Email == student.Email);

            var course = await CreateTestData(teacher, karim: karim, ibrohim: ibrohim, akmal: akmal);

            //Testing
            GetStudentsRatingQuery query = new GetStudentsRatingQuery(course.Id, teacher.Id);
            var ratingDTO = await SendAsync(query);

            var studentDTO = ratingDTO.Single(s => s.FullName == karim.FullName);
            studentDTO.TotalRate.Should().Be(16);
            studentDTO.TotalSubmit.Should().Be(4);

            studentDTO = ratingDTO.Single(s => s.FullName == ibrohim.FullName);
            studentDTO.TotalRate.Should().Be(6);
            studentDTO.TotalSubmit.Should().Be(3);

            studentDTO = ratingDTO.Single(s => s.FullName == akmal.FullName);
            studentDTO.TotalRate.Should().Be(15);
            studentDTO.TotalSubmit.Should().Be(3);
        }

        private async Task<Course> CreateTestData(Teacher teacher, Student karim, Student ibrohim, Student akmal)
        {
            //Create Course for made up Students
            var course = CreateCourse(teacher.Id);
            await AddAsync(course);

            //Create Theme for Course
            var theme = CreateTheme(course.Id, DateTime.Now.AddDays(-1));
            await AddAsync(theme);

            //Create Exercises for this Course and Students
            var variable = CreateExercise("Variables", 1, theme.Id);
            await AddAsync(variable);
            var typeCasting = CreateExercise("Type Casting", 10, theme.Id);
            await AddAsync(typeCasting);
            var dataType = CreateExercise("Data Types", 5, theme.Id);
            await AddAsync(dataType);

            //Link the Course to the Students and the Exercises to the StudentCourse for Karim
            var studentCourse = CreateStudentCourse(karim.Id, course.Id);
            await AddAsync(studentCourse);
            var studentCourseExercise = CreateStudentCourseExercise(studentCourse.Id, variable.Id, Status.Passed);
            await AddAsync(studentCourseExercise);
            studentCourseExercise = CreateStudentCourseExercise(studentCourse.Id, typeCasting.Id, Status.Passed);
            await AddAsync(studentCourseExercise);
            studentCourseExercise = CreateStudentCourseExercise(studentCourse.Id, dataType.Id, Status.Failed);
            await AddAsync(studentCourseExercise);
            studentCourseExercise = CreateStudentCourseExercise(studentCourse.Id, dataType.Id, Status.Passed);
            await AddAsync(studentCourseExercise);

            //Link the Course to the Students and the Exercises to the StudentCourse for Ibrohim
            studentCourse = CreateStudentCourse(ibrohim.Id, course.Id);
            await AddAsync(studentCourse);
            studentCourseExercise = CreateStudentCourseExercise(studentCourse.Id, variable.Id, Status.Passed);
            await AddAsync(studentCourseExercise);
            studentCourseExercise = CreateStudentCourseExercise(studentCourse.Id, typeCasting.Id, Status.Failed);
            await AddAsync(studentCourseExercise);
            studentCourseExercise = CreateStudentCourseExercise(studentCourse.Id, dataType.Id, Status.Passed);
            await AddAsync(studentCourseExercise);

            //Link the Course to the Students and the Exercises to the StudentCourse for Akmal
            studentCourse = CreateStudentCourse(akmal.Id, course.Id);
            await AddAsync(studentCourse);
            studentCourseExercise = CreateStudentCourseExercise(studentCourse.Id, variable.Id, Status.Failed);
            await AddAsync(studentCourseExercise);
            studentCourseExercise = CreateStudentCourseExercise(studentCourse.Id, typeCasting.Id, Status.Passed);
            await AddAsync(studentCourseExercise);
            studentCourseExercise = CreateStudentCourseExercise(studentCourse.Id, dataType.Id, Status.Passed);
            await AddAsync(studentCourseExercise);
            return course;
        }

        #region TestData 
        StudentCourseExercise CreateStudentCourseExercise(Guid studentCourseId, Guid exerciseId, Status status)
        {
            return new StudentCourseExercise()
            {
                StudentCourseId = studentCourseId,
                ExerciseId = exerciseId,
                Status = status,
                Code = "string"
            };
        }

        Exercise CreateExercise(string name, int rate, Guid themId)
        {
            return new Exercise()
            {
                Id = Guid.NewGuid(),
                ThemeId = themId,
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

        Course CreateCourse(Guid teacherId)
        {
            return new Course()
            {
                Id = Guid.NewGuid(),
                TeacherId = teacherId,
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
