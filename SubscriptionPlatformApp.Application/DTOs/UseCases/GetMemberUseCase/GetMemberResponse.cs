using System;
using System.Collections.Generic;
using System.Text;

namespace SubscriptionPlatformApp.Application.DTOs.UseCases.GetMemberUseCase
{
    public class GetMemberResponse
    {
        public List<MemberInfo>? MemberInfos {  get; set; }
    }
}
