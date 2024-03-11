using Domain.Entities;

namespace Application.Common.BL
{
    public static class BLService
    {
        public static DateTime? GetStartDate(this Course course)
        {
            var themes = course.Themes;
            if (themes.Any())
            {
                return themes.Select(theme => theme.StartDate).Min();
            }
            else
            {
                return null;
            }
        }

        public static DateTime? GetEndDate(this Course course)
        {
            var themes = course.Themes;
            if (themes.Any())
            {
                return themes.Select(theme => theme.EndDate).Max();
            }
            else
            {
                return null;
            }
        }

        public static int GetCompletedThemesCount(this Course course)
        {
            var quantityCompletedExercises = 0;

            var themes = course.Themes.Where(s => s.EndDate >= DateTime.Now).ToList();

            foreach (var theme in themes)
            {
                if (theme.Exercises.Any() && theme.Exercises.All(s => s.Students.Where(s => s.Status == Status.Passed).Any()) &&
                    theme.Tests.Any() && theme.Tests.All(s => s.Students.Any()))
                {
                    quantityCompletedExercises++;
                }
            }
            return quantityCompletedExercises;
        }

        public static int GetExpiredPassedThemesCount(this Course course)
        {
            var quantityExpiredThemes = 0;

            var themes = course.Themes.Where(s => s.EndDate < DateTime.Today).ToList();

            foreach (var theme in themes)
            {
                if (theme.Exercises.All(s => s.Students.Where(s => s.Status == Status.Passed).Any())
                    && theme.Tests.All(s => s.Students.Any()))
                {
                    quantityExpiredThemes++;
                }
            }
            return quantityExpiredThemes;
        }

        public static int GetExpiredThemesCount(this Course course)
        {
            return course.Themes.Count(s => s.EndDate < DateTime.Now);
        }
    }
}
