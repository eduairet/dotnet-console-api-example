# Operators and Conditionals

-   Number operations

    -   The order of operations in C# is Parentheses, Exponents, Multiplication and Division (left to right), and Addition and Subtraction (left to right).

        ```CSHARP
        // Updating one value
        int myInt = 5;
        Console.WriteLine(myInt); // 5
        myInt++; // myInt = myInt + 1;
        Console.WriteLine(myInt); // 6
        myInt += 5; // myInt = myInt + 5;
        Console.WriteLine(myInt); // 11
        myInt -= 6; // myInt = myInt - 6;
        Console.WriteLine(myInt); // 5
        // Using different variables to get a result
        int mySecondInt = 10;
        Console.WriteLine(myInt + mySecondInt); // 15
        Console.WriteLine(myInt - mySecondInt); // -5
        Console.WriteLine(myInt * mySecondInt); // 50
        Console.WriteLine(mySecondInt / myInt); // 2
        // More complex operations
        Console.WriteLine(Math.Pow(myInt, mySecondInt)); // 9765625 - Power of
        Console.WriteLine(Math.Sqrt(9)); // 3 - square root
        ```

-   String operations

    -   Some operators can be used with strings as well

        ```CSHARP
        string myString = "Hello";
        myString += " World";
        Console.WriteLine(myString); // Hello World
        // Scaped characters have a backslash \ before the actual character
        myString += "\"\n";
        Console.WriteLine(myString); // Notice \n starts a new line
        /*
        Hello World"

        */
        ```

    -   Built-in string functions
        ```CSHARP
        myString = myString.Remove(myString.Length - 1); // Remove the last character
        string[] myStringArr = myString.Split(' '); // Create an array from a string
        Console.WriteLine(myStringArr[0]); // Hello
        ```

-   Comparison

    -   There are several ways to compare values, either with operators or built in methods

        ```CSHARP
        int myAge = 21;
        int myMomAge = 42;
        // Methods
        Console.WriteLine(myAge.Equals(myMomAge)); // False
        Console.WriteLine(myAge.Equals(myMomAge / 2)); // True
        // Operators
        Console.WriteLine(myAge == myMomAge / 2); // True
        Console.WriteLine(myAge != myMomAge / 2); // False
        Console.WriteLine(myAge > myMomAge / 2); // False
        Console.WriteLine(myAge >= myMomAge / 2); // True
        Console.WriteLine(myAge < myMomAge / 2); // False
        Console.WriteLine(myAge <= myMomAge / 2); // True
        // Multiple pieces of logic
        // And && operator
        Console.WriteLine(0 == 0 && 0 != 1); // True
        Console.WriteLine(0 == 0 && 0 == 1); // False
        // Or || operator
        Console.WriteLine(0 == 0 || 0 == 1); // True
        Console.WriteLine(0 != 0 || 0 == 1); // False
        ```
