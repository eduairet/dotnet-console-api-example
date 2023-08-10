/*
// Basic types

// 1 byte is made up of 8 bits 00000000 - these bits can be used to store a number as follows
// Each bit can be worth 0 or 1 of the value it is placed in
// From the right we start with a value of 1 and double for each digit to the left
// 00000000 = 0 bits
// 00000001 = 1 bit
// 00000010 = 2 bits
// 00000011 = 3 bits
// 00000100 = 4 bits
// 00000101 = 5 bits
// 00000110 = 6 bits
// 00000111 = 7 bits
// 00001000 = 8 bits

// 1 byte (8 bit) unsigned, where signed means it can be negative
byte myByte = 255;
byte mySecondByte = 0;

// 1 byte (8 bit) signed, where signed means it can be negative
sbyte mySbyte = 127;
sbyte mySecondSbyte = -128;


// 2 byte (16 bit) unsigned, where signed means it can be negative
ushort myUshort = 65535;

// 2 byte (16 bit) signed, where signed means it can be negative
short myShort = -32768;

// 4 byte (32 bit) signed, where signed means it can be negative
int myInt = 2147483647;
int mySecondInt = -2147483648;

// 8 byte (64 bit) signed, where signed means it can be negative
long myLong = -9223372036854775808;


// 4 byte (32 bit) floating point number
float myFloat = 0.751f;
float mySecondFloat = 0.75f;

// 8 byte (64 bit) floating point number
double myDouble = 0.751;
double mySecondDouble = 0.75d;

// 16 byte (128 bit) floating point number
decimal myDecimal = 0.751m;
decimal mySecondDecimal = 0.75m;

Console.WriteLine(myFloat - mySecondFloat); // 0.0009999871
Console.WriteLine(myDouble - mySecondDouble); // 0.0010000000000000009
Console.WriteLine(myDecimal - mySecondDecimal); // 0.001

Console.WriteLine(0.551f - 0.55f); // 0.0009999871
Console.WriteLine(0.551 - 0.55); // 0.0010000000000000009
Console.WriteLine(0.551m - 0.55m); // 0.001

char myChar = 'A';
string myString = "Hello World";
Console.WriteLine(myString);
string myStringWithSymbols = "!@#$@^$%%^&(&%^*__)+%^@##$!@%123589071340698ughedfaoig137";
Console.WriteLine(myStringWithSymbols);

bool myBool = true;
System.Boolean myCopycatBool = true;
Console.WriteLine(myBool);
Console.WriteLine(myCopycatBool);

*/

// Data Structures

string[] names = new string[3];
names[0] = "John";
names[1] = "Edu";
Console.WriteLine(names[0]); // John
Console.WriteLine(names[1]); // Edu
Console.WriteLine(names[2]); // It will print an empty space
//Console.WriteLine(names[3]); // It will return an error
// Unhandled exception. System.IndexOutOfRangeException: Index was outside the bounds of the array.

string[] fruits = {"Apple", "Banana"};
Console.WriteLine(fruits[0]); // Apple
List<string> vegetables = new() { "Carrot", "Cucumber" };
Console.WriteLine(vegetables[0]); // Carrot
vegetables.Add("Onion");
Console.WriteLine(vegetables[2]); // Onion

IEnumerable<string> ieVegetables = vegetables;
Console.WriteLine(ieVegetables.First()); // Carrot
Console.WriteLine(ieVegetables.ElementAt(1)); // Cucumber

string[,] coords = {
    { "1", "2" },
    { "3", "4" },
    { "5", "6" },
};
Console.WriteLine(coords[0, 0]); // 1

Dictionary<string, string> students = new() {
    { "John", "A" },
    { "Edu", "B" },
    { "Lisa", "C" },
    { "Laura", "D" },
    { "Luke", "E" }
};
Console.WriteLine(students["John"]); // A

Dictionary<string, string[]> tasks = new() {
    { "John", new string[] { "Do homework", "Take a shower" } },
    { "Edu", new string[] { "Cook a meal", "Watch a movie" } }
};
Console.WriteLine(tasks["John"][0]); // Do homework