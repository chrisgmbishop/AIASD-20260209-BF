// Provenance: created 2026-02-11, created_by AI-assisted (Cursor), source PostHub brownfield unit tests, version 1.0
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using PostHubAPI.Controllers;
using PostHubAPI.Dtos.Post;
using PostHubAPI.Exceptions;
using PostHubAPI.Services.Interfaces;
using Xunit;

namespace PostHubAPI.Tests.Controllers;

/// <summary>
/// Unit tests for <see cref="PostController"/> with mocked <see cref="IPostService"/>.
/// </summary>
public class PostControllerTests
{
    private readonly Mock<IPostService> _postServiceMock;
    private readonly PostController _sut;

    public PostControllerTests()
    {
        _postServiceMock = new Mock<IPostService>();
        _sut = new PostController(_postServiceMock.Object);
        SetupControllerContext();
    }

    private void SetupControllerContext()
    {
        var actionDescriptor = new ControllerActionDescriptor
        {
            ControllerName = "Post",
            ActionName = nameof(PostController.GetAllPosts),
            ControllerTypeInfo = typeof(PostController).GetTypeInfo(),
            MethodInfo = typeof(PostController).GetMethod(nameof(PostController.GetAllPosts))
        };
        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            actionDescriptor,
            new ModelStateDictionary());
        _sut.ControllerContext = new ControllerContext(actionContext);
    }

    [Fact]
    public async Task GetAllPosts_ReturnsOkWithPosts()
    {
        var posts = new List<ReadPostDto>
        {
            new ReadPostDto { Id = 1, Title = "A", Body = "Body A", CreationTime = DateTime.UtcNow, Likes = 0 }
        };
        _postServiceMock.Setup(s => s.GetAllPostsAsync()).ReturnsAsync(posts);

        var result = await _sut.GetAllPosts();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(posts, okResult.Value);
    }

    [Fact]
    public async Task GetPostById_WhenExists_ReturnsOkWithPost()
    {
        var post = new ReadPostDto { Id = 1, Title = "T", Body = "B", CreationTime = DateTime.UtcNow, Likes = 0 };
        _postServiceMock.Setup(s => s.GetPostByIdAsync(1)).ReturnsAsync(post);

        var result = await _sut.GetPostById(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(post, okResult.Value);
    }

    [Fact]
    public async Task GetPostById_WhenNotFound_ReturnsNotFound()
    {
        _postServiceMock.Setup(s => s.GetPostByIdAsync(999))
            .ThrowsAsync(new NotFoundException("Post not found!"));

        var result = await _sut.GetPostById(999);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Post not found!", notFound.Value);
    }

    [Fact]
    public async Task CreatePost_WhenModelValid_ReturnsCreatedWithLocation()
    {
        var dto = new CreatePostDto { Title = "T", Body = "B" };
        var createdPost = new ReadPostDto { Id = 42, Title = "T", Body = "B", CreationTime = DateTime.UtcNow, Likes = 0 };
        _postServiceMock.Setup(s => s.CreateNewPostAsync(It.IsAny<CreatePostDto>())).ReturnsAsync(createdPost);

        var result = await _sut.CreatePost(dto);

        var created = Assert.IsType<CreatedResult>(result);
        Assert.Same(createdPost, created.Value);
        Assert.Contains("/api/Post/42", created.Location);
    }

    [Fact]
    public async Task CreatePost_WhenModelInvalid_ReturnsBadRequest()
    {
        var dto = new CreatePostDto { Title = "", Body = "" };
        _sut.ModelState.AddModelError("Title", "Required");

        var result = await _sut.CreatePost(dto);

        Assert.IsType<BadRequestObjectResult>(result);
        _postServiceMock.Verify(s => s.CreateNewPostAsync(It.IsAny<CreatePostDto>()), Times.Never);
    }

    [Fact]
    public async Task EditPost_WhenModelValidAndExists_ReturnsOkWithPost()
    {
        var dto = new EditPostDto { Title = "New", Body = "NewBody" };
        var readDto = new ReadPostDto { Id = 1, Title = "New", Body = "NewBody", CreationTime = DateTime.UtcNow, Likes = 0 };
        _postServiceMock.Setup(s => s.EditPostAsync(1, dto)).ReturnsAsync(readDto);

        var result = await _sut.EditPost(1, dto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(readDto, okResult.Value);
    }

    [Fact]
    public async Task EditPost_WhenNotFound_ReturnsNotFound()
    {
        var dto = new EditPostDto { Title = "X", Body = "Y" };
        _postServiceMock.Setup(s => s.EditPostAsync(999, dto))
            .ThrowsAsync(new NotFoundException("Post not found!"));

        var result = await _sut.EditPost(999, dto);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Post not found!", notFound.Value);
    }

    [Fact]
    public async Task DeletePost_WhenExists_ReturnsNoContent()
    {
        _postServiceMock.Setup(s => s.DeletePostAsync(1)).Returns(Task.CompletedTask);

        var result = await _sut.DeletePost(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeletePost_WhenNotFound_ReturnsNotFound()
    {
        _postServiceMock.Setup(s => s.DeletePostAsync(999))
            .ThrowsAsync(new NotFoundException("Post not found!"));

        var result = await _sut.DeletePost(999);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Post not found!", notFound.Value);
    }
}
