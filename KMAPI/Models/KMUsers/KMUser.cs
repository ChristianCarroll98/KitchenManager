using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using KitchenManager.KMAPI.List;

namespace KitchenManager.KMAPI.KMUsers
{
    public class KMUser : IdentityUser<int>
    {   
        public List<UserList> Userlists { get; set; }
    }
}
