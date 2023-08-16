int myInt = 5;
int myInt2 = 6;

if (myInt < myInt2)
{
    Console.WriteLine("myInt is lower than myInt2");
}

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