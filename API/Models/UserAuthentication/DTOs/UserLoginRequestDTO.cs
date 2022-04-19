using System.ComponentModel.DataAnnotations;

namespace KitchenManager.API.UserAuthNS.DTO
{
    public class UserLoginRequestDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
