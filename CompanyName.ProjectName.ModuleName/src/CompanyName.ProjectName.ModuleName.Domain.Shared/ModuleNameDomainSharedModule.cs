using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using CompanyName.ProjectName.ModuleName.Localization;
using Volo.Abp.Domain;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace CompanyName.ProjectName.ModuleName;

[DependsOn(
    typeof(AbpValidationModule),
    typeof(AbpDddDomainSharedModule)
)]
public class ModuleNameDomainSharedModule : AbpModule
{
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
            options.FileSets.AddEmbedded<ModuleNameDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<ModuleNameResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/ModuleName");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("ModuleName", typeof(ModuleNameResource));
        });
    }
}
