using CompanyName.ProjectName.ModuleName.Samples;
using Xunit;

namespace CompanyName.ProjectName.ModuleName.MongoDB.Domains;

[Collection(MongoTestCollection.Name)]
public class MongoDBSampleDomain_Tests : SampleManager_Tests<ModuleNameMongoDbTestModule>
{

}
