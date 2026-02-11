// Provenance: created 2026-02-11, created_by AI-assisted (Cursor), source PostHub brownfield unit tests, version 1.0
using Microsoft.EntityFrameworkCore;
using PostHubAPI.Data;

namespace PostHubAPI.Tests.Helpers;

/// <summary>
/// Provides a fresh in-memory <see cref="ApplicationDbContext"/> per test to avoid shared state.
/// </summary>
public static class InMemoryDbContextHelper
{
    /// <summary>
    /// Creates DbContextOptions for ApplicationDbContext using a unique in-memory database name.
    /// </summary>
    public static DbContextOptions<ApplicationDbContext> CreateOptions(string? databaseName = null)
    {
        var name = databaseName ?? "TestDb_" + Guid.NewGuid().ToString("N");
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(name)
            .Options;
    }

    /// <summary>
    /// Creates a new ApplicationDbContext instance with a unique in-memory database.
    /// Caller is responsible for disposing.
    /// </summary>
    public static ApplicationDbContext CreateContext(string? databaseName = null)
    {
        return new ApplicationDbContext(CreateOptions(databaseName));
    }
}
