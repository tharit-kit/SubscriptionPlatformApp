using Microsoft.AspNetCore.Mvc;
using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.TenantRegistrationUseCase;
using SubscriptionPlatformApp.Application.UseCases;
using SubscriptionPlatformApp.Application.Utils.Response;

namespace SubscriptionPlatformApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TenantRegistrationUseCase _tenantRegistrationUseCase;
        public AuthController(TenantRegistrationUseCase tenantRegistrationUseCase) 
        {
            _tenantRegistrationUseCase = tenantRegistrationUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> TenantRegistration([FromBody] TenantRegistrationRequest request, CancellationToken ct)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<TenantRegistrationResponse>()
                {
                    ResponseCode = ResponseCodes.InvalidRequest.Code,
                    ResponseDescription = ResponseCodes.InvalidRequest.Description
                });
            }

            var response = await _tenantRegistrationUseCase.ExecuteAsync(request, ct);

            return Ok(response);
        }
    }
}
