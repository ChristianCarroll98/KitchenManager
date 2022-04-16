using KitchenManager.API.UserListsNS.DTO;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace KitchenManager.API.UsersNS.DTO
{
    public class UserReadDTO
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UserReadDTO(UserModel user, string firstName, string lastName)
        {
            Email = user.Email;
            PhoneNumber = user.PhoneNumber ?? string.Empty;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}