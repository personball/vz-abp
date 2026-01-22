using CompanyName.ProjectName.Localization;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Timing;
using System;

namespace CompanyName.ProjectName;

[DependsOn(
    typeof(AbpAuditLoggingDomainSharedModule),
    typeof(AbpBackgroundJobsDomainSharedModule),
    typeof(AbpFeatureManagementDomainSharedModule),
    typeof(AbpIdentityDomainSharedModule),
    typeof(AbpOpenIddictDomainSharedModule),
    typeof(AbpPermissionManagementDomainSharedModule),
    typeof(AbpSettingManagementDomainSharedModule),
    typeof(AbpTenantManagementDomainSharedModule)
    )]
public class ProjectNameDomainSharedModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        ProjectNameGlobalFeatureConfigurator.Configure();
        ProjectNameModuleExtensionConfigurator.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpClockOptions>(options =>
        {
            options.Kind = DateTimeKind.Utc;
        });
        /*
            {
                "clockNow": "2026-01-22T08:34:58.8257187Z",
                "dateTimeNow": "2026-01-22T08:34:58.825719Z",
                "dateTimeOffsetNow": "2026-01-22T16:34:58.8257206+08:00",
                "dateTimeNowStr": "2026-01-22 16:34:58",
                "dateTimeUtcNowStr": "2026-01-22 08:34:58",
                "dateTimeOffsetNowStr": "2026-01-22 16:34:58 +08:00",
                "dateTimeOffsetUtcNowStr": "2026-01-22 08:34:58 +00:00"
            }
            在进程空间内，DateTime.Now 和 DateTimeOffset.Now 依然是服务端时区，东八区
            直接以时间类型输出webapi时，会被abp框架处理为UTC时间
            存到pg里时，也会被处理为UTC时间
        */

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ProjectNameDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<ProjectNameResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/ProjectName");

            options.DefaultResourceType = typeof(ProjectNameResource);
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("ProjectName", typeof(ProjectNameResource));
        });
    }
}
