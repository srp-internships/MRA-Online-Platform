using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.SeedData
{
    public class SeedDataModel
    {
        [SeedDataProperty(typeof(IdentityRole<Guid>), 1)]
        public List<IdentityRole<Guid>> Roles { get; set; }

        [SeedDataProperty(typeof(Admin), 2)]
        public List<Admin> Admins { get; set; }

        [SeedDataProperty(typeof(IdentityUserRole<Guid>), 5.5)]
        public List<IdentityUserRole<Guid>> UserRoles { get; set; }

        [SeedDataProperty(typeof(Student), 4)]
        public List<Student> Students { get; set; }

        [SeedDataProperty(typeof(Teacher), 5)]
        public List<Teacher> Teachers { get; set; }

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
