using System.Collections.Generic;
using KitchenManager.API.SharedNS.StatusNS;
using KitchenManager.API.UserListsNS;
using Microsoft.AspNetCore.Identity;

namespace KitchenManager.API.UsersNS
{
    public class User : IdentityUser<int>
    {
        public Status Status { get; set; }
        public List<UserList> UserLists { get; set; }
    }
}
