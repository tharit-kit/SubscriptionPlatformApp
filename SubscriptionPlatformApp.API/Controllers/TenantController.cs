using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionPlatformApp.Application.Abstractions.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.AcceptMemberInvitationUseCase;
using SubscriptionPlatformApp.Application.DTOs.UseCases.MemberInvitationUseCase;
using SubscriptionPlatformApp.Application.DTOs.UseCases.VerifyMemberInvitation;
using SubscriptionPlatformApp.Application.Utils.Response;

namespace SubscriptionPlatformApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly IGetMemberUseCase _getMemberUseCase;
        private readonly IMemberInvitaionUseCase _memberInvitaionUseCase;
        private readonly IVerifyMemberInvitationUseCase _verifyMemberInvitationUseCase;
        private readonly IAcceptMemberInvitationUseCase _acceptMemberInvitationUseCase;

        public TenantController(IGetMemberUseCase getMemberUseCase,
                                IMemberInvitaionUseCase memberInvitaionUseCase,
                                IVerifyMemberInvitationUseCase verifyMemberInvitationUseCase,
                                IAcceptMemberInvitationUseCase acceptMemberInvitationUseCase)
        {
            _getMemberUseCase = getMemberUseCase;
            _memberInvitaionUseCase = memberInvitaionUseCase;
            _verifyMemberInvitationUseCase = verifyMemberInvitationUseCase;
            _acceptMemberInvitationUseCase = acceptMemberInvitationUseCase;
        }

        [HttpGet("membership")]
        public async Task<IActionResult> GetMembers(CancellationToken ct)
        {
            var response = await _getMemberUseCase.ExecuteAsync(ct);

            return Ok(response);
        }

        [HttpPost("invite-member")]
        public async Task<IActionResult> InviteMember([FromBody] MemberInvitationRequest request, CancellationToken ct)
        {
            if (request == null)
            {
                return BadRequest(ApiResponse.Fail<MemberInvitationResponse>(ResponseCodes.InvalidRequest));
            }

            var response = await _memberInvitaionUseCase.ExecuteAsync(request, ct);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("verify-member-invitation")]
        public async Task<IActionResult> VerifyMemberInvitation([FromBody] VerifyMemberInvitationRequest request, CancellationToken ct)
        {
            if (request == null)
            {
                return BadRequest(ApiResponse.Fail<VerifyMemberInvitationResponse>(ResponseCodes.InvalidRequest));
            }

            var response = await _verifyMemberInvitationUseCase.ExecuteAsync(request, ct);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("accept-member-invitation")]
        public async Task<IActionResult> AcceptMemberInvitation([FromBody] AcceptMemberInvitationRequest request, CancellationToken ct)
        {
            if (request == null)
            {
                return BadRequest(ApiResponse.Fail<AcceptMemberInvitationResponse>(ResponseCodes.InvalidRequest));
            }

            var response = await _acceptMemberInvitationUseCase.ExecuteAsync(request, ct);

            return Ok(response);
        }
    }
}
