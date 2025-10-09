using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace CompanyName.ProjectName.ModuleName.EntityFrameworkCore;

[DependsOn(
    typeof(ModuleNameDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class ModuleNameEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ModuleNameDbContext>(options =>
        {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
        });
    }
}
