﻿namespace Domain.Entities
{
    public class Student : User, IEntity
    {
        public string Occupation { get; set; }
        public virtual ICollection<StudentCourse> Courses { get; set; } = new List<StudentCourse>();
    }
}
