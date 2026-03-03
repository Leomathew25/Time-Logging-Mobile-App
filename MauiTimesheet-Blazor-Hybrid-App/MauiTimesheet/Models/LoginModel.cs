using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MauiTimesheet.Models;
public class LoginModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}
