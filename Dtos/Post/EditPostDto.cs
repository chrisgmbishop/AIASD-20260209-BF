using System.ComponentModel.DataAnnotations;

namespace PostHubAPI.Dtos.Post;

public class EditPostDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Body { get; set; } = string.Empty;
}