using CompanyName.ProjectName.Samples;
using Xunit;

namespace CompanyName.ProjectName.EntityFrameworkCore.Domains;

[Collection(ProjectNameTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<ProjectNameEntityFrameworkCoreTestModule>
{

}
