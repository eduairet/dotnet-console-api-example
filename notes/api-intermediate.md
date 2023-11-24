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

-   Dependency Injection
-   Authentication
    -   UserId from token
-   Refactor reusable code
-   Related data
