using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace CompanyName.ProjectName.ModuleName.MongoDB;

[DependsOn(
    typeof(ModuleNameApplicationTestModule),
    typeof(ModuleNameMongoDbModule)
)]
public class ModuleNameMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}
