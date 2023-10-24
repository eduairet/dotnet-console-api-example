# Namespaces

-   Namespaces allow us to split our logic into multiple files

    -   This will help us to follow architectural patterns like MVC
    -   Example

        ```CSHARP
        // /Models/Computer.cs
        namespace HelloWorld.Models // <Project name>.<Folder name>
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
        ```

        ```CSHARP
        // /Program.cs
        using HelloWorld.Models; // Imported from /Models/Computer.cs

        internal class Program
        {
            static void Main(string[] args)
            {
                User newUser = new()
                {
                    Username = "XXXX",
                    FullName = "User User",
                    IsActive = true,
                };
            }
        }
        ```
