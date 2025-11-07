using Gym.Application.Commands.Attendance;
using Gym.Application.DTOs;
using Gym.Application.Queries.Attendance;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AttendanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("scan")]
        public async Task<IActionResult> Scan([FromBody] ScanMembershipCommand request)
        {
            if (request == null || request.MemberID <= 0)
                return BadRequest("Invalid request.");


            var result = await _mediator.Send(new ScanMembershipCommand
            {
                MemberID = request.MemberID
            });

            if (result == "Membership not found" || result == "Membership Expired")
                return BadRequest(result);

            return Ok(result);
        }
        [HttpGet("get-all-attendance")]
        public async Task<ActionResult<List<AttendanceLogDTO>>> GetAllAttendance()
        {
            var result = await _mediator.Send(new GetAllAttendanceQuery());
            return Ok(result);
        }
        [HttpGet("today")]
        public async Task<IActionResult> GetTodayAttendance()
        {
            var result = await _mediator.Send(new GetCurrentAttendanceQuery());
            return Ok(result);
        }
        [Authorize(Roles = "User")]
        [HttpGet("my-attendance")]
        public async Task<IActionResult> GetMyAttendance()
        {
            var attendances = await _mediator.Send(new GetAttendanceByIDQuery());
            return Ok(attendances);
        }
    }
}
