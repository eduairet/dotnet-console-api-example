using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloWorld.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public required string Username { get; set; }
        public string FullName { get; set; }
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
}