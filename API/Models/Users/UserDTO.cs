using KitchenManager.API.UserListsNS.DTO;
using System.Collections.Generic;
using System.Linq;

namespace KitchenManager.API.UsersNS.DTO
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public virtual string Email { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual bool TwoFactorEnabled { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        public List<UserListDTO> UserListDTOs { get; set; }

        public UserDTO() { }
        public UserDTO(User user)
        {
            UserName = user.UserName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            TwoFactorEnabled = user.TwoFactorEnabled;
            LockoutEnabled = user.LockoutEnabled;
            UserListDTOs = user.UserLists.Select(ul => new UserListDTO(ul, true)).ToList();
        }
    }
}