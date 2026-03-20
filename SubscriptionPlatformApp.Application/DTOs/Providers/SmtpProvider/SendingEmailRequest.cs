using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.DTOs.Providers.SmtpProvider
{
    public class SendingEmailRequest
    {
        public required string Subject { get; set; }
        public required SenderInfo Sender { get; set; }
        public required string HtmlContent { get; set; }
        public required List<RecepientInfo> To { get; set; }
    }

    public class SenderInfo
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
    }


    public class RecepientInfo
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
