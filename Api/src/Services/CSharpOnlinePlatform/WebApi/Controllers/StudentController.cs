using Application.Courses.DTO;
using Application.Exercises.DTO;
using Application.Students.Queries;
using Application.Themes.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Account.Services;
using Microsoft.AspNetCore.Authorization;
using System;
using Application.CodeAnalyzer.Commands;
using Application.CodeAnalyzer.DTO;
using Application.Documentations;
using Application.Documentations.DTO;
using Core.Exceptions;
using Domain.Entities;
using Application.Students.Commands;
using Core.DTO;
using Infrastructure.Identity;

namespace WebApi.Controllers;

[Authorize(ApplicationPolicies.Student)]
public class StudentController : ApiControllerBase
{
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;

    public StudentController(IUserHttpContextAccessor httpContextAccessor)
    {
        _userHttpContextAccessor = httpContextAccessor;
    }

    [HttpGet("api/[controller]/Courses")]
    public async Task<ActionResult<List<CourseDTO>>> GetCourses()
    {
        return await Mediator.Send(new GetCoursesQuery(_userHttpContextAccessor.GetUserId()));
    }

    [HttpGet("api/[controller]/Themes/{courseId}")]
    public async Task<ActionResult<List<ShortThemeDTO>>> GetThemes(Guid courseId)
    {
        return await Mediator.Send(new GetThemesQuery(courseId, _userHttpContextAccessor.GetUserId()));
    }

    [HttpGet("api/[controller]/Theme/{themeId}")]
    public async Task<ActionResult<ThemeDTO>> GetTheme(Guid themeId)
    {
        return await Mediator.Send(new GetThemeQuery(themeId, _userHttpContextAccessor.GetUserId()));
    }

    [HttpGet("api/[controller]/Exercises/{themeId}")]
    public async Task<ActionResult<List<StudentExerciseDTO>>> GetExercises(Guid themeId)
    {
        return await Mediator.Send(new GetExercisesQuery(themeId, _userHttpContextAccessor.GetUserId()));
    }

    [HttpPost("api/[controller]/CheckExercise")]
    public async Task<ActionResult<CodeAnalyzeResultDTO>> CheckExercise([FromBody] AnalyzeCodeCommandParameter analyzeCodeCommandParameter)
    {
        return await Mediator.Send(new AnalyzeCodeCommand(_userHttpContextAccessor.GetUserId(), analyzeCodeCommandParameter, GetVersion()));
    }

    [HttpGet("api/[controller]/ProjectExercises/{themeId}")]
    public async Task<ActionResult<List<StudentProjectExerciseDTO>>> GetProjectExercises(Guid themeId)
    {
        return await Mediator.Send(new GetStudentProjectExerciseQuery(themeId, _userHttpContextAccessor.GetUserId()));
    }

    [HttpPost("api/[controller]/SubmitProjectExercise")]
    public async Task<ActionResult<ServerResponseDTO>> SubmitProjectExercise()
    {
        return await Mediator.Send(new SubmitProjectExerciseCommand(Guid.Parse(Request.Form["projectId"].ToString()), 
            _userHttpContextAccessor.GetUserId(), Request.Form.Files[0]));
    }

    [HttpGet("api/[controller]/Tests/{themeId}")]
    public async Task<ActionResult<List<StudentTestDTO>>> GetTests(Guid themeId)
    {
        return await Mediator.Send(new GetTestsQuery(themeId, _userHttpContextAccessor.GetUserId()));
    }

    [HttpPost("api/[controller]/CheckTest")]
    public async Task<ActionResult<AnalyzeTestResultDTO>> CheckTest([FromBody] AnalyzeTestCommandParameter analyzeTestCommandParameter)
    {
        return await Mediator.Send(new AnalyzeTestCommand(_userHttpContextAccessor.GetUserId(), analyzeTestCommandParameter.TestId, analyzeTestCommandParameter.VariantId));
    }

    [HttpGet("api/[controller]/Rating/{courseId}")]
    public async Task<ActionResult<RatingDTO>> GetRating(Guid courseId)
    {
        return await Mediator.Send(new GetStudentRatingQuery(courseId, _userHttpContextAccessor.GetUserId()));
    }

    [HttpGet("api/[controller]/help")]
    public async Task<ActionResult<DocumentationDTO>> GetDocumentation()
    {
        return await Mediator.Send(new GetDocumentationQuery(DocumentArea.Student));
    }

    VersionDTO GetVersion()
    {
        return new VersionDTO { Language = Constants.CSHARP_LANGUAGE, Version = Constants.DOTNET_SIX_VERSION };
    }
}