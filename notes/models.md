# Models

-   They are useful to map the data on our applications

    ```CSHARP
    using HelloWorld;

    namespace HelloWorld
    {
        public class User
        {
            // Public keyword makes the property visible to other classes.
            // Required keyword makes the property required to have a value.
            // With get and set keywords, we can access the property.
            // { get; set; } = { get => _password; set => _password = value; }
            public required string Username { get; set; }
            // string? makes the property nullable.
            public string FullName { get; set; }
            public bool IsActive { get; set; }
            // Private keyword makes the property not visible to other classes.
            // It's a good practice to ghost the name of the variable in these cases.
            private string? _password; // Adding ; creates a field inside classes
            // Never use public variables as fields.
            public void SetPassword(string password)
            {
                _password = password;
            }
            // Constructor
            public User()
            {
                /*
                ?? avoids nullable string warning,
                an if statement can be used as well,
                or even by setting the default value to the property
                outside of the constructor with something like
                public string FullName { get; set; }
                */
                Username ??= "";
                FullName ??= "";
            }
        }
    }

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
            newUser.SetPassword("XXXXXXXX");
            Console.WriteLine(newUser.Username); // XXXX
            Console.WriteLine(newUser.FullName); // User User
            Console.WriteLine(newUser.IsActive); // True
            // newUser._password is not accessible outside the class.
            newUser.Username = "Lalo"; // Here we use the setter to change the value.
            Console.WriteLine(newUser.Username); // Lalo
        }
    }
    ```
