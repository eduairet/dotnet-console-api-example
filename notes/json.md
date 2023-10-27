# JSON

-   We can read JSON files the same way we read another text files, but in order to use them as an object we need to deserialize them, for this we can use several tools

    -   Built in `JsonSerializer`

        ```CSHARP
        string rugbyPlayersJson = File.ReadAllText("Data/json/rugby-players.json");

        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        IEnumerable<User>? rugbyUsers = JsonSerializer.Deserialize<IEnumerable<User>>(
            rugbyPlayersJson,
            options
        );
        ```

    -   Newtonsoft (more powerful)

        ```SHELL
        dotnet add package Newtonsoft.Json
        ```

        ```CSHARP
        string rugbyPlayersJson = File.ReadAllText("Data/json/rugby-players.json");

        IEnumerable<User>? rugbyUsers = JsonConvert.DeserializeObject<IEnumerable<User>>(
            rugbyPlayersJson
        );
        ```

-   Serialization will allow us to create JSON files from objects

    ```CSHARP
    // System
    string rugbyUsersSystem = System.Text.Json.JsonSerializer.Serialize(rugbyPlayersJson);

    // Newtonsoft
    JsonSerializerSettings settings = new()
    {
        // This will help to convert to camelCase when serializing
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };
    string rugbyUsersCopy = JsonConvert.SerializeObject(rugbyUsers, settings);
    File.WriteAllText(jsonPath + "rugby-players-copy.json", rugbyUsersCopy);
    ```
