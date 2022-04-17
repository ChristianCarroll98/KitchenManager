using System;
using System.ComponentModel.DataAnnotations;

namespace KitchenManager.API.UsersNS.DTO
{
    public class UserUpdateInfoDTO
    {
        [Required]
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
    }
}