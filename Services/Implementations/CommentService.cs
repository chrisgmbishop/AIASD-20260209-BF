using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PostHubAPI.Data;
using PostHubAPI.Dtos.Comment;
using PostHubAPI.Exceptions;
using PostHubAPI.Models;
using PostHubAPI.Services.Interfaces;

namespace PostHubAPI.Services.Implementations;

public class CommentService(ApplicationDbContext context, IMapper mapper) : ICommentService
{
    public async Task<ReadCommentDto> GetCommentAsync(int id)
    {
        Comment? comment = await context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        if (comment != null)
        {
            ReadCommentDto commentDto = mapper.Map<ReadCommentDto>(comment);
            return commentDto;
        }

        throw new NotFoundException("Comment not found!!");
    }

    public async Task<ReadCommentDto> CreateNewCommentAsync(int postId, CreateCommentDto newComment)
    {
        Post? post = await context.Posts.FirstOrDefaultAsync(c => c.Id == postId);
        if (post == null)
        {
            throw new NotFoundException("Post not found!");
        }

        Comment comment = mapper.Map<Comment>(newComment);
        comment.Post = post;
        comment.PostId = postId;
        context.Comments.Add(comment);
        await context.SaveChangesAsync();
        Comment? created = await context.Comments.FirstOrDefaultAsync(c => c.Id == comment.Id);
        if (created == null)
        {
            throw new InvalidOperationException("Comment was not persisted.");
        }
        return mapper.Map<ReadCommentDto>(created);
    }

    public async Task<ReadCommentDto> EditCommentAsync(int id, EditCommentDto dto)
    {
        Comment? commentToEdit = await context.Comments.FirstOrDefaultAsync(comment => comment.Id == id);
        if (commentToEdit != null) 
        {
            mapper.Map(dto, commentToEdit);
            await context.SaveChangesAsync();

            ReadCommentDto readCommentDto = mapper.Map<ReadCommentDto>(commentToEdit);
            return readCommentDto;
        }

        throw new NotFoundException("Comment not found!");
    }

    public async Task DeleteCommentAsync(int id)
    {
        Comment? comment = await context.Comments.FirstOrDefaultAsync(comment => comment.Id == id);
        if(comment != null)
        {
            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
        }
        else
        {
            throw new NotFoundException("Comment not found!");
        }
    }
}