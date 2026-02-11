// Provenance: created 2026-02-11, created_by AI-assisted (Cursor), source PostHub brownfield unit tests, version 1.0
using Microsoft.Extensions.Configuration;

namespace PostHubAPI.Tests.Helpers;

/// <summary>
/// Builds IConfiguration with JWT settings required by UserService.
/// </summary>
public static class JwtConfigurationHelper
{
    private const string DefaultSecret = "TestSecretKeyThatIsLongEnoughForHmacSha256Algorithm";

    /// <summary>
    /// Creates an IConfiguration instance with JWT:Secret, JWT:Issuer, JWT:Audience.
    /// </summary>
    public static IConfiguration Create()
    {
        var data = new Dictionary<string, string?>
        {
            ["JWT:Secret"] = DefaultSecret,
            ["JWT:Issuer"] = "PostHubAPI.Tests",
            ["JWT:Audience"] = "PostHubAPI.Tests"
        };
        return new ConfigurationBuilder()
            .AddInMemoryCollection(data)
            .Build();
    }
}
