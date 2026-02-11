// Provenance: created 2026-02-11, created_by AI-assisted (Cursor), source PostHub brownfield unit tests, version 1.0
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PostHubAPI.Data;
using PostHubAPI.Dtos.Post;
using PostHubAPI.Exceptions;
using PostHubAPI.Profiles;
using PostHubAPI.Services.Implementations;
using PostHubAPI.Tests.Helpers;
using Xunit;

namespace PostHubAPI.Tests.Services;

/// <summary>
/// Unit tests for <see cref="PostService"/> CRUD and NotFound behavior.
/// </summary>
public class PostServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly PostService _sut;

    public PostServiceTests()
    {
        _context = InMemoryDbContextHelper.CreateContext();
        _context.Database.EnsureCreated();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<PostProfile>();
            cfg.AddProfile<CommentProfile>();
        });
        _mapper = config.CreateMapper();
        _sut = new PostService(_context, _mapper);
    }

    public void Dispose() => _context.Dispose();

    private static CreatePostDto NewPostDto(string title = "Title", string body = "Body")
    {
        return new CreatePostDto { Title = title, Body = body };
    }

    [Fact]
    public async Task GetAllPostsAsync_WhenEmpty_ReturnsEmptySequence()
    {
        var result = await _sut.GetAllPostsAsync();
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllPostsAsync_AfterCreatingPosts_ReturnsMappedDtos()
    {
        await _sut.CreateNewPostAsync(NewPostDto("A", "Body A"));
        await _sut.CreateNewPostAsync(NewPostDto("B", "Body B"));

        var result = await _sut.GetAllPostsAsync();
        var list = result.ToList();
        Assert.Equal(2, list.Count);
        Assert.Contains(list, p => p.Title == "A" && p.Body == "Body A");
        Assert.Contains(list, p => p.Title == "B" && p.Body == "Body B");
    }

    [Fact]
    public async Task GetPostByIdAsync_WhenExists_ReturnsMappedDto()
    {
        var created = await _sut.CreateNewPostAsync(NewPostDto("T", "B"));
        var result = await _sut.GetPostByIdAsync(created.Id);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal("T", result.Title);
        Assert.Equal("B", result.Body);
    }

    [Fact]
    public async Task GetPostByIdAsync_WhenNotExists_ThrowsNotFoundException()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.GetPostByIdAsync(999));
    }

    [Fact]
    public async Task CreateNewPostAsync_ReturnsNewDtoWithId()
    {
        var created = await _sut.CreateNewPostAsync(NewPostDto());
        Assert.True(created.Id > 0);
    }

    [Fact]
    public async Task EditPostAsync_WhenExists_UpdatesAndReturnsDto()
    {
        var created = await _sut.CreateNewPostAsync(NewPostDto("Old", "OldBody"));
        var dto = new EditPostDto { Title = "New", Body = "NewBody" };

        var result = await _sut.EditPostAsync(created.Id, dto);

        Assert.Equal("New", result.Title);
        Assert.Equal("NewBody", result.Body);
        var inDb = await _context.Posts.AsNoTracking().FirstAsync(p => p.Id == created.Id);
        Assert.Equal("New", inDb.Title);
    }

    [Fact]
    public async Task EditPostAsync_WhenNotExists_ThrowsNotFoundException()
    {
        var dto = new EditPostDto { Title = "X", Body = "Y" };
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.EditPostAsync(999, dto));
    }

    [Fact]
    public async Task DeletePostAsync_WhenExists_RemovesPost()
    {
        var created = await _sut.CreateNewPostAsync(NewPostDto());
        await _sut.DeletePostAsync(created.Id);
        var found = await _context.Posts.FindAsync(created.Id);
        Assert.Null(found);
    }

    [Fact]
    public async Task DeletePostAsync_WhenNotExists_ThrowsNotFoundException()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _sut.DeletePostAsync(999));
    }
}
