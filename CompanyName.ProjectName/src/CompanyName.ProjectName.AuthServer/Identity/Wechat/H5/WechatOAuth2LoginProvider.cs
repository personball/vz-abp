using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;

namespace CompanyName.ProjectName.Identity.Wechat.H5
{
    /// <summary>
    /// h5 标准 OAuth2.0 认证流程
    /// </summary>
    public class WechatOAuth2LoginProvider : ExternalLoginProviderBase, ITransientDependency
    {
        public const string Name = "WechatOAuth2";
        
        public WechatOAuth2LoginProvider(
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant,
            IdentityUserManager userManager,
            IIdentityUserRepository identityUserRepository,
            IOptions<IdentityOptions> identityOptions)
            : base(guidGenerator, currentTenant, userManager, identityUserRepository, identityOptions)
        {
        }

        public override Task<bool> IsEnabledAsync()
        {
            throw new System.NotImplementedException();
        }

        public override Task<bool> TryAuthenticateAsync(string userName, string plainPassword)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<ExternalLoginUserInfo> GetUserInfoAsync(string userName)
        {
            throw new System.NotImplementedException();
        }
    }
}