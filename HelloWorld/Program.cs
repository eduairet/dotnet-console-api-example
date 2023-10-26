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
            DataContextDapper dataContextDapper = new();
            string sqlCommand = "SELECT GETDATE()";
            DateTime rightNow = dataContextDapper.LoadDataSingle<DateTime>(sqlCommand);
            Console.WriteLine(rightNow); // Today's date

            User newUser = new()
            {
                Username = "user1",
                FullName = "User1 User1",
                IsActive = true,
            };

            string sql = @"
                INSERT INTO UserSchema.Users (
                    Username, FullName, IsActive
                ) VALUES (" + SingleQuotes(newUser.Username) +
                    "," + SingleQuotes(newUser.FullName) +
                    "," + SingleQuotes(newUser.IsActive.ToString()) +
                ")";
            int result = dataContextDapper.ExecuteSqlWithRowCount(sql);
            Console.WriteLine(result);

            string sqlQuerySelect = @"
                SELECT Users.Username,
                       Users.FullName,
                       Users.IsActive
                  FROM UserSchema.Users";
            IEnumerable<User> results = dataContextDapper.LoadData<User>(sqlQuerySelect);
            foreach (User user in results)
            {
                Console.WriteLine(string.Format(
                    "UserId: {0} Username: {1} FullName: {2} IsActive: {3}",
                    user.UserId,
                    user.Username,
                    user.FullName,
                    user.IsActive
                ));
            }

            User newUser2 = new()
            {
                Username = "user2",
                FullName = "User2 User2",
                IsActive = true,
            };

            DataContextEntity dataContextEntity = new();
            dataContextEntity.Add(newUser2);
            dataContextEntity.SaveChanges();
            List<User>? users = dataContextEntity.Users?.ToList<User>();
            if (users != null)
            {
                users?.ForEach(user => Console.WriteLine(string.Format(
                        "UserId: {0} Username: {1} FullName: {2} IsActive: {3}",
                        user.UserId,
                        user.Username,
                        user.FullName,
                        user.IsActive
                    )));
            }

        }
    }
}
