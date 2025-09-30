using System.ComponentModel.DataAnnotations;

namespace Connixt.UI.Models;

public class LoginViewModel
{
    [Required, EmailAddress]
    public string? Username { get; set; }

    [Required, DataType(DataType.Password)]
    public string? Password { get; set; }

    public string? ErrorMessage { get; set; }
}
