// Provenance: created 2026-02-11, created_by AI-assisted (Cursor), source PostHub brownfield unit tests, version 1.0
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PostHubAPI.Data;
using PostHubAPI.Models;

namespace PostHubAPI.Tests.Helpers;

/// <summary>
/// Builds a real <see cref="UserManager{TUser}"/> backed by an in-memory store for unit testing UserService.
/// </summary>
public static class UserManagerHelper
{
    /// <summary>
    /// Creates a UserManager for the given in-memory ApplicationDbContext.
    /// Caller must dispose the context when done; UserManager holds a reference to the store which uses the context.
    /// </summary>
    public static UserManager<User> Create(ApplicationDbContext context)
    {
        var store = new Microsoft.AspNetCore.Identity.EntityFrameworkCore.UserStore<User>(context);
        var options = new OptionsWrapper<IdentityOptions>(new IdentityOptions());
        var hasher = new PasswordHasher<User>();
        var userValidators = new List<IUserValidator<User>> { new UserValidator<User>() };
        var passwordValidators = new List<IPasswordValidator<User>> { new PasswordValidator<User>() };
        var keyNormalizer = new UpperInvariantLookupNormalizer();
        var errors = new IdentityErrorDescriber();
        var logger = new Mock<ILogger<UserManager<User>>>().Object;
        var serviceProvider = new Mock<IServiceProvider>().Object;

        return new UserManager<User>(
            store,
            options,
            hasher,
            userValidators,
            passwordValidators,
            keyNormalizer,
            errors,
            serviceProvider,
            logger);
    }
}
