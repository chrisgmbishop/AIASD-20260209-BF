// Provenance: created 2026-02-11, created_by AI-assisted (Cursor), source PostHub brownfield unit tests, version 1.0
using Microsoft.AspNetCore.Mvc;
using Moq;
using PostHubAPI.Controllers;
using PostHubAPI.Dtos.User;
using PostHubAPI.Services.Interfaces;
using Xunit;

namespace PostHubAPI.Tests.Controllers;

/// <summary>
/// Unit tests for <see cref="UserController"/> (Register, Login) with mocked <see cref="IUserService"/>.
/// </summary>
public class UserControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly UserController _sut;

    public UserControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _sut = new UserController(_userServiceMock.Object);
    }

    [Fact]
    public async Task Register_WhenModelValid_ReturnsOkWithToken()
    {
        var dto = new RegisterUserDto
        {
            Email = "u@example.com",
            Username = "user",
            Password = "Pass1!",
            ConfirmPassword = "Pass1!"
        };
        var token = "jwt.token.here";
        _userServiceMock.Setup(s => s.Register(It.IsAny<RegisterUserDto>())).ReturnsAsync(token);

        var result = await _sut.Register(dto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(token, okResult.Value);
        _userServiceMock.Verify(s => s.Register(dto), Times.Once);
    }

    [Fact]
    public async Task Register_WhenServiceThrowsArgumentException_ReturnsBadRequest()
    {
        var dto = new RegisterUserDto
        {
            Email = "u@example.com",
            Username = "user",
            Password = "Pass1!",
            ConfirmPassword = "Pass1!"
        };
        _userServiceMock.Setup(s => s.Register(It.IsAny<RegisterUserDto>()))
            .ThrowsAsync(new ArgumentException("User already exists"));

        var result = await _sut.Register(dto);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("User already exists", badRequest.Value);
    }

    [Fact]
    public async Task Register_WhenModelInvalid_ReturnsBadRequest()
    {
        var dto = new RegisterUserDto { Email = "", Username = "", Password = "", ConfirmPassword = "" };
        _sut.ModelState.AddModelError("Email", "Required");

        var result = await _sut.Register(dto);

        Assert.IsType<BadRequestObjectResult>(result);
        _userServiceMock.Verify(s => s.Register(It.IsAny<RegisterUserDto>()), Times.Never);
    }

    [Fact]
    public async Task Login_WhenModelValid_ReturnsOkWithToken()
    {
        var dto = new LoginUserDto { Username = "user", Password = "Pass1!" };
        var token = "jwt.token";
        _userServiceMock.Setup(s => s.Login(It.IsAny<LoginUserDto>())).ReturnsAsync(token);

        var result = await _sut.Login(dto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(token, okResult.Value);
        _userServiceMock.Verify(s => s.Login(dto), Times.Once);
    }

    [Fact]
    public async Task Login_WhenServiceThrowsArgumentException_ReturnsBadRequest()
    {
        var dto = new LoginUserDto { Username = "nobody", Password = "x" };
        _userServiceMock.Setup(s => s.Login(It.IsAny<LoginUserDto>()))
            .ThrowsAsync(new ArgumentException("Not registered"));

        var result = await _sut.Login(dto);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Not registered", badRequest.Value);
    }

    [Fact]
    public async Task Login_WhenModelInvalid_ReturnsBadRequest()
    {
        var dto = new LoginUserDto { Username = "", Password = "" };
        _sut.ModelState.AddModelError("Username", "Required");

        var result = await _sut.Login(dto);

        Assert.IsType<BadRequestObjectResult>(result);
        _userServiceMock.Verify(s => s.Login(It.IsAny<LoginUserDto>()), Times.Never);
    }
}
