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
