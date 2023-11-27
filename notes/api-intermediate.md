# API Intermediate

-   Repository flow

    -   Abstracts the data access layer
    -   Encapsulates the logic for retrieving and manipulating data
    -   Makes the code more readable
    -   It's usually added in the `Data` folder

        ```CSHARP
        // Program.cs
        // Allows the use of the IConfiguration interface
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        ```

        ```CSHARP
        // IUserRepository.cs
        // Interface for the UserRepository class
        namespace DotnetAPI.Data;

        public interface IUserRepository
        {
            bool SaveChanges();
            void AddEntity<T>(T entity);
            void RemoveEntity<T>(T entity);
        }
        ```

        ```CSHARP
        // UserRepository.cs
        // Class that implements the IUserRepository interface
        namespace DotnetAPI.Data;

        public class UserRepository(IConfiguration config) : IUserRepository
        {
            private readonly DataContextEF _data = new(config);

            public bool SaveChanges() => _data.SaveChanges() > 0;

            public void AddEntity<T>(T entity)
            {
                if (entity != null) _data.Add(entity);
            }

            public void RemoveEntity<T>(T entity)
            {
                if (entity != null) _data.Remove(entity);
            }
        }
        ```

    -   This approach will help us to keep the controllers clean and readable since all the data access logic will be in the repository class

-   Dependency Injection
-   Authentication
    -   It holds the logic for the authentication in the API
    -   We save the user's password (prevention in case a hacker access the DB) in the database as a hash
    - We also add a PasswordSalt to the user's password which is a random string that is added to the password before hashing it to make it more secure
    -   UserId from token
-   Refactor reusable code
-   Related data
