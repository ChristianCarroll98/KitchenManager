using System.ComponentModel.DataAnnotations;

namespace KitchenManager.KMAPI.KMUsers.Login
{
    public class KMUserLoginRequest
        {
            [Required]
            public string UserName { get; set; }

            [Required]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
            
            public bool LockOutOnFailure { get; set; }
        }
    }
