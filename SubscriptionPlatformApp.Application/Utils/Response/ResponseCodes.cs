using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Utils.Response
{
    public static class ResponseCodes
    {
        public static readonly ResponseCode Success =
            new("SUCCESS", "Request completed successfully");

        public static readonly ResponseCode InvalidRequest =
            new("INVALID_REQUEST", "The request is invalid");

        public static readonly ResponseCode UserNotFound =
            new("USER_NOT_FOUND", "User not found");

        public static readonly ResponseCode MembershipNotFound =
            new("MEMBERSHIP_NOT_FOUND", "Membership not found");

        public static readonly ResponseCode VerificationTokenNotFound =
            new("VERIFICATION_TOKEN_NOT_FOUND", "Verification token not found");

        public static readonly ResponseCode VerificationTokenExpired =
            new("VERIFICATION_TOKEN_EXPIRED", "Verification token has expired");

        public static readonly ResponseCode UserRejected =
            new("USER_REJECTED", "User has been rejected");

        public static readonly ResponseCode EmailAlreadyVerified =
            new("EMAIL_ALREADY_VERIFIED", "Email has been verified");

        public static readonly ResponseCode SystemError =
            new("SYSTEM_ERROR", "Unexpected error occurred");
    }
}
