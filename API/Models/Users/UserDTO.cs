using KitchenManager.API.UserListsNS.DTO;
using System.Collections.Generic;
using System.Linq;

namespace KitchenManager.API.UsersNS.DTO
{
    public class UserDTO
    {
        public string UserName { get; set; } = string.Empty;
        public virtual string Email { get; set; } = string.Empty;
        public virtual string PhoneNumber { get; set; } = string.Empty;
        public virtual bool TwoFactorEnabled { get; set; } = false;
        public virtual bool LockoutEnabled { get; set; } = true;
        public List<UserListDTO> UserListDTOs { get; set; } = new();

        public UserDTO() { }
        public UserDTO(User user, bool includeUserList)
        {
            UserName = user.UserName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            TwoFactorEnabled = user.TwoFactorEnabled;
            LockoutEnabled = user.LockoutEnabled;
            if (includeUserList) UserListDTOs = user.UserLists.Select(ul => new UserListDTO(ul, false)).ToList();
        }
    }
}