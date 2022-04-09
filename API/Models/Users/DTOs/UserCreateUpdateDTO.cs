using System.ComponentModel.DataAnnotations;

namespace KitchenManager.API.UsersNS.DTO
{
    public class UserCreateUpdateDTO
    {
        [Required]
        public virtual string Email { get; set; }
        [Required]
        public virtual string PhoneNumber { get; set; }
        public virtual bool TwoFactorEnabled { get; set; } = false;
        public virtual bool LockoutEnabled { get; set; } = true;
        //add to this as needed when adding authentication system.
    }
}