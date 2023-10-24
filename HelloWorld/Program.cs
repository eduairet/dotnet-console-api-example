using HelloWorld.Models;

internal class Program
{
    static void Main(string[] args)
    {
        User newUser = new()
        {
            Username = "XXXX",
            FullName = "User User",
            IsActive = true,
        };
        newUser.SetPassword("XXXXXXXX");
        Console.WriteLine(newUser.Username); // XXXX
        Console.WriteLine(newUser.FullName); // User User
        Console.WriteLine(newUser.IsActive); // True
        // newUser._password is not accessible outside the class.
        newUser.Username = "Lalo"; // Here we use the setter to change the value.
        Console.WriteLine(newUser.Username); // Lalo
    }
}
