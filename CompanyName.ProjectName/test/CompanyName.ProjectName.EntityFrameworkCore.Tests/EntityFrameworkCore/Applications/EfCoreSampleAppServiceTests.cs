using CompanyName.ProjectName.Samples;
using Xunit;

namespace CompanyName.ProjectName.EntityFrameworkCore.Applications;

[Collection(ProjectNameTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<ProjectNameEntityFrameworkCoreTestModule>
{

}
