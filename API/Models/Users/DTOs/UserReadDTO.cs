using System;

namespace KitchenManager.API.UsersNS.DTO
{
    public class UserReadDTO
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }

        public UserReadDTO(UserModel user)
        {
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Birthday = user.Birthday;
        }
    }
}