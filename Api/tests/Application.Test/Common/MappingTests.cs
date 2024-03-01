using Application.Common.Mapping;
using Application.Courses.DTO;
using Application.Exercises.DTO;
using Application.Teachers.Queries.ProjectExerciseQuery;
using Application.Themes.DTO;
using AutoMapper;
using Domain.Entities;
using NUnit.Framework;
using System;
using System.Runtime.Serialization;

namespace Application.Test.Common
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(conf => conf.AddProfile<MappingProfile>());
            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Test]
        [TestCase(typeof(Theme), typeof(ShortThemeDTO))]
        [TestCase(typeof(Course), typeof(CourseDTO))]
        [TestCase(typeof(Exercise), typeof(StudentExerciseDTO))]
        [TestCase(typeof(Exercise), typeof(ShortExerciseDTO))]
        [TestCase(typeof(Theme), typeof(ThemeDTO))]
        [TestCase(typeof(Course), typeof(ShortCourseDTO))]
        [TestCase(typeof(Domain.Entities.Test), typeof(TeacherTestDTO))]
        [TestCase(typeof(ProjectExercise), typeof(StudentProjectExerciseDTO))]
        [TestCase(typeof(ProjectExercise), typeof(GetProjectExerciseQueryDTO))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = GetInstanceOf(source);

            _mapper.Map(instance, source, destination);
        }

        private object GetInstanceOf(Type type)
        {
            if (type.GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance(type)!;

            // Type without parameterless constructor
            return FormatterServices.GetUninitializedObject(type);
        }
    }
}
