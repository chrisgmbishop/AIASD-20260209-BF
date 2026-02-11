using PostHubAPI.Dtos.Comment;

namespace PostHubAPI.Services.Interfaces;

public interface ICommentService
{
    Task<ReadCommentDto> GetCommentAsync(int id);
    Task<ReadCommentDto> CreateNewCommentAsync(int postId, CreateCommentDto newComment);
    Task DeleteCommentAsync(int id);
    Task<ReadCommentDto> EditCommentAsync(int id, EditCommentDto dto);
}