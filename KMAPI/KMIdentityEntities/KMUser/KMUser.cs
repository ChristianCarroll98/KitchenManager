using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.KMAPI
{
    public class KMUser : IdentityUser<int>
    {   
        // My Customized Identity User Class
        [MaxLength(256)]
        public string FirstName { get; set; }

        [MaxLength(256)]
        public string LastName { get; set; }

        public List<UserList> Userlists { get; set; }
    }
}
