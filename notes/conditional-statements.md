# Conditional Statements

-   `if / else if / else` executes a block of code if it satisfies an expression

    ```CSHARP
    string myString = "hello";
    string myStringUpper = "HELLO";

    if (myString == myStringUpper)
    {
        Console.WriteLine("They are equal");
    }
    else if (myString.ToUpper() == myStringUpper)
    {
        // Executes if first if statement is false
        Console.WriteLine("They're almost equal");
    }
    else
    {
        // Executes if every other if statement is false
        Console.WriteLine("They are not equal");
    }
    // Prints: They're almost equal
    ```

-   `switch` executes an action if there's a match on a value

    ```CSHARP
    // Value to match
    switch (myString.ToLower())
    {
        case "hello":
            Console.WriteLine("Nice to meet you!");
            break;

        case "goodbye":
            Console.WriteLine("See you soon!");
            break;

        default:
            Console.WriteLine("What?");
            break;
    }
    ```
