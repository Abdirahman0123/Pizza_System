using System.ComponentModel.DataAnnotations;

namespace Pizza_System.Dtos
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"^[a-zA-Z ]*$",ErrorMessage = "Numbers & Symbols are not allowed.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage ="Last name is required")]
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Numbers & Symbols are not allowed.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter valid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
