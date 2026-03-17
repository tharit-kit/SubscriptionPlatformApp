using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.Utils.Response
{
    public class ResponseCode
    {
        public string Code { get; }
        public string Description { get; }

        public ResponseCode(string code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}
