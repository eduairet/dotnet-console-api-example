using HelloWorld.Data;
using HelloWorld.Models;

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
            DataContext dataContext = new();
            string sqlCommand = "SELECT GETDATE()";
            DateTime rightNow = dataContext.LoadDataSingle<DateTime>(sqlCommand);
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
            int result = dataContext.ExecuteSqlWithRowCount(sql);
            Console.WriteLine(result);

            string sqlQuerySelect = @"
                SELECT _user.Username,
                       _user.FullName,
                       _user.IsActive
                  FROM UserSchema._user";
            IEnumerable<User> results = dataContext.LoadData<User>(sqlQuerySelect);
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
