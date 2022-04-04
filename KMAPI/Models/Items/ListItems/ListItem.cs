using System;
using System.ComponentModel.DataAnnotations;

namespace KitchenManager.KMAPI.Items.ListItems
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
