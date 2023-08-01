using Application.Account.DTO;
using Application.Account.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Application.Students.Queries;
using Application.Courses.DTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ApiControllerBase
    {
        [HttpPost("SignIn")]
        public async Task<ActionResult<TokenDTO>> Login([FromBody] LoginUserCommand query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("Refresh")]
        public async Task<ActionResult<TokenDTO>> Refresh([FromBody] RefreshTokenCommand query)
        {
            return await Mediator.Send(query);
        }

        [Authorize]
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand changePassword)
        {
            return Ok(await Mediator.Send(changePassword));
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand forgotPassword)
        {
            return Ok(await Mediator.Send(forgotPassword));
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand resetPassword)
        {
            return Ok(await Mediator.Send(resetPassword));
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand signUpCommand)
        {
            return Ok(await Mediator.Send(signUpCommand));
        }

        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] SignUpConfirmEmailCommand confirmEmailCommand)
        {
            return Ok(await Mediator.Send(confirmEmailCommand));
        }

        [HttpGet("Courses")]
        public async Task<ActionResult<List<ShortCourseDTO>>> GetCourses()
        {
            return await Mediator.Send(new GetShortCourseCommand());
        }
    }
}
