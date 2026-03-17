using SubscriptionPlatformApp.Application.Utils.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.DTOs.UseCases
{
    public class ApiResponse<T>
    {
        public string ResponseCode { get; set; } = default!;
        public string ResponseDescription { get; set; } = default!;
        public T? Data { get; set; }

        public bool IsSuccess => ResponseCode == ResponseCodes.Success.Code;
    }
}
