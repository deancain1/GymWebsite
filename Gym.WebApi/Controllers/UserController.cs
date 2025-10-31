using Gym.Application.Queries.Users;
using MediatR;
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

        [HttpGet("total-user")]
        public async Task<IActionResult> GetTotalUser()
        {
            var totalUser = await _mediator.Send(new GetTotalUserQuery());
            return Ok(totalUser);
        }
        [HttpGet("total-admins")]
        public async Task<IActionResult> GetTotalAdmins()
        {
            var totalAdmins = await _mediator.Send(new GetTotalAdminQuery());
            return Ok(totalAdmins);
        }
    }
}
