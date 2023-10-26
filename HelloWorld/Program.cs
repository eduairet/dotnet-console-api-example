using System.Data;
using Dapper;
using HelloWorld.Models;
using Microsoft.Data.SqlClient;


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
            var connectionOptions = new List<string>()
            {
                "Server=localhost;",
                "Database=DotNetCourseDatabase;",
                "TrustServerCertificate=true;",
                "Trusted_Connection=true;" // Windows Authentication
            };
            string connectionString = string.Join("", connectionOptions);
            IDbConnection dbConnection = new SqlConnection(connectionString);
            string sqlCommand = "SELECT GETDATE()";
            DateTime rightNow = dbConnection.QuerySingle<DateTime>(sqlCommand);
            Console.WriteLine(rightNow); // Today's date

            User newUser = new()
            {
                Username = "XXXX",
                FullName = "User User",
                IsActive = true,
            };

            string sql = @"
                INSERT INTO UserSchema._user (
                    Username, FullName, IsActive
                ) VALUES (" + SingleQuotes(newUser.Username) +
                    "," + SingleQuotes(newUser.FullName) +
                    "," + SingleQuotes(newUser.IsActive.ToString()) +
                ")";
            int result = dbConnection.Execute(sql);
            Console.WriteLine(result);

            string sqlQuerySelect = @"
                SELECT _user.Username,
                       _user.FullName,
                       _user.IsActive
                  FROM UserSchema._user";
            IEnumerable<User> results = dbConnection.Query<User>(sqlQuerySelect);
            foreach (User user in results)
            {
                Console.WriteLine(string.Format(
                    "Username: {0}\nFullName: {1}\nIsActive: {2}",
                    user.Username,
                    user.FullName,
                    user.IsActive
                ));
            }
        }
    }
}
