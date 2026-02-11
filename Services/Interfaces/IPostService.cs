using PostHubAPI.Dtos.Post;

namespace PostHubAPI.Services.Interfaces;

public interface IPostService
{
    Task<IEnumerable<ReadPostDto>> GetAllPostsAsync();
    Task<ReadPostDto> GetPostByIdAsync(int id);
    Task<ReadPostDto> CreateNewPostAsync(CreatePostDto dto);
    Task<ReadPostDto> EditPostAsync(int id, EditPostDto dto);
    Task DeletePostAsync(int id);
}