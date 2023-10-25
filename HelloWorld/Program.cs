using System.Data;
using Dapper;
using HelloWorld.Models;
using Microsoft.Data.SqlClient;


namespace HelloWorld
{
    public class Program
    {
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
            newUser.SetPassword("XXXXXXXX");
            Console.WriteLine(newUser.Username); // XXXX
            Console.WriteLine(newUser.FullName); // User User
            Console.WriteLine(newUser.IsActive); // True
                                                 // newUser._password is not accessible outside the class.
            newUser.Username = "Lalo"; // Here we use the setter to change the value.
            Console.WriteLine(newUser.Username); // Lalo
        }
    }
}
