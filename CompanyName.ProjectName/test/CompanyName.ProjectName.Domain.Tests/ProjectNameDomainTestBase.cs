using Volo.Abp.Modularity;

namespace CompanyName.ProjectName;

/* Inherit from this class for your domain layer tests. */
public abstract class ProjectNameDomainTestBase<TStartupModule> : ProjectNameTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
