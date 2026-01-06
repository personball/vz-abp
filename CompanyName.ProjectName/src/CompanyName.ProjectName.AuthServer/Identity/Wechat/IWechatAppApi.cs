// 使用 client_credential 认证的 access token 调用的应用相关接口
using System.Net.Http;
using CompanyName.ProjectName.Identity.Wechat.Dto;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace CompanyName.ProjectName.Identity.Wechat
{
    // [LogFilter]
    [HttpHost("https://api.weixin.qq.com")]
    public interface IWeChatAppApi : IHttpApi
    {
        /// <summary>
        /// 获取基于 client_credential 的应用级 access token （可定时刷新）
        /// https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=APPID&secret=APPSECRET
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        /// <param name="grant_type"></param>
        /// <returns></returns>
        [HttpGet("/cgi-bin/token")]
        [JsonReturn]
        ITask<AppAccessTokenResult> GetAccessTokenAsync(string appid, string secret, string grant_type = "client_credential");

        /// <summary>
        /// https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=ACCESS_TOKEN
        /// </summary>
        /// <param name="access_token">基于 client_credential 的应用级 access token</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("/wxa/getwxacodeunlimit")]
        ITask<HttpResponseMessage> GetWXCodeUnlimit(string access_token, [JsonContent] QrCodeRequest request);

        /// <summary>
        /// 获取用户手机号
        /// </summary>
        /// <param name="access_token">基于 client_credential 的应用级 access token</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("/wxa/business/getuserphonenumber")]
        [JsonReturn]
        ITask<GetUserPhonenumberResult> GetUserPhonenumber(string access_token, [JsonContent] GetUserPhonenumberRequest request);

        /// <summary>
        /// 发送统一服务消息
        /// </summary>
        /// <param name="access_token">基于 client_credential 的应用级 access token</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/cgi-bin/message/wxopen/template/uniform_send")]
        ITask<HttpResponseMessage> SendUniformMsgAsync(string access_token, [JsonContent] object input);

        /// <summary>
        /// 公众号开发 模板消息接口
        /// https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=ACCESS_TOKEN
        /// </summary>
        /// <param name="access_token">基于 client_credential 的应用级 access token</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("/cgi-bin/message/template/send")]
        [JsonReturn]
        ITask<TplMsgResult> TplMsgNotify(string access_token, [JsonContent] object input);
    }
}




