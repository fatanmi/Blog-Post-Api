using System.ComponentModel.DataAnnotations;


namespace ServerLibrary.Model.DTO
{
    public class UserDTO : LoginUserDTO
    {

        [Required]
        [DataType(DataType.Text)]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }

    public class LoginUserDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, ErrorMessage = "Your Password is limited to {2} to {1} characters", MinimumLength = 5)]
        public string Password { get; set; }

    }
    public class CreateUserDTO : UserDTO
    {
        public ICollection<string> Roles { get; set; }
    }

}
