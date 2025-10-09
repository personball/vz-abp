using CompanyName.ProjectName.ModuleName.MongoDB;
using CompanyName.ProjectName.ModuleName.Samples;
using Xunit;

namespace CompanyName.ProjectName.ModuleName.MongoDb.Applications;

[Collection(MongoTestCollection.Name)]
public class MongoDBSampleAppService_Tests : SampleAppService_Tests<ModuleNameMongoDbTestModule>
{

}
