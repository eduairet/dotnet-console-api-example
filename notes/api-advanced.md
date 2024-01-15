# API Advance

-   With the help of stored procedures we can optimize our endpoints and make them dynamic

    ```CS
    [HttpGet()] // This covers 4 different endpoints, get-users, get-user, get-user(s)-active, get-user(s)-inactive
    public IEnumerable<UserComplete> GetUsers(int? userId, bool? isActive)
    {
        string sql = "EXEC TutorialAppSchema.spUsers_Get";
        if (userId != null && userId > 0) sql += $" @UserID = {userId},";
        if (isActive != null) sql += $" @Active = {((bool)isActive ? 1 : 0)},";
        sql = sql.TrimEnd(',');
        IEnumerable<UserComplete> users = _data.LoadData<UserComplete>(sql);
        return users;
    }
    ```

-   In order to improve the security of our dapper endpoints we need to use parameterized queries

    -   These are used to prevent SQL injection attacks
    -   Types of parameterized queries

        -   `List<SqlParameter>` can be used in certain cases
            ```C#
            string sqlAddAuth = @$"EXEC TutorialAppSchema.spAuth_Upsert @Email = @EmailParam,
            @PasswordHash = @PasswordHashParam,
            @PasswordSalt = @PasswordSaltParam";
            List<SqlParameter> sqlParameters = [];
            SqlParameter emailParameter = new("@EmailParam", SqlDbType.VarChar) { Value = email };
            sqlParameters.Add(emailParameter);
            SqlParameter passwordSaltParameter = new("@PasswordHashParam", SqlDbType.VarBinary) { Value = passwordHash };
            sqlParameters.Add(passwordSaltParameter);
            SqlParameter passwordHashParameter = new("@PasswordSaltParam", SqlDbType.VarBinary) { Value = passwordSalt };
            sqlParameters.Add(passwordHashParameter);
            return _data.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters);
            ```
            -   Execute with parameters function:
                ```C#
                public bool ExecuteSqlWithParameters(string sql, List<SqlParameter> sqlParameters)
                {
                    SqlCommand sqlCommand = new(sql);
                    foreach (var sqlParameter in sqlParameters)
                    {
                        sqlCommand.Parameters.Add(sqlParameter);
                    }
                    SqlConnection dbConnection = new(_connectionString);
                    dbConnection.Open();
                    sqlCommand.Connection = dbConnection;
                    int rowsAffected = sqlCommand.ExecuteNonQuery();
                    dbConnection.Close();
                    return rowsAffected > 0;
                }
                ```
        -   `DynamicParameters` are a more clean way to use parameterized queries and they're cleaner
            ```C#
            string sql = $"EXEC TutorialAppSchema.spAuth_Get @Email = @EmailParam";
            DynamicParameters sqlParameters = new(); // Wee need to use DynamicParameters for single row results
            sqlParameters.Add("@EmailParam", email, DbType.String);
            return _data.LoadDataSingleWithParams<UserForLoginConfirmationDto>(sql, sqlParameters);
            ```
