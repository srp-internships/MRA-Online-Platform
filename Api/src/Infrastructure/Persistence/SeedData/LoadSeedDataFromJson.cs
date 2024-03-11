using Core.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Reflection;

namespace Infrastructure.Persistence.SeedData
{
    public class LoadSeedDataFromJson : ILoadSeedData
    {
        private readonly string _seedDataFile;
        public LoadSeedDataFromJson(IHostEnvironment environment)
        {
            _seedDataFile = environment.IsProduction() ? "BlankData.json" : "TestData.json";
        }

        public SeedDataModel Load()
        {
           string path = Path.Combine(Assembly.GetExecutingAssembly().Location.GetDirectoryPathFromFile(), "Persistence", _seedDataFile);
           return JsonConvert.DeserializeObject<SeedDataModel>(File.ReadAllText(path));
        }
    }
}
