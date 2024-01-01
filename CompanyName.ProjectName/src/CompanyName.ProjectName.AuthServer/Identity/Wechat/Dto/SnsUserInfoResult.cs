using System.Collections.Generic;

namespace CompanyName.ProjectName.Identity.Wechat.Dto
{
    public class SnsUserInfoResult
    {
        public string? openid { get; set; }
        public string? nickname { get; set; }
        public int sex { get; set; }
        public string? province { get; set; }
        public string? city { get; set; }
        public string? country { get; set; }
        public string? headimgurl { get; set; }
        public List<string>? privilege { get; set; }
        public string? unionid { get; set; }
    }
}