# Data Structures

-   Fixed size arrays

    ```CSHARP
    string[] names = new string[3]; // Array created with empty values
    names[0] = "John";
    names[1] = "Edu";
    Console.WriteLine(names[0]); // John
    Console.WriteLine(names[1]); // Edu
    Console.WriteLine(names[2]); // It will print an empty space
    Console.WriteLine(names[3]); // It will return an error
    // Unhandled exception. System.IndexOutOfRangeException: Index was outside the bounds of the array.
    string[] fruits = {"Apple", "Banana"}; // Array created with values
    Console.WriteLine(fruits[0]); // Apple
    ```

-   Dynamic size arrays are `List`s

    ```CSHARP
    List<string> vegetables = new List<string>(); // No values
    List<string> vegetables = new List<string>() { "Carrot" }; // Values
    // Or in a simplified way
    List<string> vegetables = new() { "Carrot", "Cucumber" };
    Console.WriteLine(vegetables[0]); // Carrot
    vegetables.Add("Onion"); // With lists we can add any element
    Console.WriteLine(vegetables[2]); // Onion
    ```

-   `IEnumerable`s are faster than lists and arrays and are useful if we need enumerate the collections

    -   Don't have an index
    -   Are constructed from lists
    -   Has extra built-in functionality

        ```CSHARP
        IEnumerable<string> ieVegetables = vegetables;
        Console.WriteLine(ieVegetables.First()); // Carrot
        Console.WriteLine(ieVegetables.ElementAt(1)); // Cucumber
        ```

-   Multidimensional arrays

    ```CSHARP
    // Two dimensions
    string[,] coords = {
        { "1", "2" },
        { "3", "4" },
        { "5", "6" },
    };
    Console.WriteLine(coords[0, 0]); // 1
    ```

-   `Dictionary` is a structure that has keys instead indexes

    ```CSHARP
    Dictionary<string, string> students = new() {
        { "John", "A" },
        { "Edu", "B" },
        { "Lisa", "C" },
        { "Laura", "D" },
        { "Luke", "E" }
    };
    // Values are accessed by their key
    Console.WriteLine(students["John"]); // A

    // You can nest data structures inside a dictionary
    Dictionary<string, string[]> tasks = new() {
        { "John", new string[] { "Do homework", "Take a shower" } },
        { "Edu", new string[] { "Cook a meal", "Watch a movie" } }
    };
    Console.WriteLine(tasks["John"][0]); // Do homework
    ```
