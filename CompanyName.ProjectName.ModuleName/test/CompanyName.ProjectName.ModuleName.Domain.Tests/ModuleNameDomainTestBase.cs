using Volo.Abp.Modularity;

namespace CompanyName.ProjectName.ModuleName;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class ModuleNameDomainTestBase<TStartupModule> : ModuleNameTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
