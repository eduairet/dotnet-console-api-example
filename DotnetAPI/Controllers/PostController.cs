using DotnetAPI.Models;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public partial class PostsController(IConfiguration config) : ControllerBase
{
    private readonly DataContext _data = new(config);

    [AllowAnonymous]
    [HttpGet]
    public IEnumerable<Post> Posts(int? userId, int? postId, string? searchValue)
    {
        string sql = $"EXEC TutorialAppSchema.spPosts_Get";
        if (userId != null && userId > 0) sql += $" @UserId = {userId},";
        if (postId != null && postId > 0) sql += $" @PostId = {postId},";
        if (searchValue != null) sql += $" @SearchValue = '{searchValue}',";
        sql = sql.TrimEnd(',');
        IEnumerable<Post> posts = _data.LoadData<Post>(sql);
        return posts;
    }

    [HttpGet("my-posts")]
    public IEnumerable<Post> MyPosts()
    {
        string sql = $"EXEC TutorialAppSchema.spPosts_Get @UserId = {User.FindFirst("userId")?.Value}";
        IEnumerable<Post> posts = _data.LoadData<Post>(sql);
        return posts;
    }


    [HttpPut]
    public IActionResult Upsert(PostUpsertDto post)
    {
        string sql = @$"EXEC TutorialAppSchema.spPosts_Upsert @UserId = {User.FindFirst("userId")?.Value}
            , @Title = '{post.Title}'
            , @Content = '{post.Content}'";
        if (post.Id != null && post.Id > 0) sql += $"\n, @PostId = {post.Id}";
        if (_data.ExecuteSql(sql)) return Ok();
        return BadRequest("Failed to create post");
    }

    [HttpDelete]
    public IActionResult DeletePost(int postId)
    {
        string sql = $"EXEC TutorialAppSchema.spPosts_Delete @UserId = {User.FindFirst("userId")?.Value}, @PostId = {postId}";
        if (_data.ExecuteSql(sql)) return Ok();
        return BadRequest("Failed to delete post");
    }
}