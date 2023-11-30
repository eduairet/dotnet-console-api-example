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
    -   We also add a `PasswordSalt` to the user's password which is a random string that is added to the password before hashing it to make it more secure
    -   We'll need a password key to hash the password, we can use the `appsettings.json` file to store it or a package like `dotenv.net`
        ```JSON
        {
            "AppSettings": {
                "PasswordKey": "RANDOM_LARGE_STRING_HERE"
            }
        }
        ```
    -   The `PasswordKey` will be used to hash the password and the `PasswordSalt` will be used to add a random string to the password before hashing it

        ```CSHARP
        private byte[] CreatePasswordHash(string password, byte[] passwordSalt)
        {
            // Combine the password key and password salt into a single string
            string passwordSaltPlusString = PasswordKey + Convert.ToBase64String(passwordSalt);

            // Convert the combined string into a byte array
            byte[] passwordSaltPlusBytes = Encoding.ASCII.GetBytes(passwordSaltPlusString);

            // Create a salted hash of the password using the PBKDF2 algorithm
            byte[] passwordHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: passwordSaltPlusBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            );

            // Return the generated password hash
            return passwordHash;
        }
        ```

    -   JWT (JSON Web Token)

        -   It's a standard that defines a way for securely transmitting information between parties as a JSON object
        -   To configure JWT in our API we need to add the TokenKey to the appsettings.json file
            ```JSON
            {
                "AppSettings": {
                    "TokenKey": "RANDOM_LARGE_STRING_HERE"
                }
            }
            ```
        -   Then use the `TokenKey` to create a token

            ```CSHARP
            private string CreateToken(int userId)
            {
                // Create the claims for the token
                Claim[] claims =
                [
                    new("userId", userId.ToString())
                ];

                // Create a key from the token key, we need to convert it to a byte array
                SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(TokenKey));

                // Create the credentials for the token using the key and the algorithm to sign it
                SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256Signature);


                SecurityTokenDescriptor tokenDescriptor = new()
                {
                    Subject = new ClaimsIdentity(claims), // Claims for the token
                    SigningCredentials = credentials, // Credentials for the token
                    Expires = DateTime.Now.AddDays(7) // Token expires in 7 days
                };

                // Create the token handler and create the token using the token descriptor
                JwtSecurityTokenHandler tokenHandler = new();
                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            ```

        -   We can verify the token on the https://jwt.io/ debugger and get the following information
            ```JSON
            // Header
            {
                "alg": "HS256",
                "typ": "JWT"
            }
            // Payload
            {
                "unique_name": "1",
                "nbf": 1627776000,
                "exp": 1628380800,
                "iat": 1627776000
            }
            ```

-   Refactor reusable code
-   Related data
