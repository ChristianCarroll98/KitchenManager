using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.KMAPI
{
    public class ListItem : Item
    {
        public ListItem()
        {
            Discriminator = "ListItem";
        }
        
        [Required]
        public int UserListId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
