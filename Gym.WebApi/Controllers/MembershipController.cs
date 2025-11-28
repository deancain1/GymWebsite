using Gym.Application.Commands.Membership;
using Gym.Application.Commands.Users;
using Gym.Application.DTOs;
using Gym.Application.Queries.Memberships;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MembershipController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "User")]
        [HttpPost("create-membership")]

        public async Task<IActionResult> CreateMembership([FromBody] CreateMembershipCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            return Ok(result);
        }
        [Authorize(Roles = "Admin, Staff")]
        [HttpGet("get-all-members")]
        public async Task<ActionResult<List<MembershipDTO>>> GetAllMemberships()
        {
            var result = await _mediator.Send(new GetAllMembershipsQuery());
            return Ok(result);
        }
        [Authorize(Roles = "Admin, Staff")]
        [HttpPut("update-status/{memberId}")]
        public async Task<IActionResult> UpdateStatus(int memberId, [FromBody] string status)
        {
            var command = new UpdateMembershipStatusCommand
            {
                MemberID = memberId,
                Status = status
            };

            await _mediator.Send(command);
            return Ok(new { Message = "Membership status updated successfully" });

        }

        [Authorize(Roles = "Admin, Staff")]
        [HttpGet("get-total-memberships")]
        public async Task<ActionResult<int>> GetTotalMemberships()
        {
            return Ok(await _mediator.Send(new GetTotalMembershipsQuery()));
        }

        [Authorize(Roles = "Admin, Staff")]
        [HttpGet("memberships-per-month")]
        public async Task<IActionResult> GetMembershipsPerMonth()
        {
            var result = await _mediator.Send(new GetMembershipsPerMonthQuery());
            return Ok(result);
        }

        [Authorize(Roles = "Admin, Staff")]
        [HttpGet("expired-memberships")]
        public async Task<IActionResult> GetExpiredMemberships()
        {
            var result = await _mediator.Send(new GetExpiredMembershipsQuery());
            return Ok(result);
        }

        [Authorize(Roles = "Admin, Staff")]
        [HttpGet("plan-counts")]
        public async Task<IActionResult> GetPlanCounts()
        {
            var result = await _mediator.Send(new GetCountMembersipTypeQuery());
            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet("user-qrcode")]
        public async Task<ActionResult<MembershipDTO>> GetCurrentUserQrCode()
        {
            var result = await _mediator.Send(new GetCurrentUserQrCodeQuery());
            if (result == null)
                return NotFound("Membership or QR code not found.");

            return Ok(result);
        }

        [Authorize(Roles = "Admin, Staff")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMemberships(int id)
        {
            await _mediator.Send(new DeleteMembershipsCommand { MemberID = id });
            return NoContent();

        }
    }
}

