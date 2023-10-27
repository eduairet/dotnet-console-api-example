# Model Mapping

-   AutoMapper allows us to map values into our desired keys
    ```SHELL
    dotnet add package AutoMapper
    ```
    ```CSHARP
    Mapper mapper = new(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserSnake, User>()
                .ForMember(
                    dest => dest.IsActive,
                    options => options.MapFrom(src => src.is_active)
                )
                .ForMember(
                    dest => dest.FullName,
                    options => options.MapFrom(src => src.full_name)
                )
                .ForMember(
                    dest => dest.Username,
                    options => options.MapFrom(src => src.username)
                );
        }
    ));
    ```
-   We can point to the JSON key into our model to make it easier to map
    ```CSHARP
    // Model
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

    // Program
    // Maps fields automatically
    IEnumerable<User>? dummyUsers = JsonConvert.DeserializeObject<IEnumerable<User>>(dummyUsersJson);
    if (dummyUsers != null)
    {
        foreach (User dummyUser in dummyUsers)
        {
            Console.WriteLine(dummyUser.Username);
        }
    }
    ```
