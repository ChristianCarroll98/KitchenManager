using KitchenManager.KMAPI.UserLists;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.KMAPI.KMUsers
{
    public class KMUser : IdentityUser<int>
    {   
        public List<UserList> Userlists { get; set; }
    }
}
