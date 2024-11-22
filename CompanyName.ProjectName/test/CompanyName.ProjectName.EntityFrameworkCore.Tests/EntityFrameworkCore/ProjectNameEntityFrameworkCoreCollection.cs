using Xunit;

namespace CompanyName.ProjectName.EntityFrameworkCore;

[CollectionDefinition(ProjectNameTestConsts.CollectionDefinitionName)]
public class ProjectNameEntityFrameworkCoreCollection : ICollectionFixture<ProjectNameEntityFrameworkCoreFixture>
{

}
