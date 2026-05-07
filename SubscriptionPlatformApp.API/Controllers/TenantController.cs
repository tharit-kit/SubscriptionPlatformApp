using Microsoft.AspNetCore.Mvc;
using SubscriptionPlatformApp.Application.Abstractions.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.TenantRegistrationUseCase;
using SubscriptionPlatformApp.Application.Utils.Response;

namespace SubscriptionPlatformApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly IGetMemberUseCase _getMemberUseCase;

        public TenantController(IGetMemberUseCase getMemberUseCase)
        {
            _getMemberUseCase = getMemberUseCase;
        }

        [HttpGet("membership")]
        public async Task<IActionResult> GetMembers(CancellationToken ct)
        {
            var response = await _getMemberUseCase.ExecuteAsync(ct);

            return Ok(response);
        }
    }
}
