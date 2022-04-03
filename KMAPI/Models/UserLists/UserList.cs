using KitchenManager.KMAPI.Icons;
using KitchenManager.KMAPI.Items.ListItems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.KMAPI.List
{
    public class UserList
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int KMUserId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Description { get; set; } = string.Empty;

        public Icon Icon { get; set; }

        public List<ListItem> ListItems { get; set; }
    }
}
