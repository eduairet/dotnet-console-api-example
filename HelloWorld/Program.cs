using AutoMapper;
using HelloWorld.Data;
using HelloWorld.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HelloWorld
{
    public class Program
    {
        private static string SingleQuotes(string raw)
        {
            return "'" + raw.Replace("'", "''") + "'"; // Replacement handles single quotes in a Database
        }
        public static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build(); // Loads appsettings.json and returns the configuration with .Build()

            DataContextDapper dataContextDapper = new(config);

            /*
            User newUser = new()
            {
                Username = "user1",
                FullName = "User1 User1",
                IsActive = true,
            };

            string sqlDir = "Data/sql-scripts/";

            string sqlInsertUser = @"
                INSERT INTO UserSchema.Users (
                    Username, FullName, IsActive
                ) VALUES (" + SingleQuotes(newUser.Username) +
                    "," + SingleQuotes(newUser.FullName) +
                    "," + SingleQuotes(newUser.IsActive.ToString()) +
                ")";

            File.WriteAllText(sqlDir + "insert-user.sql", sqlInsertUser);

            string sqlQuerySelect = @"
                SELECT Users.UserId, 
                       Users.Username,
                       Users.FullName,
                       Users.IsActive
                  FROM UserSchema.Users";

            File.WriteAllText(sqlDir + "select-users.sql", sqlQuerySelect);

            string logFilePath = "log.txt";
            using StreamWriter writer = new(logFilePath, append: true);
            writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            writer.Close();

            string logContent = File.ReadAllText(logFilePath);
            Console.WriteLine(logContent);

            string jsonPath = "Data/json/";

            string rugbyPlayersJson = File.ReadAllText(jsonPath + "rugby-players.json");

            IEnumerable<User>? rugbyUsers = JsonConvert.DeserializeObject<IEnumerable<User>>(
                rugbyPlayersJson
            );

            if (rugbyUsers != null)
            {
                foreach (User rugbyUser in rugbyUsers)
                {
                    string sqlInsertUser = @"
                        INSERT INTO UserSchema.Users (
                            Username, FullName, IsActive
                        ) VALUES (" + SingleQuotes(rugbyUser.Username) +
                            "," + SingleQuotes(rugbyUser.FullName) +
                            "," + SingleQuotes(rugbyUser.IsActive.ToString()) +
                        ")";
                    dataContextDapper.ExecuteSql(sqlInsertUser);
                }
            }

            // Settings that'll help to convert to camelCase when serializing
            JsonSerializerSettings settings = new()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            string rugbyUsersCopy = JsonConvert.SerializeObject(rugbyUsers, settings);
            File.WriteAllText(jsonPath + "rugby-players-copy.json", rugbyUsersCopy);
            */

            string jsonPath = "Data/json/";
            string dummyUsersJson = File.ReadAllText(jsonPath + "dummy-users.json");

            /*
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

            JsonSerializerSettings settings = new()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            IEnumerable<UserSnake>? dummyUsersCopy = JsonConvert.DeserializeObject<IEnumerable<UserSnake>>(dummyUsersJson);
            if (dummyUsersCopy != null)
            {
                IEnumerable<User>? dummyUsers = mapper.Map<IEnumerable<User>>(dummyUsersCopy);
                foreach (User dummyUser in dummyUsers)
                {
                    Console.WriteLine(dummyUser.Username);
                }
            }
            */

            IEnumerable<User>? dummyUsers = JsonConvert.DeserializeObject<IEnumerable<User>>(dummyUsersJson);
            if (dummyUsers != null)
            {
                foreach (User dummyUser in dummyUsers)
                {
                    Console.WriteLine(dummyUser.Username);
                }
            }
        }
    }
}
