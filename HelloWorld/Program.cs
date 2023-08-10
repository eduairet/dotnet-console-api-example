int myInt = 5;
Console.WriteLine(myInt); // 5
myInt++; // myInt = myInt + 1;
Console.WriteLine(myInt); // 6
myInt += 5; // myInt = myInt + 5;
Console.WriteLine(myInt); // 11
myInt -= 6; // myInt = myInt - 6;
Console.WriteLine(myInt); // 5

int mySecondInt = 10;
Console.WriteLine(myInt + mySecondInt); // 15
Console.WriteLine(myInt - mySecondInt); // -5
Console.WriteLine(myInt * mySecondInt); // 50
Console.WriteLine(mySecondInt / myInt); // 2

Console.WriteLine(Math.Pow(myInt, mySecondInt)); // 9765625
Console.WriteLine(Math.Sqrt(9)); // 3

string myString = "Hello";
myString += " World";
Console.WriteLine(myString); // Hello World
myString += "\"\n";
Console.WriteLine(myString); // Notice \n starts a new line
/*
Hello World"

*/
myString = myString.Remove(myString.Length - 1); // Remove the last character
string[] myStringArr = myString.Split(' ');
Console.WriteLine(myStringArr[0]); // Hello

int myAge = 21;
int myMomAge = 42;
Console.WriteLine(myAge.Equals(myMomAge)); // False
Console.WriteLine(myAge.Equals(myMomAge / 2)); // True
Console.WriteLine(myAge == myMomAge / 2); // True
Console.WriteLine(myAge != myMomAge / 2); // False
Console.WriteLine(myAge > myMomAge / 2); // False
Console.WriteLine(myAge >= myMomAge / 2); // True
Console.WriteLine(myAge < myMomAge / 2); // False
Console.WriteLine(myAge <= myMomAge / 2); // True

Console.WriteLine(0 == 0 && 0 != 1); // True
Console.WriteLine(0 == 0 && 0 == 1); // False
Console.WriteLine(0 == 0 || 0 == 1); // True
Console.WriteLine(0 != 0 || 0 == 1); // False
