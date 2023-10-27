using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HelloWorld.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
        [JsonPropertyName("username")]
        public required string Username { get; set; }
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }
        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }
        private string? _password;
        public void SetPassword(string password)
        {
            _password = password;
        }
        public User()
        {
            Username ??= "";
            FullName ??= "";
        }
    }

    public class UserSnake
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; }
        public required string username { get; set; }
        public string full_name { get; set; }
        public bool is_active { get; set; }
        private string? _password;
        public void SetPassword(string password)
        {
            _password = password;
        }
        public UserSnake()
        {
            username ??= "";
            full_name ??= "";
        }
    }
}