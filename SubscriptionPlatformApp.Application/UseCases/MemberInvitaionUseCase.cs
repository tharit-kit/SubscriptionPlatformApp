using Microsoft.Extensions.Logging;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.Abstractions.Providers;
using SubscriptionPlatformApp.Application.Abstractions.Services;
using SubscriptionPlatformApp.Application.Abstractions.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.MemberInvitationUseCase;
using SubscriptionPlatformApp.Application.Utils.Response;
using SubscriptionPlatformApp.Domain.Entities;
using SubscriptionPlatformApp.Domain.Enums;

namespace SubscriptionPlatformApp.Application.UseCases
{
    public class MemberInvitaionUseCase : IMemberInvitaionUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISecureTokenGenerator _secureTokenGenerator;
        private readonly IEmailService _emailService;
        private readonly ILogger<MemberInvitaionUseCase> _logger;
        private readonly ITenantContextAccessor _tenantContextAccessor;

        public MemberInvitaionUseCase(IUnitOfWork unitOfWork,
                                      ISecureTokenGenerator secureTokenGenerator,
                                      ICurrentUserService currentUserService,
                                      IEmailService emailService,
                                      ILogger<MemberInvitaionUseCase> logger,
                                      ITenantContextAccessor tenantContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _currentUserService = currentUserService;
            _secureTokenGenerator = secureTokenGenerator;
            _emailService = emailService;
            _tenantContextAccessor = tenantContextAccessor;
        }

        public async Task<ApiResponse<MemberInvitationResponse>> ExecuteAsync(MemberInvitationRequest request, CancellationToken ct)
        {
            try
            {
                if (!_currentUserService.IsAuthenticated)
                {
                    return ApiResponse.Fail<MemberInvitationResponse>(ResponseCodes.Unauthorized);
                }

                var tenantContext = _tenantContextAccessor.Current;
                if (tenantContext == null)
                {
                    return ApiResponse.Fail<MemberInvitationResponse>(ResponseCodes.TenantNotFound);
                }

                if (tenantContext.Role != MembershipRole.Admin.ToString())
                {
                    return ApiResponse.Fail<MemberInvitationResponse>(ResponseCodes.InsufficientPrivilege);
                }

                var adminUserId = _currentUserService.UserId!.Value;
                var tenantId = tenantContext.TenantId;
                var invitedEmail = request.Email.Trim();

                var inviterMembership = await _unitOfWork.Membership.FindByTenantIdAndUserIdAsync(tenantId, adminUserId, ct);
                if (inviterMembership == null)
                {
                    return ApiResponse.Fail<MemberInvitationResponse>(ResponseCodes.MembershipNotFound);
                }

                var inviter = await _unitOfWork.User.FindById(adminUserId, ct);
                if (inviter == null)
                {
                    return ApiResponse.Fail<MemberInvitationResponse>(ResponseCodes.UserNotFound);
                }

                if (string.Equals(inviter.Email, invitedEmail, StringComparison.OrdinalIgnoreCase))
                {
                    return ApiResponse.Fail<MemberInvitationResponse>(ResponseCodes.CannotInviteSelf);
                }

                var invitee = await _unitOfWork.User.FindByEmail(invitedEmail, ct);
                if (invitee != null)
                {
                    var inviteeMembership = await _unitOfWork.Membership.FindByTenantIdAndUserIdAsync(tenantId, invitee.UserId, ct);
                    if (inviteeMembership != null)
                    {
                        return ApiResponse.Fail<MemberInvitationResponse>(ResponseCodes.UserAlreadyInTenant);
                    }
                }

                var now = DateTime.UtcNow;
                var wasAlreadyInvited = await _unitOfWork.MemberInvitation.HasActiveInvitationAsync(
                    tenantId,
                    invitedEmail,
                    now,
                    ct);
                if (wasAlreadyInvited)
                {
                    return ApiResponse.Fail<MemberInvitationResponse>(ResponseCodes.UserAlreadyInvited);
                }

                var tenant = await _unitOfWork.Tenant.FindByIdAsync(tenantId, ct);
                if (tenant == null)
                {
                    return ApiResponse.Fail<MemberInvitationResponse>(ResponseCodes.TenantNotFound);
                }

                var invitation = new MemberInvitations
                {
                    MemberInvitationId = Guid.NewGuid(),
                    TenantId = tenantId,
                    InvitedEmail = invitedEmail,
                    Role = request.Role,
                    HashedToken = _secureTokenGenerator.Generate().Hash,
                    CreatedBy = adminUserId,
                    CreatedAt = now,
                    InvitationStatus = InvitationStatus.Invited,
                    ExpiresAt = now.AddMinutes(15),
                };

                await _unitOfWork.MemberInvitation.AddAsync(invitation, ct);
                await _unitOfWork.SaveChangesAsync(ct);

                var isSendEmail = await _emailService.SendMemberInvitationEmailAsync(invitedEmail,
                                                                                     "New Member",
                                                                                     tenant.TenantName,
                                                                                     inviter.FullName,
                                                                                     request.Role,
                                                                                     invitation.HashedToken,
                                                                                     invitation.ExpiresAt,
                                                                                     ct);
                if (isSendEmail)
                {
                    _logger.LogInformation("Member invitation email sent successfully");
                }

                var res = new MemberInvitationResponse
                {
                    InvitationId = invitation.MemberInvitationId
                };

                return ApiResponse.Success<MemberInvitationResponse>(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return ApiResponse.Fail<MemberInvitationResponse>(ResponseCodes.SystemError);
            }
        }
    }
}
