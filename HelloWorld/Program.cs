int[] ints = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
int intsTotal = 0;

static void printExecutionTime(DateTime startTime)
{
    Console.WriteLine((DateTime.Now - startTime).TotalSeconds);
}

// for loop
var startTime = DateTime.Now;
for (int i = 0; i < ints.Length; i++)
{
    intsTotal += ints[i];
}
Console.WriteLine(intsTotal); // 55
printExecutionTime(startTime); // 0.0079095

// foreach loop is faster than the for loop (more readable)
startTime = DateTime.Now;
foreach (int i in ints)
{
    intsTotal += i;
}
Console.WriteLine(intsTotal); // 110
printExecutionTime(startTime); // 0.0001872

// The while loop executes until a condition is matched
int index = 0;
startTime = DateTime.Now;
while (index < ints.Length)
{
    intsTotal += ints[index];
    index++;
}
Console.WriteLine(intsTotal); // 165
printExecutionTime(startTime); // 0.0001059

// The do while loop is similar to the while loop
// but executes the logic first and then checks the condition
// this will ensure at least one run
index = 0;
startTime = DateTime.Now;
do
{
    intsTotal += ints[index];
    index++;
}
while (index < ints.Length);
Console.WriteLine(intsTotal); // 220
printExecutionTime(startTime); // 0.0001219

// Loops have better performance even compared with some built-in methods
// take this in count when optimization is needed
startTime = DateTime.Now;
intsTotal += ints.Sum();
Console.WriteLine(intsTotal); // 275
printExecutionTime(startTime); // 0.0028676

// Loops and conditions can be combined to create powerful logic
foreach (int i in ints)
{
    if (i % 2 == 0)
    {
        // Only adds even numbers to the total
        intsTotal += i;
    }
}
Console.WriteLine(intsTotal); // 305


int total = 0;
DateTime start = DateTime.Now;
for (int i = 1; i < 1000; i++)
{
    total += i; 
}
Console.WriteLine(total); // 499501
printExecutionTime(start); // 0.0002616