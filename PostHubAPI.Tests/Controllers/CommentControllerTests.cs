// Provenance: created 2026-02-11, created_by AI-assisted (Cursor), source PostHub brownfield unit tests, version 1.0
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using PostHubAPI.Controllers;
using PostHubAPI.Dtos.Comment;
using PostHubAPI.Exceptions;
using PostHubAPI.Services.Interfaces;
using Xunit;

namespace PostHubAPI.Tests.Controllers;

/// <summary>
/// Unit tests for <see cref="CommentController"/> with mocked <see cref="ICommentService"/>.
/// </summary>
public class CommentControllerTests
{
    private readonly Mock<ICommentService> _commentServiceMock;
    private readonly CommentController _sut;

    public CommentControllerTests()
    {
        _commentServiceMock = new Mock<ICommentService>();
        _sut = new CommentController(_commentServiceMock.Object);
        SetupControllerContext();
    }

    private void SetupControllerContext()
    {
        var actionDescriptor = new ControllerActionDescriptor
        {
            ControllerName = "Comment",
            ActionName = nameof(CommentController.GetComment),
            ControllerTypeInfo = typeof(CommentController).GetTypeInfo(),
            MethodInfo = typeof(CommentController).GetMethod(nameof(CommentController.GetComment))
        };
        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            actionDescriptor,
            new ModelStateDictionary());
        _sut.ControllerContext = new ControllerContext(actionContext);
    }

    [Fact]
    public async Task GetComment_WhenExists_ReturnsOkWithComment()
    {
        var comment = new ReadCommentDto { Id = 1, Body = "Comment", CreationTime = DateTime.UtcNow };
        _commentServiceMock.Setup(s => s.GetCommentAsync(1)).ReturnsAsync(comment);

        var result = await _sut.GetComment(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(comment, okResult.Value);
    }

    [Fact]
    public async Task GetComment_WhenNotFound_ReturnsNotFound()
    {
        _commentServiceMock.Setup(s => s.GetCommentAsync(999))
            .ThrowsAsync(new NotFoundException("Comment not found!!"));

        var result = await _sut.GetComment(999);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Comment not found!!", notFound.Value);
    }

    [Fact]
    public async Task CreateNewComment_WhenModelValidAndPostExists_ReturnsCreated()
    {
        var dto = new CreateCommentDto { Body = "New comment" };
        var createdComment = new ReadCommentDto { Id = 10, Body = "New comment", CreationTime = DateTime.UtcNow };
        _commentServiceMock.Setup(s => s.CreateNewCommentAsync(1, It.IsAny<CreateCommentDto>())).ReturnsAsync(createdComment);

        var result = await _sut.CreateNewComment(1, dto);

        var created = Assert.IsType<CreatedResult>(result);
        Assert.Same(createdComment, created.Value);
        Assert.Contains("/api/Comment/10", created.Location);
    }

    [Fact]
    public async Task CreateNewComment_WhenModelInvalid_ReturnsBadRequest()
    {
        var dto = new CreateCommentDto { Body = "" };
        _sut.ModelState.AddModelError("Body", "Required");

        var result = await _sut.CreateNewComment(1, dto);

        Assert.IsType<BadRequestObjectResult>(result);
        _commentServiceMock.Verify(s => s.CreateNewCommentAsync(It.IsAny<int>(), It.IsAny<CreateCommentDto>()), Times.Never);
    }

    [Fact]
    public async Task CreateNewComment_WhenPostNotFound_ReturnsNotFound()
    {
        var dto = new CreateCommentDto { Body = "Comment" };
        _commentServiceMock.Setup(s => s.CreateNewCommentAsync(999, It.IsAny<CreateCommentDto>()))
            .ThrowsAsync(new NotFoundException("Post not found!"));

        var result = await _sut.CreateNewComment(999, dto);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Post not found!", notFound.Value);
    }

    [Fact]
    public async Task EditComment_WhenModelValidAndExists_ReturnsOkWithComment()
    {
        var dto = new EditCommentDto { Body = "Updated" };
        var readDto = new ReadCommentDto { Id = 1, Body = "Updated", CreationTime = DateTime.UtcNow };
        _commentServiceMock.Setup(s => s.EditCommentAsync(1, dto)).ReturnsAsync(readDto);

        var result = await _sut.EditComment(1, dto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(readDto, okResult.Value);
    }

    [Fact]
    public async Task EditComment_WhenNotFound_ReturnsNotFound()
    {
        var dto = new EditCommentDto { Body = "X" };
        _commentServiceMock.Setup(s => s.EditCommentAsync(999, dto))
            .ThrowsAsync(new NotFoundException("Comment not found!"));

        var result = await _sut.EditComment(999, dto);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Comment not found!", notFound.Value);
    }

    [Fact]
    public async Task DeleteComment_WhenExists_ReturnsNoContent()
    {
        _commentServiceMock.Setup(s => s.DeleteCommentAsync(1)).Returns(Task.CompletedTask);

        var result = await _sut.DeleteComment(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteComment_WhenNotFound_ReturnsNotFound()
    {
        _commentServiceMock.Setup(s => s.DeleteCommentAsync(999))
            .ThrowsAsync(new NotFoundException("Comment not found!"));

        var result = await _sut.DeleteComment(999);

        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Comment not found!", notFound.Value);
    }
}
