using HelloWorld.Data;
using HelloWorld.Models;
using Microsoft.Extensions.Configuration;

namespace HelloWorld
{
    public class Program
    {
        private static string SingleQuotes(string raw)
        {
            return "'" + raw + "'";
        }
        public static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build(); // Loads appsettings.json and returns the configuration with .Build()

            DataContextDapper dataContextDapper = new(config);

            User newUser = new()
            {
                Username = "user1",
                FullName = "User1 User1",
                IsActive = true,
            };

            string sqlDir = "sql-scripts/";

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
        }
    }
}
