using Application.Account.Services;
using Application.Courses.DTO;
using Application.Documentations;
using Application.Documentations.DTO;
using Application.Exercises.DTO;
using Application.Teachers.Commands.CourseCommand;
using Application.Teachers.Commands.ExerciseCommand;
using Application.Teachers.Commands.ProjectExerciseCommand.CheckProjectExercise;
using Application.Teachers.Commands.ProjectExerciseCommand.CreateProjectExercise;
using Application.Teachers.Commands.ProjectExerciseCommand.DeleteProjectExercise;
using Application.Teachers.Commands.ProjectExerciseCommand.UpdateProjectExercise;
using Application.Teachers.Commands.TestCommand;
using Application.Teachers.Commands.ThemeCommands;
using Application.Teachers.DTO;
using Application.Teachers.Queries.CourseQuery;
using Application.Teachers.Queries.ExerciseQuery;
using Application.Teachers.Queries.ProjectExerciseQuery;
using Application.Teachers.Queries.RatingQuery;
using Application.Teachers.Queries.StudentCourseProjectExerciseQuery;
using Application.Teachers.Queries.TestsQuery;
using Application.Teachers.Queries.ThemeQuery;
using Application.Themes.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Authorize(Roles = "teacher")]
    public class TeacherController : ApiControllerBase
    {
        private readonly IUserHttpContextAccessor _userHttpContextAccessor;

        public TeacherController(IUserHttpContextAccessor userHttpContextAccessor)
        {
            _userHttpContextAccessor = userHttpContextAccessor;
        }
        
        [HttpGet("api/[controller]/Rating/{courseId}")]
        public async Task<ActionResult<List<GetRatingDTO>>> GetStudentsRating(Guid courseId)
        {
            return Ok(await Mediator.Send(new GetStudentsRatingQuery(courseId, _userHttpContextAccessor.GetUserId())));
        }

        [HttpGet("api/[controller]/help")]
        public async Task<ActionResult<DocumentationDTO>> GetDocumentation()
        {
            return await Mediator.Send(new GetDocumentationQuery(DocumentArea.Teacher));
        }

        [HttpGet("api/[controller]/GetStudentProjectExercise/{projectExerciseId}")]
        public async Task<ActionResult<List<GetStudentCourseProjectExerciseDTO>>> GetStudentProjectExercise(Guid projectExerciseId)
        {
            return Ok(await Mediator.Send(new GetStudentCourseProjectExerciseQuery(projectExerciseId, _userHttpContextAccessor.GetUserId())));
        }

        #region Course

        [HttpPost("api/[controller]/CreateCourse")]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseCommandDTO createCourseDTO)
        {
            return Ok(await Mediator.Send(new CreateCourseCommand(_userHttpContextAccessor.GetUserId(),
                createCourseDTO.Name, createCourseDTO.CourseLanguage)));
        }

        [HttpPut("api/[controller]/UpdateCourse")]
        public async Task<IActionResult> UpdateCourse([FromBody] UpdateCourseCommandDTO updateCourseDTO)
        {
            return Ok(await Mediator.Send(new UpdateCourseCommand(_userHttpContextAccessor.GetUserId(),
                updateCourseDTO.Id, updateCourseDTO.Name, updateCourseDTO.CourseLanguage)));
        }

        [HttpGet("api/[controller]/Courses")]
        public async Task<ActionResult<List<GetCourseDTO>>> GetCourses()
        {
            return Ok(await Mediator.Send(new GetTeacherCoursesQuery(_userHttpContextAccessor.GetUserId())));
        }

        [HttpDelete("api/[controller]/DeleteCourse/{courseId}")]
        public async Task<ActionResult> DeleteCourse(Guid courseId)
        {
            return Ok(await Mediator.Send(new DeleteCourseCommand(_userHttpContextAccessor.GetUserId(), courseId)));
        }
        #endregion

        #region Theme

        [HttpPost("api/[controller]/CreateTheme")]
        public async Task<IActionResult> CreateTheme([FromBody] CreateThemeCommandDTO createThemeDTO)
        {
            return Ok(await Mediator.Send(new CreateThemeCommand(_userHttpContextAccessor.GetUserId(),
                createThemeDTO.Name, createThemeDTO.Content, createThemeDTO.StartDate, createThemeDTO.EndDate,
                createThemeDTO.CourseId)));
        }

        [HttpPut("api/[controller]/UpdateTheme")]
        public async Task<IActionResult> UpdateTheme([FromBody] UpdateThemeCommandDTO updateThemeDTO)
        {
            return Ok(await Mediator.Send(new UpdateThemeCommand(updateThemeDTO.Id, updateThemeDTO.Name,
                updateThemeDTO.Content, updateThemeDTO.StartDate, updateThemeDTO.EndDate,
                _userHttpContextAccessor.GetUserId())));
        }

        [HttpGet("api/[controller]/Themes/{courseId}")]
        public async Task<ActionResult<List<ShortThemeDTO>>> GetThemes(Guid courseId)
        {
            return Ok(await Mediator.Send(new GetTeacherThemesQuery(courseId, _userHttpContextAccessor.GetUserId())));
        }

        [HttpGet("api/[controller]/Theme/{themeId}")]
        public async Task<ActionResult<ThemeDTO>> GetTheme(Guid themeId)
        {
            return Ok(await Mediator.Send(new GetTeacherThemeQuery(themeId, _userHttpContextAccessor.GetUserId())));
        }

        [HttpDelete("api/[controller]/DeleteTheme/{themeId}")]
        public async Task<ActionResult> DeleteTheme(Guid themeId)
        {
            return Ok(await Mediator.Send(new DeleteThemeCommand(themeId, _userHttpContextAccessor.GetUserId())));
        }
        #endregion

        #region Exercise

        [HttpGet("api/[controller]/Exercises/{themeId}")]
        public async Task<ActionResult<List<TeacherExerciseDTO>>> GetExercises(Guid themeId)
        {
            return Ok(await Mediator.Send(new GetExercisesTeacherQuery(themeId, _userHttpContextAccessor.GetUserId())));
        }

        [HttpPost("api/[controller]/CreateExercise")]
        public async Task<IActionResult> CreateExercise([FromBody] CreateExerciseCommandDTO createExerciseDTO)
        {
            return Ok(await Mediator.Send(new CreateExerciseCommand(createExerciseDTO, _userHttpContextAccessor.GetUserId())));
        }

        [HttpPut("api/[controller]/UpdateExercise")]
        public async Task<IActionResult> UpdateExercise([FromBody] UpdateExerciseCommandDTO updateExerciseDTO)
        {
            return Ok(await Mediator.Send(new UpdateExerciseCommand(updateExerciseDTO, _userHttpContextAccessor.GetUserId())));
        }

        [HttpDelete("api/[controller]/DeleteExercise/{exerciseGuid}")]
        public async Task<ActionResult> DeleteExercise(Guid exerciseGuid)
        {
            return Ok(await Mediator.Send(new DeleteExerciseCommand(exerciseGuid, _userHttpContextAccessor.GetUserId())));
        }

        #endregion

        #region Project Exercise

        [HttpGet("api/[controller]/ProjectExercises/{themeId}")]
        public async Task<ActionResult<List<GetProjectExerciseQueryDTO>>> GetProjectExercises(Guid themeId)
        {
            return Ok(await Mediator.Send(new GetProjectExerciseQuery(_userHttpContextAccessor.GetUserId(), themeId)));
        }

        [HttpPost("api/[controller]/CreateProjectExercise")]
        public async Task<IActionResult> CreateProjectExercise([FromBody] CreateProjectExerciseCommandDTO createExerciseDTO)
        {
            return Ok(await Mediator.Send(new CreateProjectExerciseCommand(_userHttpContextAccessor.GetUserId(), createExerciseDTO)));
        }

        [HttpPut("api/[controller]/UpdateProjectExercise")]
        public async Task<IActionResult> ProjectExercise([FromBody] UpdateProjectExerciseCommand updateProjectExerciseDTO)
        {
            return Ok(await Mediator.Send(updateProjectExerciseDTO));
        }

        [HttpDelete("api/[controller]/DeleteProjectExercise/{projectExerciseGuid}")]
        public async Task<ActionResult> DeleteProjectExercise(Guid projectExerciseGuid)
        {
            return Ok(await Mediator.Send(new DeleteProjectExerciseCommand(projectExerciseGuid)));
        }

        [HttpPut("api/[controller]/CheckProjectExercise")]
        public async Task<IActionResult> CheckProjectExercise([FromBody] CheckProjectExerciseCommandDTO checkExerciseDTO)
        {
            return Ok(await Mediator.Send(new CheckProjectExerciseCommand(checkExerciseDTO, _userHttpContextAccessor.GetUserId())));
        }

        #endregion

        #region Test
        [HttpGet("api/[controller]/Tests/{themeId}")]
        public async Task<ActionResult<List<TeacherTestDTO>>> GetTests(Guid themeId)
        {
            return Ok(await Mediator.Send(new GetTestsTeacherQuery(themeId)));
        }

        [HttpPost("api/[controller]/CreateTest")]
        public async Task<IActionResult> CreateTest([FromBody] CreateTestCommand createTestDTO)
        {
            return Ok(await Mediator.Send(createTestDTO));
        }

        [HttpPut("api/[controller]/UpdateTest")]
        public async Task<IActionResult> UpdateTest([FromBody] UpdateTestCommand updateTestDTO)
        {
            return Ok(await Mediator.Send(updateTestDTO));
        }

        [HttpPut("api/[controller]/UpdateVariants")]
        public async Task<IActionResult> UpdateVariants([FromBody] VariantTestDTO[] updateVariantDTO)
        {
            return Ok(await Mediator.Send(new UpdateVariantCommand(updateVariantDTO)));
        }

        [HttpDelete("api/[controller]/DeleteTest/{testGuid}")]
        public async Task<ActionResult> DeleteTest(Guid testGuid)
        {
            return Ok(await Mediator.Send(new DeleteTestCommand(testGuid)));
        }
        #endregion

    }
}
