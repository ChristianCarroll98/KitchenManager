using System.Collections.Generic;
using KitchenManager.API.SharedNS.StatusNS;
using KitchenManager.API.UserListsNS;
using Microsoft.AspNetCore.Identity;

namespace KitchenManager.API.UsersNS
{
    public class UserModel : IdentityUser<int>
    {
        public Status Status { get; set; }
        public List<UserListModel> UserLists { get; set; }

        public UserModel() : base()
        {
        }
    }
}
