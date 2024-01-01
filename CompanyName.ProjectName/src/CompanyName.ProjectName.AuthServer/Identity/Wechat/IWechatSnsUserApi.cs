using CompanyName.ProjectName.Identity.Wechat.Dto;
using WebApiClientCore;
using WebApiClientCore.Attributes;

#pragma warning disable CS1570
namespace CompanyName.ProjectName.Identity.Wechat
{
    /// <summary>
    /// 基于微信用户授权的 authCode 的微信社交用户相关api
    /// </summary>
    //[LogFilter]
    [HttpHost("https://api.weixin.qq.com")]
    public interface IWechatSnsUserApi : IHttpApi
    {
        /// <summary>
        /// 标准 oauth2 流程 authCode 换token 协议 api
        /// https://api.weixin.qq.com/sns/oauth2/access_token?appid=APPID&secret=SECRET&code=CODE&grant_type=authorization_code
        /// </summary>
        /// <param name="appid">公众号对应的 appid</param>
        /// <param name="secret"></param>
        /// <param name="code">微信 redirect 回来的 code</param>
        /// <param name="grant_type">默认即可</param>
        /// <returns></returns>
        [HttpGet("/sns/oauth2/access_token")]
        [JsonReturn]
        ITask<SnsAuthApiResult> AuthCodeAsync(string appid, string secret, string code, string grant_type = "authorization_code");

        /// <summary>
        /// 刷新token
        /// https://api.weixin.qq.com/sns/oauth2/refresh_token?appid=APPID&grant_type=refresh_token&refresh_token=REFRESH_TOKEN
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="refresh_token"></param>
        /// <param name="grant_type"></param>
        /// <returns></returns>
        [HttpGet("/sns/oauth2/refresh_token")]
        [JsonReturn]
        ITask<SnsRefreshTokenResult> RefreshTokenAsync(string appid, string refresh_token, string grant_type = "refresh_token");

        /// <summary>
        /// 拉取用户信息
        /// https://api.weixin.qq.com/sns/userinfo?access_token=ACCESS_TOKEN&openid=OPENID&lang=zh_CN
        /// </summary>
        /// <param name="access_token">用户授权的 access token</param>
        /// <param name="openid">用户 openid</param>
        /// <param name="lang">语言，默认中文</param>
        /// <returns></returns>
        [HttpGet("/sns/userinfo")]
        [JsonReturn]
        ITask<SnsUserInfoResult> GetUserInfoAsync(string access_token, string openid, string lang = "zh_CN");

        /// <summary>
        /// 验证token有效性
        /// https://api.weixin.qq.com/sns/auth?access_token=ACCESS_TOKEN&openid=OPENID
        /// </summary>
        /// <param name="access_token">用户授权的 access token </param>
        /// <param name="openid">用户 openid</param>
        /// <returns></returns>
        [HttpGet("/sns/auth")]
        [JsonReturn]
        ITask<SnsAuthTokenValidResult> ValidateToken(string access_token, string openid);

        /// <summary>
        /// 小程序登陆 code 换 openid 和 sessionKey 
        /// </summary>
        /// <param name="appid">小程序 appId </param>
        /// <param name="secret">小程序 secret </param>
        /// <param name="js_code">小程序端 js 代码获取的 code </param>
        /// <param name="grant_type">默认值即可</param>
        /// <returns></returns>
        [HttpGet("/sns/jscode2session")]
        [JsonReturn(EnsureMatchAcceptContentType = false)]
        ITask<SnsJsCode2SesionAuthResult> JsCode2SessionAuthAsync(string appid, string secret, string js_code, string grant_type = "authorization_code");

    }
}
#pragma warning restore CS1570
