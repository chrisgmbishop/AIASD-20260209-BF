// Provenance: created 2026-02-11, created_by AI-assisted (Cursor), source PostHub brownfield unit tests, version 1.0
using Microsoft.AspNetCore.Identity;
using PostHubAPI.Data;
using PostHubAPI.Dtos.User;
using PostHubAPI.Models;
using PostHubAPI.Services.Implementations;
using PostHubAPI.Tests.Helpers;
using Xunit;

namespace PostHubAPI.Tests.Services;

/// <summary>
/// Unit tests for <see cref="UserService"/> (Register, Login) using real Identity and in-memory store.
/// </summary>
public class UserServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly UserService _sut;

    public UserServiceTests()
    {
        _context = InMemoryDbContextHelper.CreateContext();
        _context.Database.EnsureCreated();
        var userManager = UserManagerHelper.Create(_context);
        var configuration = JwtConfigurationHelper.Create();
        _sut = new UserService(configuration, userManager);
    }

    public void Dispose() => _context?.Dispose();

    private static RegisterUserDto ValidRegisterDto()
    {
        return new RegisterUserDto
        {
            Email = "user@example.com",
            Username = "testuser",
            Password = "Password1!",
            ConfirmPassword = "Password1!"
        };
    }

    private static LoginUserDto ValidLoginDto()
    {
        return new LoginUserDto { Username = "testuser", Password = "Password1!" };
    }

    [Fact]
    public async Task Register_WithValidDto_ReturnsJwtToken()
    {
        var dto = ValidRegisterDto();

        var token = await _sut.Register(dto);

        Assert.NotNull(token);
        Assert.NotEmpty(token);
        Assert.DoesNotContain("System.Threading.Tasks.Task", token);
    }

    [Fact]
    public async Task Register_ThenLogin_WithSameCredentials_ReturnsToken()
    {
        var registerDto = ValidRegisterDto();
        await _sut.Register(registerDto);
        var loginDto = ValidLoginDto();

        var token = await _sut.Login(loginDto);

        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ThrowsArgumentException()
    {
        var dto = ValidRegisterDto();
        await _sut.Register(dto);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.Register(dto));
        Assert.Contains("already exists", ex.Message);
    }

    [Fact]
    public async Task Login_WithUnregisteredUsername_ThrowsArgumentException()
    {
        var dto = new LoginUserDto { Username = "nobody", Password = "AnyPass1!" };

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.Login(dto));
        Assert.Contains("not registered", ex.Message);
    }

    [Fact]
    public async Task Login_WithWrongPassword_ThrowsArgumentException()
    {
        await _sut.Register(ValidRegisterDto());
        var dto = new LoginUserDto { Username = "testuser", Password = "WrongPassword1!" };

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _sut.Login(dto));
        Assert.Contains("Unable to authenticate", ex.Message);
    }
}
