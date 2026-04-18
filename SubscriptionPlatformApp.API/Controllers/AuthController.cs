using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SubscriptionPlatformApp.Application.Abstractions.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.ResendVerificationEmailUseCase;
using SubscriptionPlatformApp.Application.DTOs.UseCases.TenantRegistrationUseCase;
using SubscriptionPlatformApp.Application.DTOs.UseCases.UserVerificationUseCase;
using SubscriptionPlatformApp.Application.Utils.Response;

namespace SubscriptionPlatformApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITenantRegistrationUseCase _tenantRegistrationUseCase;
        private readonly IEmailVerificationUseCase _emailVerificationUseCase;
        private readonly IResendVerificationEmailUseCase _resendVerificationEmailUseCase;

        public AuthController(ITenantRegistrationUseCase tenantRegistrationUseCase, 
            IEmailVerificationUseCase emailVerificationUseCase, 
            IResendVerificationEmailUseCase resendVerificationEmailUseCase)
        {
            _tenantRegistrationUseCase = tenantRegistrationUseCase;
            _emailVerificationUseCase = emailVerificationUseCase;
            _resendVerificationEmailUseCase = resendVerificationEmailUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> TenantRegistration([FromBody] TenantRegistrationRequest request, CancellationToken ct)
        {
            if (request == null)
            {
                return BadRequest(ApiResponse.Fail<TenantRegistrationResponse>(ResponseCodes.InvalidRequest));
            }

            var response = await _tenantRegistrationUseCase.ExecuteAsync(request, ct);

            return Ok(response);
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> EmailVerification([FromBody] Guid tokenId, CancellationToken ct)
        {
            if (tokenId == Guid.Empty)
            {
                return BadRequest(ApiResponse.Fail<EmailVerificationResponse>(ResponseCodes.InvalidRequest));
            }

            var response = await _emailVerificationUseCase.ExecuteAsync(tokenId, ct);

            return Ok(response);
        }

        [HttpPost("resend-verification-email")]
        public async Task<IActionResult> ResendVerificationEmail([FromBody] ResendVerificationEmailRequest request, CancellationToken ct)
        {
            if (request == null)
            {
                return BadRequest(ApiResponse.Fail<ResendVerificationEmailResponse>(ResponseCodes.InvalidRequest));
            }

            var response = await _resendVerificationEmailUseCase.ExecuteAsync(request, ct);

            return Ok(response);
        }
    }
}
