namespace CompanyName.ProjectName.Identity.Wechat.Dto
{
    public class AppAccessTokenResult
    {
        //{"access_token":"ACCESS_TOKEN","expires_in":7200}
        public string? access_token { get; set; }
        public int expires_in { get; set; }
    }
}