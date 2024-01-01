using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CompanyName.ProjectName.Identity.ExternalAuth.Dto;
using CompanyName.ProjectName.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.OpenIddict;
using Volo.Abp.Uow;

namespace CompanyName.ProjectName.Controllers
{
    public class ExternalAuthController : AbpControllerBase
    {
        protected readonly SignInManager<IdentityUser> SignInManager;
        protected readonly UserManager<IdentityUser> UserManager;

        public ExternalAuthController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager
        )
        {
            LocalizationResource = typeof(ProjectNameResource);
            SignInManager = signInManager;
            UserManager = userManager;
        }

        [HttpPost]
        [UnitOfWork]
        public virtual async Task<IActionResult> ByCodeAsync([FromBody] ExternalAuthByCodeInput input)
        {
            if (input.ProviderName == "WechatMini")
            {

                // var user = await UserManager.FindByLoginAsync(input.ProviderName, openid);

                // if (user == null)
                // {
                //     // TODO: register user
                //     user = new Volo.Abp.Identity.IdentityUser(
                //        GuidGenerator.Create(),
                //        userName,
                //        email,
                //        tenantId)
                //     {
                //         Name = adminUserName
                //     };

                //     (await UserManager.CreateAsync(user, password, validatePassword: false)).CheckErrors();

                //     await UserManager.AddLoginAsync(user, new UserLoginInfo(input.ProviderName, openid, null));
                // }

                var props = new AuthenticationProperties();
                var tokens = new List<AuthenticationToken>();
                // TODO: Abp Claim Name? 和 swagger 登录获取的 token 中的对照下
                var tok1 = new AuthenticationToken { Name = "One", Value = "1" };
                var tok2 = new AuthenticationToken { Name = "Two", Value = "2" };
                var tok3 = new AuthenticationToken { Name = "Three", Value = "3" };
                tokens.Add(tok1);
                tokens.Add(tok2);
                tokens.Add(tok3);
                props.StoreTokens(tokens);

                var authResult = AuthenticateResult.Success(
                    new AuthenticationTicket(new ClaimsPrincipal(), props, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme));

                if (authResult.Principal == null)
                {
                    return Forbid(
                            authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                            properties: new AuthenticationProperties(new Dictionary<string, string?>
                            {
                                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = $"{nameof(authResult.Principal)} Should Not Null!"
                            }));
                }

                return SignIn(authResult.Principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = $"{nameof(input.ProviderName)}:{input.ProviderName} Not Implemented"
                    }));
        }

        // TODO: ByCode
        /*
- 拿 code 换 openid + sessionkey
- 调微信接口 获取用户信息 （先基于 openId 本地注册？或者获取用户信息后再本地注册）
- 验证通过则基于本地 userId 
- //跳过，不必要 AbpSignInManager.ExternalLoginSignInAsync(providerName,openid) // signIn (Context.SignInAsync(AuthenticationScheme,...)且HttpContext.User 赋值) 之后，
- 可以基于 HttpContext.User 直接颁发 token  

abp/modules/openiddict
``` Volo.Abp.OpenIddict.Controllers.TokenController # HandleAuthorizationCodeAsync

            await OpenIddictClaimsPrincipalManager.HandleAsync(request, principal);
            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
```    
        */
    }
}

// abp 的第三方登录机制：
// - 标准重定向流程走 RazorPage LoginModel（OpenIddictSupportedLoginModel）那套，认证成功写入cookie，之后再基于本地的 oauth2 请求token （双重OAuth2.0？）
// - 微信小程序用户认证适配必须通过 api 换取 token（不涉及cookie，调微信 api 认证通过后直接返回本地 AuthServer 的 token），https://juejin.cn/post/7212074532340908091

// - abp openiddict 支持 password flow 使用第三方的 账号密码验证

// Wechat H5 标准流程对接可以参考 Microsoft Identity OAuthHandler<> （小程序基于code，对于 IAuthenticationHandler，没有传递code的调用方法，不适用，仅能基于固定配置发起完整的oauth流程）