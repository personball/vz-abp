using Volo.Abp.Modularity;

namespace CompanyName.ProjectName.ModuleName;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class ModuleNameApplicationTestBase<TStartupModule> : ModuleNameTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
