using System;
using System.ComponentModel.DataAnnotations;

namespace KitchenManager.API.UsersNS.DTO
{
    public class UserCreateDTO
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public string Role { get; set; }
    }
}