namespace CompanyName.ProjectName.Identity.Wechat.Dto
{
    public class SnsAuthApiResult
    {
        public string? access_token { get; set; }
        public int expires_in { get; set; }
        public string? refresh_token { get; set; }
        public string? openid { get; set; }
        public string? scope { get; set; }
    }
}