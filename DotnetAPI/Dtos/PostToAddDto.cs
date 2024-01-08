using System.ComponentModel.DataAnnotations;

namespace DotnetAPI.Dtos;

public partial class PostToAddDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Title { get; set; } = "";

    [Required]
    [StringLength(int.MaxValue, MinimumLength = 1)]
    public string Content { get; set; } = "";
}