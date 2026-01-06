using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using CompanyName.ProjectName.Identity.Wechat;
using IdentityUser = Volo.Abp.Identity.IdentityUser;
using SignInResult = Microsoft.AspNetCore.Mvc.SignInResult;

namespace OpenIddict.Server.ExtensionGrants;

// https://abp.io/community/articles/6v0df94z#gsc.tab=0
// usage as curl
// POST /connect/token
// -H 'content-type: application/x-www-form-urlencoded'
// -H 'accept: application/json, text/plain, */*'
// --data-raw 'grant_type=WechatMiniProgramGrant&code={code}&client_id=ProjectName_WechatMiniApp'

// https://developers.weixin.qq.com/miniprogram/dev/framework/open-ability/login.html
public class WechatMiniProgramGrant : ITokenExtensionGrant
{
    public const string ExtensionGrantName = nameof(WechatMiniProgramGrant);

    public string Name => ExtensionGrantName;

    public async Task<IActionResult> HandleAsync(ExtensionGrantContext context)
    {
        return await HandleUserWechatMiniJsCode2SessionAsync(context);
    }

    private async Task<IActionResult> HandleUserWechatMiniJsCode2SessionAsync(ExtensionGrantContext context)
    {
        var code = context.Request.GetParameter("code").ToString();
        if (string.IsNullOrEmpty(code))
        {
            return new ForbidResult(
                new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidRequest
                }!));
        }
        // Call Wechat API to get the user info by code
        var api_client = context.HttpContext.RequestServices.GetRequiredService<IWechatSnsUserApi>();
        //TODO: 区分哪个小程序
        var wechat_options = context.HttpContext.RequestServices.GetRequiredService<IOptions<WechatOptions>>();
        var wechat_cfg = wechat_options.Value;
        var wechat_user = await api_client.JsCode2SessionAuthAsync(wechat_cfg.AppId, wechat_cfg.AppSecret, code);
        if (wechat_user.errcode != 0)
        {
            return new ForbidResult(
                new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidRequest
                }!));
        }

        var userManager = context.HttpContext.RequestServices.GetRequiredService<IdentityUserManager>();
        var GuidGenerator = context.HttpContext.RequestServices.GetRequiredService<IGuidGenerator>();
        var user = await userManager.FindByLoginAsync(nameof(WechatMiniProgramGrant), wechat_user.openid);
        if (user == null)
        {
            // create user if not exist
            user = new IdentityUser(
               GuidGenerator.Create(),
               wechat_user.openid,
               $"{wechat_user.openid}@abp.io")
            {
                Name = wechat_user.openid,
            };

            var password = Guid.NewGuid().ToString("N") + "aA1!";

            (await userManager.CreateAsync(user, password, validatePassword: false)).CheckErrors();

            await userManager.AddLoginAsync(user, new UserLoginInfo(nameof(WechatMiniProgramGrant), wechat_user.openid, null));
        }
        // We have validated the user token and got the user id

        var userClaimsPrincipalFactory = context.HttpContext.RequestServices.GetRequiredService<IUserClaimsPrincipalFactory<IdentityUser>>();
        var claimsPrincipal = await userClaimsPrincipalFactory.CreateAsync(user);

        // Prepare the scopes
        var scopes = GetScopes(context);

        claimsPrincipal.SetScopes(scopes);
        claimsPrincipal.SetResources(await GetResourcesAsync(context, scopes));
        await context.HttpContext.RequestServices.GetRequiredService<AbpOpenIddictClaimsPrincipalManager>().HandleAsync(context.Request, claimsPrincipal);
        return new SignInResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, claimsPrincipal);
    }

    private ImmutableArray<string> GetScopes(ExtensionGrantContext context)
    {
        // Prepare the scopes
        // The scopes must be defined in the OpenIddict server

        // If you want to get the scopes from the request, you have to add `scope` parameter in the request
        // scope: AbpAPI profile roles email phone offline_access

        //var scopes = context.Request.GetScopes();

        // If you want to set the scopes here, you can use the following code
        var scopes = new[] {
            OpenIddictConstants.Permissions.Scopes.Address,
            OpenIddictConstants.Permissions.Scopes.Email,
            OpenIddictConstants.Permissions.Scopes.Phone,
            OpenIddictConstants.Permissions.Scopes.Profile,
            OpenIddictConstants.Permissions.Scopes.Roles,
            "ProjectName"
        }.ToImmutableArray();

        return scopes;
    }

    private async Task<IEnumerable<string>> GetResourcesAsync(ExtensionGrantContext context, ImmutableArray<string> scopes)
    {
        var resources = new List<string>();
        if (!scopes.Any())
        {
            return resources;
        }

        await foreach (var resource in context.HttpContext.RequestServices.GetRequiredService<IOpenIddictScopeManager>().ListResourcesAsync(scopes))
        {
            resources.Add(resource);
        }
        return resources;
    }
}