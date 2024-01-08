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
    public IEnumerable<Post> Posts()
    {
        string sql = @"SELECT Id
                ,UserId
                ,Title
                ,Content
                ,CreatedAt
                ,UpdatedAt
            FROM TutorialAppSchema.Posts";
        IEnumerable<Post> posts = _data.LoadData<Post>(sql);
        return posts;
    }

    [AllowAnonymous]
    [HttpGet("by-user/{userId:int}")]
    public IEnumerable<Post> PostsByUser(int userId)
    {
        string sql = @"SELECT Id
                ,UserId
                ,Title
                ,Content
                ,CreatedAt
                ,UpdatedAt
            FROM TutorialAppSchema.Posts
            WHERE UserId = " + userId.ToString();
        IEnumerable<Post> posts = _data.LoadData<Post>(sql);
        return posts;
    }

    [AllowAnonymous]
    [HttpGet("{postId:int}")]
    public IEnumerable<Post> Post(int postId)
    {
        string sql = @"SELECT Id
                ,UserId
                ,Title
                ,Content
                ,CreatedAt
                ,UpdatedAt
            FROM TutorialAppSchema.Posts
            WHERE id = " + postId.ToString();
        IEnumerable<Post> posts = _data.LoadData<Post>(sql);
        return posts;
    }

    [HttpGet("my-posts")]
    public IEnumerable<Post> MyPosts()
    {
        string sql = @"SELECT Id
                ,UserId
                ,Title
                ,Content
                ,CreatedAt
                ,UpdatedAt
            FROM TutorialAppSchema.Posts
            WHERE UserId = '" + User.FindFirst("userId")?.Value + "'";
        IEnumerable<Post> posts = _data.LoadData<Post>(sql);
        return posts;
    }

    [HttpPost]
    public IActionResult AddPost(PostToAddDto postToAdd)
    {
        string sql = @"
            INSERT INTO TutorialAppSchema.Posts (
                UserId,
                Title,
                Content,
                CreatedAt,
                UpdatedAt
            ) VALUES (
                " + User.FindFirst("userId")?.Value + @"
                ,'" + postToAdd.Title.Replace("'", "''") + @"'
                ,'" + postToAdd.Content.Replace("'", "''") + @"'
                , GETDATE()
                , GETDATE()
            )";
        if (_data.ExecuteSql(sql)) return Ok();
        return BadRequest("Failed to create post");
    }

    [HttpPut]
    public IActionResult EditPost(PostToEditDto postToAdd)
    {
        string sql = @"
            UPDATE TutorialAppSchema.Posts
            SET
                Title = '" + postToAdd.Title.Replace("'", "''") + @"'
                ,Content = '" + postToAdd.Content.Replace("'", "''") + @"'
                ,UpdatedAt = GETDATE()
            WHERE Id = " + postToAdd.Id.ToString() + @"
            AND UserId = " + User.FindFirst("userId")?.Value;
        if (_data.ExecuteSql(sql)) return Ok();
        return BadRequest("Failed to edit post");
    }

    [HttpDelete]
    public IActionResult DeletePost(int postId)
    {
        string sql = @"
            DELETE FROM TutorialAppSchema.Posts
            WHERE Id = " + postId.ToString() + @"
            AND UserId = " + User.FindFirst("userId")?.Value;
        if (_data.ExecuteSql(sql)) return Ok();
        return BadRequest("Failed to delete post");
    }
}