# Methods

```CSHARP
// Method allow us to repeat some logic
// In C# methods have the following structure
// static(if applies) visibility(if applies can be public or private) returnValue name(params) {logic};
void printExecutionTime(DateTime startTime)
{
    Console.WriteLine((DateTime.Now - startTime).TotalSeconds);
}

int total = 0;
DateTime start = DateTime.Now;
for (int i = 1; i < 1000; i++)
{
    total += i;
}
Console.WriteLine(total); // 499500
printExecutionTime(start); // 0.0002642
```
