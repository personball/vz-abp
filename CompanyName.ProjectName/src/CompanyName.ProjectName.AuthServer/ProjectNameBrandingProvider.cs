using Microsoft.Extensions.Localization;
using CompanyName.ProjectName.Localization;
using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace CompanyName.ProjectName;

[Dependency(ReplaceServices = true)]
public class ProjectNameBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<ProjectNameResource> _localizer;

    public ProjectNameBrandingProvider(IStringLocalizer<ProjectNameResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
