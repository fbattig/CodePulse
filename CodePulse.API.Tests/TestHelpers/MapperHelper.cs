using AutoMapper;
using CodePulse.API.Mappings;
using Microsoft.Extensions.Logging.Abstractions;

namespace CodePulse.API.Tests.TestHelpers
{
    /// <summary>
    /// Builds a real <see cref="IMapper"/> from the application's AutoMapper profile so
    /// controller tests exercise the actual mapping configuration (not a mock).
    /// </summary>
    public static class MapperHelper
    {
        public static IMapper Create()
        {
            var configuration = new MapperConfiguration(
                cfg => cfg.AddProfile<AutoMapperProfiles>(),
                NullLoggerFactory.Instance);

            return configuration.CreateMapper();
        }
    }
}
