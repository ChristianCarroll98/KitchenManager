using System.ComponentModel.DataAnnotations;

namespace KitchenManager.API.UsersNS.Login
{
    public class UserLoginRequestDTO
    {
        [Required]
        public string UserEmail { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
            
        public bool LockOutOnFailure { get; set; }
    }
}
