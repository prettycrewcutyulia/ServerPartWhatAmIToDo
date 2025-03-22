namespace ServerPartWhatAmIToDo.Models;
using System.ComponentModel.DataAnnotations;

public class ResetPasswordModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
