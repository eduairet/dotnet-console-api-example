namespace HelloWorld.Models
{
    public class User
    {
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