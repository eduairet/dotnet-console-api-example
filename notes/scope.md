# Scope

-   Scope allows to have variables with the same name in different levels of our code

    ```CSHARP
    string someString = "Top level scope!";
    Console.WriteLine(someString); // Top level scope!
    void stringFunction(string someString)
    {
        // someString will be scoped to this function
        Console.WriteLine(someString);
    }
    void stringFunctionNoArgs()
    {
        // someString will be taken from the top level scope
        Console.WriteLine(someString);
        string someString = "This will break the connection with our top level scope and cause an error";
    }
    stringFunction("Inner scope!"); // Inner scope!
    stringFunctionNoArgs(); // Top level scope!
    ```

-   In order to avoid bugs it's always better to name the variables different and according to the scoped block they live in
-   Some roundabouts about naming variables according to the scope are the following
    ```CSHARP
    string SomeString = ""; // Pascal case for top level variables
    string someString = ""; // Camel case for nested scope variables
    ```
