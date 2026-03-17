using SubscriptionPlatformApp.Application.DTOs.UseCases;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Utils.Response
{
    public static class ApiResponse
    {
        public static ApiResponse<T> Success<T>(T data)
        {
            return new ApiResponse<T>
            {
                ResponseCode = ResponseCodes.Success.Code,
                ResponseDescription = ResponseCodes.Success.Description,
                Data = data
            };
        }

        public static ApiResponse<T> Fail<T>(ResponseCode code)
        {
            return new ApiResponse<T>
            {
                ResponseCode = code.Code,
                ResponseDescription = code.Description,
                Data = default
            };
        }

        public static ApiResponse<T> Fail<T>(ResponseCode code, string customMessage)
        {
            return new ApiResponse<T>
            {
                ResponseCode = code.Code,
                ResponseDescription = customMessage,
                Data = default
            };
        }
    }
}
