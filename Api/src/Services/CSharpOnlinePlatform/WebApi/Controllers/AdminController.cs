using Application.Account.Services;
using Application.Admin.Commands.Documentations;
using Application.Admin.Commands.Documentations.Command;
using Application.Admin.Commands.TeacherCommand;
using Application.Admin.Commands.TeacherCommand.TeacherCRUD;
using Application.Admin.Queries;
using Application.Documentations.DTO;
using Application.Documentations;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class AdminController : ApiControllerBase
    {
        private readonly IUserHttpContextAccessor _userHttpContextAccessor;

        public AdminController(IUserHttpContextAccessor httpContextAccessor)
        {
            _userHttpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 0, MaxNodeCount = 10)]
        public async Task<Admin> Get()
        {
            return await Mediator.Send(new GetAdminQuery(_userHttpContextAccessor.GetUserId()));
        }

        [HttpDelete("DeleteTeacher/{teacherGuid}")]
        public async Task<ActionResult<Guid>> DeleteTeacher(Guid teacherGuid)
        {
            return Ok(await Mediator.Send(new DeleteTeacherCommand(teacherGuid)));
        }

        [HttpPut("UpdateTeacher")]
        public async Task<ActionResult<Guid>> UpdateTeacher([FromBody] UpdateTeacherCommand updateTeacher)
        {
            return await Mediator.Send(updateTeacher);
        }

        [HttpGet("Teachers")]
        public async Task<ActionResult<List<GetTeacherCommandDTO>>> GetTeachers()
        {
            return await Mediator.Send(new GetTeachersCommand());
        }

        [HttpGet("api/[controller]/help")]
        public async Task<ActionResult<DocumentationDTO>> GetHelp()
        {
            return await Mediator.Send(new GetDocumentationQuery(DocumentArea.Admin));
        }

        [HttpGet("Docs")]
        public async Task<ActionResult<List<Documentation>>> GetDocumentation()
        {
            return await Mediator.Send(new GetAllDocumentationQuery());
        }

        [HttpPost("CreateDoc")]
        public async Task<ActionResult<IdentityResult>> CreateDocumentation([FromBody] CreateDocumentationCommand docCommand)
        {
            return Ok(await Mediator.Send(docCommand));
        }

        [HttpDelete("DeleteDoc/{docGuid}")]
        public async Task<ActionResult<IdentityResult>> DeleteDocumentation(Guid docGuid)
        {
            return Ok(await Mediator.Send(new DeleteDocumentationCommand(docGuid)));
        }

        [HttpPut("UpdateDoc")]
        public async Task<ActionResult<IdentityResult>> UpdateDocumentation([FromBody] UpdateDocumentationCommand docCommand)
        {
            return Ok(await Mediator.Send(docCommand));
        }
    }
}
