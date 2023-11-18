using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAPI.Models;

public partial class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Name { get { return FirstName + " " + LastName; } }
    public string Email { get; set; } = "";
    public string Gender { get; set; } = "";
    public bool Active { get; set; } = false;
};