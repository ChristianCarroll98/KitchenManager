using System;
using System.ComponentModel.DataAnnotations;

namespace KitchenManager.KMAPI.Items.ListItems
{
    public enum ListItemStatus
    {
        listed,
        unlisted,
        deleted
    }

    public class ListItem : Item
    {
        public ListItem()
        {
            Discriminator = "ListItem";
        }
        
        public int UserListId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public DateTime ExpirationDate { get; set; }

        public ListItemStatus Status { get; set; } = ListItemStatus.listed;
    }
}
