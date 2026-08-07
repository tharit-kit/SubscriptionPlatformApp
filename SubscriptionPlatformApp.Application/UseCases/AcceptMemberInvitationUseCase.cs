using Microsoft.Extensions.Logging;
using SubscriptionPlatformApp.Application.Abstractions.Persistence;
using SubscriptionPlatformApp.Application.Abstractions.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases;
using SubscriptionPlatformApp.Application.DTOs.UseCases.AcceptMemberInvitationUseCase;
using SubscriptionPlatformApp.Application.DTOs.UseCases.TenantRegistrationUseCase;
using SubscriptionPlatformApp.Application.DTOs.UseCases.VerifyMemberInvitation;
using SubscriptionPlatformApp.Application.Helpers;
using SubscriptionPlatformApp.Application.Utils.Response;
using SubscriptionPlatformApp.Domain.Entities;
using SubscriptionPlatformApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.UseCases
{
    public class AcceptMemberInvitationUseCase : IAcceptMemberInvitationUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AcceptMemberInvitationUseCase> _logger;

        public AcceptMemberInvitationUseCase(IUnitOfWork unitOfWork, ILogger<AcceptMemberInvitationUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponse<AcceptMemberInvitationResponse>> ExecuteAsync(AcceptMemberInvitationRequest request, CancellationToken ct)
        {
            try
            {
                var invitation = await _unitOfWork.MemberInvitation.FindByToken(request.Token, ct);
                if (invitation == null)
                {
                    return ApiResponse.Fail<AcceptMemberInvitationResponse>(ResponseCodes.MemberInvitationNotFound);
                }

                var tenant = await _unitOfWork.Tenant.FindByIdAsync(invitation.TenantId, ct);
                if (tenant == null)
                {
                    return ApiResponse.Fail<AcceptMemberInvitationResponse>(ResponseCodes.TenantNotFound);
                }

                var newMemberId = Guid.NewGuid();
                var now = DateTime.UtcNow;
                var memberName = string.Empty;

                if (invitation.ExpiresAt < now)
                {
                    return ApiResponse.Fail<AcceptMemberInvitationResponse>(ResponseCodes.MemberInvitationExpired);
                }

                if (request.IsNewUser)
                {
                    memberName = request.FullName;
                    var generatedSalt = PasswordHasher.GenerateSalt();
                    var hashedPassword = PasswordHasher.GenerateHash(request.ConfirmPassword ?? "", generatedSalt);

                    var newUser = new Users
                    {
                        UserId = newMemberId,
                        Email = invitation.InvitedEmail,
                        FullName = request.FullName ?? string.Empty,
                        HashedPassword = hashedPassword,
                        GeneratedSalt = generatedSalt,
                        UserStatus = UserStatus.Active,
                        CreatedAt = now,
                        CreatedBy = newMemberId
                    };
                    await _unitOfWork.User.AddAsync(newUser, ct);
                }
                else
                {
                    var invitee = await _unitOfWork.User.FindByEmail(invitation.InvitedEmail, ct);
                    if (invitee == null)
                    {
                        return ApiResponse.Fail<AcceptMemberInvitationResponse>(ResponseCodes.UserNotFound);
                    }

                    newMemberId = invitee.UserId;
                    memberName = invitee.FullName;
                }

                invitation.InvitationStatus = InvitationStatus.Accepted;
                invitation.AcceptedAt = now;
                invitation.UpdatedAt = now;
                invitation.UpdatedBy = newMemberId;

                _unitOfWork.MemberInvitation.Update(invitation);

                var newMembership = new Memberships
                {
                    MembershipId = Guid.NewGuid(),
                    UserId = newMemberId,
                    TenantId = invitation.TenantId,
                    Role = invitation.Role,
                    MemberStatus = MemberStatus.Active,
                    JoinedAt = now,
                    CreatedAt = now,
                    CreatedBy = newMemberId
                };

                await _unitOfWork.Membership.AddAsync(newMembership, ct);

                await _unitOfWork.SaveChangesAsync(ct);



                var res = new AcceptMemberInvitationResponse
                {
                    FullName = memberName ?? "",
                    TenantName = tenant.TenantName
                };

                return ApiResponse.Success<AcceptMemberInvitationResponse>(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return ApiResponse.Fail<AcceptMemberInvitationResponse>(ResponseCodes.SystemError);
            }
        }
    }
}
