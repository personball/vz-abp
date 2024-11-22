namespace CompanyName.ProjectName.Identity.Wechat.Dto
{
    public class SnsJsCode2SesionAuthResult
    {
        public string session_key { get; set; } = string.Empty;

        public string openid { get; set; } = string.Empty;

        public string unionid { get; set; } = string.Empty;

        public int errcode { get; set; }

        public string errmsg { get; set; } = string.Empty;
    }
}