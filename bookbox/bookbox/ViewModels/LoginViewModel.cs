using System.ComponentModel.DataAnnotations;

namespace BookBox.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        [StringLength(10, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
