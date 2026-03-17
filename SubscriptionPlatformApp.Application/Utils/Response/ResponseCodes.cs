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

        public static readonly ResponseCode NotFound =
            new("NOT_FOUND", "Resource not found");

        public static readonly ResponseCode SystemError =
            new("SYSTEM_ERROR", "Unexpected error occurred");
    }
}
