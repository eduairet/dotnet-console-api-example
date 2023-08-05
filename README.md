# .NET Console API

## Create a project

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

## C# Intro

-   Console write functions[^1]

    -   It writes data to the console

        ```CSHARP
        Console.WriteLine("Hello, World!");
        Console.Write("Hello, World!"); // No line break
        Console.Write(args[0]); // Argument 1 from the CLI

        // $ dotnet run dope
        // Hello, World!
        // Hello, World!dope
        ```

-   Variable types

    -   Bytes can be numbers in a range of 256 elements (8 bits)

        ```CSHARP
        // bit - 0 or 1
        // byte - 8 bytes
        // 00000000 = 0
        // 00000001 = 1
        // 00000010 = 2
        // 00000011 = 3
        // 00000100 = 4
        // 00000101 = 5
        // 00000110 = 6
        // 00000111 = 7
        // 00001000 = 8
        // and so on...
        ```

        -   Signed bytes (just positive between 0 - 255)
        -   Unsigned bytes (positive and negative between -128 - 127)

            ```CSHARP
            // Signed
            byte myByte = 255;
            byte mySecondByte = 0;
            sbyte mySbyte = 127;
            // Unsigned
            sbyte mySecondSbyte = -128;
            ```

[^1]:
    Notice that previous versions of .NET have these functions inside a class and they use a different syntax

    ```CSHARP
    using System; // Console comes from this package

    namespace MyApp // Note: actual namespace depends on the project name.
    {
        internal class Program
        {
            // args accepts a string array from the CLI
            static void Main(string[] args)
            {
                Console.WriteLine("Hello World!");
            }
        }
    }
    ```
