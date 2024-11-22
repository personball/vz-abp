using Volo.Abp.Modularity;

namespace CompanyName.ProjectName;

public abstract class ProjectNameApplicationTestBase<TStartupModule> : ProjectNameTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
