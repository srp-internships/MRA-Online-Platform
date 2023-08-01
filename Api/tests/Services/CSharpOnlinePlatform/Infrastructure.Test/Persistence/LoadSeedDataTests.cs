using Infrastructure.Persistence.SeedData;
using Microsoft.Extensions.Hosting;
using Moq;
using NUnit.Framework;

namespace Infrastructure.Test.Persistence
{
    public class LoadSeedDataTests
    {
        [Test]
        public void LoadSeedDataFromJson_InDevelopment_ShouldHaveAnyTeacher()
        {
            var hostEnviorent = new Mock<IHostEnvironment>(MockBehavior.Strict);
            hostEnviorent.Setup(x => x.EnvironmentName).Returns("Development");

            var seedDataFromJson = new LoadSeedDataFromJson(hostEnviorent.Object);
            var model = seedDataFromJson.Load();
            Assert.That(model.Teachers, Is.Not.Null);
        }

        [Test]
        public void LoadSeedDataFromJson_InProduction_ShouldNotHaveAnyTeacher()
        {
            var hostEnviorent = new Mock<IHostEnvironment>(MockBehavior.Strict);
            hostEnviorent.Setup(x => x.EnvironmentName).Returns("Production");

            var seedDataFromJson = new LoadSeedDataFromJson(hostEnviorent.Object);
            var model = seedDataFromJson.Load();
            Assert.That(model.Teachers, Is.Null);
        }
    }
}
