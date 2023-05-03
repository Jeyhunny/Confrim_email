using System.ComponentModel.DataAnnotations;

namespace ELearn.ViewModel.Account
{
    public class RegisterVM
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-maail is not valid")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)] 
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password), Compare(nameof(Password))] 
        public string ComfirmPassword { get; set; }

        public bool IsRememberMe { get; set; }
    }
}
