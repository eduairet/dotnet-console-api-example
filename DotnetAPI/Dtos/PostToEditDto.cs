using System.ComponentModel.DataAnnotations;

namespace DotnetAPI.Dtos;

public partial class PostToEditDto : PostToAddDto
{
    [Required]
    public int Id { get; set; }
}