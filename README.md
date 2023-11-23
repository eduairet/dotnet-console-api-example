# .NET Console API

## Getting Started

-   First go to the directory where your project is going to be held `cd directory-of-your-project`
-   Create your project `dotnet new <API Type> --name or -n <Name of Your Project>`:
    ```SHELL
    dotnet new console -n HelloWorld
    ```
-   Check the structure of your project

    ```
    DOTNET-API-EXAMPLE
    │
    └───HelloWorld
        │   HelloWorld.csproj # What is in our project
        │   Program.cs # The actual code of our project
        │
        ├───bin
        └───obj
    ```

-   Run your project
    ```SHELL
    dotnet run # dotnet watch run > Runs when there's a change on the code
    # > Hello, World!
    ```
-   Check the app at the [HelloWorld directory](./HelloWorld/)

## Notes

1. [Writing to the Console and C# Basic Types](./notes/console-basic-types.md)
2. [Data Structures](./notes/data-structures.md)
3. [Operators and Comparison](./notes/operators-comparison.md)
4. [Conditional Statements](./notes/conditional-statements.md)
5. [Loops](./notes/loops.md)
6. [Methods](./notes/methods.md)
7. [Scope](./notes/scope.md)
8. [Models](./notes/models.md)
9. [Namespaces](./notes/namespaces.md)
10. [MS SQL Database Connection](./notes/sql-connection.md)
11. [Config](./notes/config.md)
12. [File Read and Write](./notes/files.md)
13. [JSON](./notes/json.md)
14. [Model Mapping](./notes/model-mapping.md)
15. [SQL](./notes/sql.md)
16. [API Basics](./notes/api-basics.md)
17. [API Intermediate](./notes/api-intermediate.md)
