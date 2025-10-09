using Volo.Abp.Modularity;

namespace CompanyName.ProjectName.ModuleName;

[DependsOn(
    typeof(ModuleNameDomainModule),
    typeof(ModuleNameTestBaseModule)
)]
public class ModuleNameDomainTestModule : AbpModule
{

}
