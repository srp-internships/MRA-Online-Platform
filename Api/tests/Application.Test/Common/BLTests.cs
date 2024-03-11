using Application.Common.BL;
using Domain.Entities;
using NUnit.Framework;
using System;


namespace Application.Test.Common
{
    public class BLTests
    {
        [Test]
        public void Course_StartDate_Should_Be_Calculated_From_Themes()
        {
            var course = new Course();
            var expectedStartDate = new DateTime(2022, 1, 3);

            course.Themes.Add(new Theme
            {
                StartDate = new DateTime(2022, 1, 27),
                EndDate = new DateTime(2022, 2, 2),
            });
            course.Themes.Add(new Theme
            {
                StartDate = new DateTime(2022, 2, 5),
                EndDate = new DateTime(2022, 2, 15),
            });
            course.Themes.Add(new Theme
            {
                StartDate = new DateTime(2022, 3, 10),
                EndDate = new DateTime(2022, 3, 15),
            });

            course.Themes.Add(new Theme
            {
                StartDate = new DateTime(2022, 1, 3),
                EndDate = new DateTime(2022, 1, 7),
            });
            Assert.That(course.GetStartDate(), Is.EqualTo(expectedStartDate));
        }

        [Test]
        public void Course_StartDate_Should_Be_Null_WhenThemesAreEmpty()
        {
            var course = new Course();
            Assert.That(course.GetStartDate(), Is.Null);
        }

        [Test]
        public void Course_EndDate_Should_Be_Calculated_From_Themes()
        {
            var course = new Course();
            var expectedEndDate = new DateTime(2022, 3, 15);

            course.Themes.Add(new Theme
            {
                StartDate = new DateTime(2022, 1, 27),
                EndDate = new DateTime(2022, 2, 2),
            });
            course.Themes.Add(new Theme
            {
                StartDate = new DateTime(2022, 2, 5),
                EndDate = new DateTime(2022, 2, 15),
            });
            course.Themes.Add(new Theme
            {
                StartDate = new DateTime(2022, 3, 10),
                EndDate = new DateTime(2022, 3, 15),
            });

            course.Themes.Add(new Theme
            {
                StartDate = new DateTime(2022, 1, 3),
                EndDate = new DateTime(2022, 1, 7),
            });
            Assert.That(course.GetEndDate(), Is.EqualTo(expectedEndDate));
        }

        [Test]
        public void Course_EndDate_Should_Be_Null_WhenThemesAreEmpty()
        {
            var course = new Course();
            Assert.That(course.GetEndDate(), Is.Null);
        }

        [Test]
        public void Course_CompletedThemes_ShouldGetCountOfThemes_WhereEndDateIsExpiredTest()
        {
            var course = new Course();
            course.Themes.Add(new Theme
            {
                StartDate = new DateTime(2022, 1, 27),
                EndDate = DateTime.Today.AddDays(5),//End date is not expired 
            });
            course.Themes.Add(new Theme
            {
                StartDate = new DateTime(2022, 2, 5),
                EndDate = DateTime.Today.AddDays(-5),//End date is expired
            });
            Assert.That(course.GetExpiredThemesCount(), Is.EqualTo(1));
        }
    }
}

