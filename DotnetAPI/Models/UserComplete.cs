namespace DotnetAPI.Models;

public partial class UserComplete : User
{
    public string JobTitle { get; set; } = "";
    public string Department { get; set; } = "";
    public decimal Salary { get; set; }
    public decimal AvgSalary { get; set; }
}