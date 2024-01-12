namespace DotnetAPI.Dtos;

public partial class PostUpsertDto
{
    public int? Id { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
}