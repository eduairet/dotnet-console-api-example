## Writing to the Console and C# Basic Types

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

    -   Numbers

        -   Bytes can be numbers in a range of 256 (16\*\*2) elements (8 bits)

            ```CSHARP
            // bit - 0 or 1 | 1 byte = 8 bits
            // 00000000 = 0 | 00000001 = 1 | 00000010 = 2 | 00000011 = 3
            // 00000100 = 4 | 00000101 = 5 | 00000110 = 6 | 00000111 = 7
            // 00001000 = 8 | and so on...

            // Signed: positive between 0 and 255
            byte myByte = 255;
            // Unsigned: positive and negative between -128 - 127
            sbyte mySByte = 127;
            sbyte mySecondSByte = -128;
            ```

        -   Shorts are 2 byte numbers in a range of 65,536 elements (16\*\*4)

            ```CSHARP
            // Signed: positive between 0 and 65535
            ushort myUShort = 65535;
            // Unsigned: positive and negative between -32768 and 32767
            short myShort = 32767;
            short mySecondShort = -32768;
            ```

        -   Integers are 4 byte numbers in a range of 4,294,967,296 elements (16\*\*8)

            ```CSHARP
            // Positive and negative between -2147483648 and 2147483647
            int myInt = 2147483647;
            int mySecondInt = -2147483648;
            ```

        -   Longs are 8 byte numbers in a range of 18,446,744,073,709,551,616 elements (16\*\*16)

            ```CSHARP
            // Positive and negative between -9223372036854775808 and 9223372036854775807
            long myLong = 9223372036854775807;
            long mySecondLong = -9223372036854775808;
            ```

        -   Floating point numbers

            ```CSHARP
            // float = 4 bytes
            float myFloat = 0.751f;
            // double = 8 bytes
            double myDouble = 0.751; // Without a letter is a double
            double mySecondDouble = 0.75d; // But you can add a d
            // decimal = 16 bytes
            decimal myDecimal = 0.751m; // Most precise for accurate math
            ```

            -   Most of the times we'll use decimal, using float or double might lead to unexpected behavior

                ```CSHARP
                Console.WriteLine(0.551f - 0.55f);
                // float 0.0009999871
                Console.WriteLine(0.551d - 0.55d); // or 0.551 - 0.55
                // double 0.0010000000000000009
                Console.WriteLine(0.551m - 0.55m);
                // decimal 0.001
                ```

    -   Strings

        ```CSHARP
        char myChar = 'A'; // Single characters in single quotes
        string myString = "AEio/1" // Double quotes for strings
        ```

    -   Booleans
        ```CSHARP
        bool cool = true;
        bool bad = false
        ```

-   As you may noticed, every variable comes defined with their type, that special word is called alias

    ```CSHARP
    // bool is the alias for System.Boolean
    // typeAlias name = value;
    bool myBool = true;
    System.Boolean myCopycatBool = true;
    // Both previous variables are the same
    ```

[^1]:
    Be aware that previous versions of .NET have these functions inside a class and they use a different syntax

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
