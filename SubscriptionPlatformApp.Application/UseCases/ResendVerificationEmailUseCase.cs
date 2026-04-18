using Microsoft.Extensions.Logging;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.Abstractions.Providers;
using SubscriptionPlatformApp.Application.Abstractions.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.ResendVerificationEmailUseCase;
using SubscriptionPlatformApp.Application.DTOs.UseCases.TenantRegistrationUseCase;
using SubscriptionPlatformApp.Application.Utils.Response;
using SubscriptionPlatformApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.UseCases
{
    public class ResendVerificationEmailUseCase : IResendVerificationEmailUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly ILogger<ResendVerificationEmailUseCase> _logger;
        public ResendVerificationEmailUseCase(IUnitOfWork unitOfWork, IEmailService emailService, ILogger<ResendVerificationEmailUseCase> logger) 
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<ApiResponse<ResendVerificationEmailResponse>> ExecuteAsync(ResendVerificationEmailRequest request, CancellationToken ct)
        {
            try
            {
                var user = await _unitOfWork.User.FindById(request.UserId, ct);
                if (user == null)
                {
                    return ApiResponse.Fail<ResendVerificationEmailResponse>(ResponseCodes.UserNotFound);
                }

                var now = DateTime.UtcNow;
                var emailVerification = new EmailVerificationTokens
                {
                    EmailVerificationTokenId = Guid.NewGuid(),
                    UserId = request.UserId,
                    TenantId = request.TenantId,
                    ExpireAt = now.AddMinutes(15),
                    CreatedAt = now,
                    CreatedBy = request.UserId,
                };

                await _unitOfWork.EmailVerificationToken.AddAsync(emailVerification, ct);
                await _unitOfWork.SaveChangesAsync(ct);

                var isSendEmail = await _emailService.SendVerificationEmailAsync(user.Email, user.FullName, emailVerification.EmailVerificationTokenId, ct);
                if (isSendEmail)
                {
                    _logger.LogInformation("Verification email has been sent successfully");
                }

                return ApiResponse.Success<ResendVerificationEmailResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Failed to resend verification email");
                return ApiResponse.Fail<ResendVerificationEmailResponse>(ResponseCodes.SystemError);
            }
        }
    }
}
