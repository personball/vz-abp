using Volo.Abp.Modularity;

namespace CompanyName.ProjectName;

[DependsOn(
    typeof(ProjectNameDomainModule),
    typeof(ProjectNameTestBaseModule)
)]
public class ProjectNameDomainTestModule : AbpModule
{

}
