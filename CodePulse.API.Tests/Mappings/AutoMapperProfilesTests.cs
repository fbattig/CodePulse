using AutoMapper;
using CodePulse.API.Mappings;
using Microsoft.Extensions.Logging.Abstractions;

namespace CodePulse.API.Tests.Mappings
{
    public class AutoMapperProfilesTests
    {
        [Fact]
        public void AutoMapperConfiguration_IsValid()
        {
            // Arrange
            var configuration = new MapperConfiguration(
                cfg => cfg.AddProfile<AutoMapperProfiles>(),
                NullLoggerFactory.Instance);

            // Act + Assert (throws if any mapping is misconfigured)
            configuration.AssertConfigurationIsValid();
        }
    }
}
