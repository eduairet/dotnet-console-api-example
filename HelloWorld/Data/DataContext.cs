using HelloWorld.Models;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld.Data
{
    public class DataContextDapper
    {
        private readonly string _connectionString = string.Join("", new List<string>() {
                "Server=localhost;",
                "Database=DotNetCourseDatabase;",
                "TrustServerCertificate=true;",
                "Trusted_Connection=true;" // Windows Authentication
        });

        // <T> stands for generic type
        public IEnumerable<T> LoadData<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.Query<T>(sql);
        }

        public T LoadDataSingle<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.QuerySingle<T>(sql);
        }

        public bool ExecuteSql(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.Execute(sql) > 0;
        }

        public int ExecuteSqlWithRowCount(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.Execute(sql);
        }
    }

    public class DataContextEntity : DbContext
    {
        /*
         * Entity Framework identify which tables match with our models
         * we just need to create a DbSet for each model,
         * we might need to make it nullable (just in case they exist)
         */
        public DbSet<User>? Users { get; set; }

        private readonly string _connectionString = string.Join("", new List<string>() {
                "Server=localhost;",
                "Database=DotNetCourseDatabase;",
                "TrustServerCertificate=true;",
                "Trusted_Connection=true;" // Windows Authentication
        });

        // This is where we create our model
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    _connectionString,
                    optionsBuilder => optionsBuilder.EnableRetryOnFailure()
                );
            }
        }

        // Here is where we map it
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("UserSchema"); // Just use when you need a specific schema
            modelBuilder.Entity<User>().HasKey(u => u.UserId); // You can use this approach as well .ToTable("Users", "UserSchema");
        }
    }
}