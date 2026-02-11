// Provenance: created 2026-02-11, created_by AI-assisted (Cursor), source PostHub brownfield unit tests, version 1.0
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PostHubAPI.Data;
using PostHubAPI.Dtos.Comment;
using PostHubAPI.Exceptions;
using PostHubAPI.Models;
using PostHubAPI.Profiles;
using PostHubAPI.Services.Implementations;
using PostHubAPI.Tests.Helpers;
using Xunit;

namespace PostHubAPI.Tests.Services;

/// <summary>
/// Unit tests for <see cref="CommentService"/> CRUD and NotFound behavior.
/// </summary>
public class CommentServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly CommentService _sut;

    public CommentServiceTests()
    {
        _context = InMemoryDbContextHelper.CreateContext();
        _context.Database.EnsureCreated();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PostProfile>();
            cfg.AddProfile<CommentProfile>();
        });
        _mapper = config.CreateMapper();
        _sut = new CommentService(_context, _mapper);
    }

    public void Dispose() => _context.Dispose();

    private static CreateCommentDto NewCommentDto(string body = "Comment body")
    {
        return new CreateCommentDto { Body = body };
    }

    private async Task<int> CreatePostAsync(string title = "Post", string body = "Body")
    {
        var post = new Post { Title = title, Body = body };
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return post.Id;
    }

    [Fact]
    public async Task GetCommentAsync_WhenExists_ReturnsMappedDto()
    {
        var postId = await CreatePostAsync();
        var created = await _sut.CreateNewCommentAsync(postId, NewCommentDto("My comment"));

        var result = await _sut.GetCommentAsync(created.Id);

        Assert.Equal(created.Id, result.Id);
        Assert.Equal("My comment", result.Body);
    }

    [Fact]
    public async Task GetCommentAsync_WhenNotExists_ThrowsNotFoundException()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.GetCommentAsync(999));
    }

    [Fact]
    public async Task CreateNewCommentAsync_WhenPostExists_ReturnsNewDtoWithId()
    {
        var postId = await CreatePostAsync();
        var created = await _sut.CreateNewCommentAsync(postId, NewCommentDto());
        Assert.True(created.Id > 0);
    }

    [Fact]
    public async Task CreateNewCommentAsync_WhenPostNotExists_ThrowsNotFoundException()
    {
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _sut.CreateNewCommentAsync(999, NewCommentDto()));
    }

    [Fact]
    public async Task EditCommentAsync_WhenExists_UpdatesAndReturnsDto()
    {
        var postId = await CreatePostAsync();
        var created = await _sut.CreateNewCommentAsync(postId, NewCommentDto("Old"));
        var dto = new EditCommentDto { Body = "Updated" };

        var result = await _sut.EditCommentAsync(created.Id, dto);

        Assert.Equal("Updated", result.Body);
    }

    [Fact]
    public async Task EditCommentAsync_WhenNotExists_ThrowsNotFoundException()
    {
        var dto = new EditCommentDto { Body = "X" };
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.EditCommentAsync(999, dto));
    }

    [Fact]
    public async Task DeleteCommentAsync_WhenExists_RemovesComment()
    {
        var postId = await CreatePostAsync();
        var created = await _sut.CreateNewCommentAsync(postId, NewCommentDto());
        await _sut.DeleteCommentAsync(created.Id);
        var found = await _context.Comments.FindAsync(created.Id);
        Assert.Null(found);
    }

    [Fact]
    public async Task DeleteCommentAsync_WhenNotExists_ThrowsNotFoundException()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.DeleteCommentAsync(999));
    }
}
