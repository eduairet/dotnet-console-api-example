using DotnetAPI.Models;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data;

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
        DynamicParameters sqlParameters = new();
        if (userId != null && userId > 0)
        {
            sql += " @UserId = @UserIDParam,";
            sqlParameters.Add("@UserIDParam", userId, DbType.Int64);
        }
        if (postId != null && postId > 0)
        {
            sql += " @PostId = @PostIdParam,";
            sqlParameters.Add("@PostIdParam", postId, DbType.Int64);
        }
        if (searchValue != null)
        {
            sql += " @SearchValue = @SearchParam,";
            sqlParameters.Add("@SearchParam", searchValue, DbType.String);
        }
        sql = sql.TrimEnd(',');
        IEnumerable<Post> posts = _data.LoadDataWithParams<Post>(sql, sqlParameters);
        return posts;
    }

    [HttpGet("my-posts")]
    public IEnumerable<Post> MyPosts()
    {
        string sql = "EXEC TutorialAppSchema.spPosts_Get @UserId = @UserIDParam";
        DynamicParameters sqlParameters = new();
        sqlParameters.Add("@UserIDParam", User.FindFirst("userId")?.Value, DbType.Int64);
        IEnumerable<Post> posts = _data.LoadDataWithParams<Post>(sql, sqlParameters);
        return posts;
    }


    [HttpPut]
    public IActionResult Upsert(PostUpsertDto post)
    {
        string sql = @"EXEC TutorialAppSchema.spPosts_Upsert
              @UserId = @UserIdParam
            , @Title = @TitleParam
            , @Content = @ContentParam";
        DynamicParameters sqlParameters = new();
        sqlParameters.Add("@UserIdParam", User.FindFirst("userId")?.Value, DbType.Int64);
        sqlParameters.Add("@TitleParam", post.Title, DbType.String);
        sqlParameters.Add("@ContentParam", post.Content, DbType.String);
        if (post.Id != null && post.Id > 0)
        {
            sql += $"\n, @PostId = @PostIdParam";
            sqlParameters.Add("@PostIdParam", post.Id, DbType.Int64);
        }
        if (_data.ExecuteSqlWithParameters(sql, sqlParameters)) return Ok();
        return BadRequest("Failed to create post");
    }

    [HttpDelete]
    public IActionResult DeletePost(int postId)
    {
        string sql = $"EXEC TutorialAppSchema.spPosts_Delete @UserId = @UserIDParam, @PostId = @PostIdParam";
        DynamicParameters sqlParameters = new();
        sqlParameters.Add("@UserIDParam", User.FindFirst("userId")?.Value, DbType.Int64);
        sqlParameters.Add("@PostIdParam", postId, DbType.Int64);
        if (_data.ExecuteSqlWithParameters(sql, sqlParameters)) return Ok();
        return BadRequest("Failed to delete post");
    }
}