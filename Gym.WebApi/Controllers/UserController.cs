using Gym.Application.Commands.Users;
using Gym.Application.Queries.Attendance;
using Gym.Application.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var result = await _mediator.Send(new GetUserByIDQuery { UserId = id });
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserCommand command)
        {
            if (id != command.UserId)
                return BadRequest("User ID in URL does not match request body.");

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("by-role/{roleName}")]
        public async Task<IActionResult> GetUserByRole(string roleName)
        {
            var query = new GetUserByRoleQuery(roleName);
            var user = await _mediator.Send(query);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                await _mediator.Send(new DeleteUserCommand { UserId = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpGet("total-user")]
        public async Task<IActionResult> GetTotalUser()
        {
            var result = await _mediator.Send(new GetTotalUserQuery());
            return Ok(result);
        }
        [HttpGet("total-admins")]
        public async Task<IActionResult> GetTotalAdmins()
        {
            var result = await _mediator.Send(new GetTotalAdminQuery());
            return Ok(result);
        }
        [Authorize(Roles = "User")]
        [HttpGet("user-info")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var result = await _mediator.Send(new GetCurrentUserQuery());
            return Ok(result);
        }
    }
}
