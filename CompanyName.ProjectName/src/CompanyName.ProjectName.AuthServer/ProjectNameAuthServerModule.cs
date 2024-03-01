using System;
using System.IO;
using System.Linq;
using Localization.Resources.AbpUi;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CompanyName.ProjectName.EntityFrameworkCore;
using CompanyName.ProjectName.Localization;
using CompanyName.ProjectName.MultiTenancy;
using StackExchange.Redis;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Auditing;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.UI;
using Volo.Abp.VirtualFileSystem;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using CompanyName.ProjectName.Identity.Wechat;
using Microsoft.Extensions.Configuration;

namespace CompanyName.ProjectName;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpDistributedLockingModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
    typeof(ProjectNameEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreSerilogModule)
    )]
public class ProjectNameAuthServerModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<OpenIddictBuilder>(b =>
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            var issuer = new Uri(configuration["AuthServer:Authority"]!);

            b
            .AddServer(builder =>
            {
                if (hostingEnvironment.IsDevelopment())
                {
                    builder.UseAspNetCore().DisableTransportSecurityRequirement();
                }
                else
                {
                    builder.UseAspNetCore();
                }

                builder.SetAccessTokenLifetime(TimeSpan.FromDays(90));

                // override all endpoint uris
                builder.SetIssuer(issuer);
                builder.SetConfigurationEndpointUris(new Uri(configuration["AuthServer:Authority"] + "/.well-known/openid-configuration"));
                builder.SetCryptographyEndpointUris(new Uri(configuration["AuthServer:Authority"] + "/.well-known/jwks"));
                builder.SetAuthorizationEndpointUris(new Uri(configuration["AuthServer:Authority"] + "/connect/authorize"));
                builder.SetTokenEndpointUris(new Uri(configuration["AuthServer:Authority"] + "/connect/token"));
                builder.SetIntrospectionEndpointUris(new Uri(configuration["AuthServer:Authority"] + "/connect/introspect"));
                builder.SetLogoutEndpointUris(new Uri(configuration["AuthServer:Authority"] + "/connect/logout"));
                builder.SetRevocationEndpointUris(new Uri(configuration["AuthServer:Authority"] + "/connect/revocat"));
                builder.SetUserinfoEndpointUris(new Uri(configuration["AuthServer:Authority"] + "/connect/userinfo"));
                builder.SetDeviceEndpointUris(new Uri(configuration["AuthServer:Authority"] + "/connect/device"));
                builder.SetVerificationEndpointUris(new Uri(configuration["AuthServer:Authority"] + "/connect/verify"));

                // https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html
                // https://learn.microsoft.com/zh-cn/dotnet/core/additional-tools/self-signed-certificates-guide#with-openssl
                var certificate = new X509Certificate2(
                    Path.Combine(AppContext.BaseDirectory, configuration["OpenIddict:CAFilePath"])); // TODO: 需生成证书 pki/ca.pfx
                builder.AddSigningCertificate(certificate);
                builder.AddEncryptionCertificate(certificate);
            })
            .AddValidation(options =>
            {
                options.AddAudiences("ProjectName");
                builder.SetIssuer(issuer);
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        context.Services.AddHealthChecks();
        context.Services.Configure<HealthCheckPublisherOptions>(options =>
        {
            options.Delay = TimeSpan.FromSeconds(2);
            options.Predicate = (check) => check.Tags.Contains("ready");
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ProjectNameResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                BasicThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });

        Configure<AbpAuditingOptions>(options =>
        {
            //options.IsEnabledForGetRequests = true;
            options.ApplicationName = "AuthServer";
        });

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<ProjectNameDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}CompanyName.ProjectName.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<ProjectNameDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}CompanyName.ProjectName.Domain"));
            });
        }

        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());

            options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
            options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
        });

        Configure<AbpBackgroundJobOptions>(options =>
        {
            options.IsJobExecutionEnabled = false;
        });

        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "ProjectName:";
        });

        var dataProtectionBuilder = context.Services.AddDataProtection().SetApplicationName("ProjectName");
        if (!hostingEnvironment.IsDevelopment())
        {
            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]!);
            dataProtectionBuilder.PersistKeysToStackExchangeRedis(redis, "ProjectName-Protection-Keys");
        }

        context.Services.AddSingleton<IDistributedLockProvider>(sp =>
        {
            var connection = ConnectionMultiplexer
                .Connect(configuration["Redis:Configuration"]!);
            return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
        });

        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]?
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.RemovePostFix("/"))
                            .ToArray() ?? Array.Empty<string>()
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        // TODO: ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED: "true"
        context.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            // see https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-3.1
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        // TODO: Wechat H5 AddOAuth AuthenticationScheme ?

        // TODO: 注册第三方认证
        // Configure<AbpIdentityOptions>(options =>
        // {
        //     options.ExternalLoginProviders.Add<FakeExternalLoginProvider>(FakeExternalLoginProvider.Name);
        // });


        context.Services.AddHttpApi(typeof(IWechatSnsUserApi), opt =>
        {
            configuration.GetSection("IWechatSnsUserApi").Bind(opt);
        });
        context.Services.AddHttpApi(typeof(IWeChatAppApi), opt =>
        {
            configuration.GetSection("IWeChatAppApi").Bind(opt);
        });

        Configure<WechatOptions>(options => configuration.GetSection(WechatOptions.SectionName).Bind(options));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        app.UseForwardedHeaders();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseAuthorization();
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
            {
                Predicate = (check) => check.Tags.Contains("ready"),
            });

            endpoints.MapHealthChecks("/health/live", new HealthCheckOptions()
            {
            });
        });
    }
}
