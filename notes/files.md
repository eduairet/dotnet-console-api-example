# File Read and Write

-   Writing a file

    ```CSHARP
    string sqlDir = "sql-scripts/";

    string sqlInsertUser = @"
        INSERT INTO UserSchema.Users (
            Username, FullName, IsActive
        ) VALUES (" + SingleQuotes(newUser.Username) +
            "," + SingleQuotes(newUser.FullName) +
            "," + SingleQuotes(newUser.IsActive.ToString()) +
        ")";

    File.WriteAllText(sqlDir + "insert-user.sql", sqlInsertUser);
    ```

-   Append text to file

    ```CSHARP
    using StreamWriter writer = new("log.txt", append: true);
    writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    // Every time we run the program we'll append a line with the date time of the run
    writer.Close(); // Close it to allow more operations on the file
    ```

-   Read a file
    ```CSHARP
    File.ReadAllText("log.txt", en);
    ```
