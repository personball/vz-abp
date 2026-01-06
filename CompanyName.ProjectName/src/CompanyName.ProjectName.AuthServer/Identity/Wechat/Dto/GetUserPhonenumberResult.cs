namespace CompanyName.ProjectName.Identity.Wechat.Dto
{
    public class GetUserPhonenumberResult
    {
        public int errcode { get; set; }
        public string? errmsg { get; set; }
        public PhoneInfo phone_info { get; set; } = null!;

        public class PhoneInfo
        {
            public string phoneNumber { get; set; } = null!;
            public string purePhoneNumber { get; set; } = null!;
            public string countryCode { get; set; } = null!;
            public Watermark watermark { get; set; } = null!;
        }

        public class Watermark
        {
            public int timestamp { get; set; }
            public string appid { get; set; } = null!;
        }
    }
}