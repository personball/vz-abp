namespace CompanyName.ProjectName.Identity.Wechat.Dto
{
    public class SnsJsCode2SesionAuthResult
    {
        public string session_key { get; set; }

        public string openid { get; set; }

        public string unionid { get; set; }

        public int errcode { get; set; }

        public string errmsg { get; set; }
    }
}