using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.SeedData
{
    public class SeedDataModel
    {
        [SeedDataProperty(typeof(Contact), 6)]
        public List<Contact> Contacts { get; set; }

        [SeedDataProperty(typeof(Course), 7)]
        public List<Course> Courses { get; set; }

        [SeedDataProperty(typeof(Theme), 8)]
        public List<Theme> Themes { get; set; }

        [SeedDataProperty(typeof(Exercise), 9)]
        public List<Exercise> Exercises { get; set; }

        [SeedDataProperty(typeof(StudentCourse), 10)]
        public List<StudentCourse> StudentCourses { get; set; }

        [SeedDataProperty(typeof(StudentCourseExercise), 11)]
        public List<StudentCourseExercise> StudentCourseExercises { get; set; }
    }
}
