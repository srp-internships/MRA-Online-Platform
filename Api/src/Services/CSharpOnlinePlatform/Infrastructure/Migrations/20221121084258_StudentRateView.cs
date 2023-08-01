using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class StudentRateView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE or ALTER VIEW StudentRateView as 
SELECT  [StudentId] AS Id,
		[AspNetUsers].FirstName,
		[AspNetUsers].LastName,
        Count([StudentCourseExercise].Status) AS TotalSubmit,
		COALESCE(Sum(CASE WHEN [StudentCourseExercise].[Status] = 1  THEN [Exercise].[Rating] END), 0) AS TotalRate,
		CAST(ROW_NUMBER() OVER(ORDER BY COALESCE(Sum(CASE WHEN [StudentCourseExercise].[Status] = 1  THEN [Exercise].[Rating] END), 0) DESC,Count([StudentCourseExercise].Status) DESC) as INT) AS Position
  FROM [StudentCourse] 
		inner join [StudentCourseExercise] on [StudentCourse].Id = [StudentCourseExercise].StudentCourseId
        inner join AspNetUsers on [StudentCourse].StudentId=[AspNetUsers].Id 
		inner join [Exercise] on [StudentCourseExercise].ExerciseId=[Exercise].Id 
GROUP BY [StudentCourse].StudentId,[AspNetUsers].FirstName,[AspNetUsers].LastName");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW StudentRateView;");
        }
    }
}
