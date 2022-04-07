using KitchenManager.API.UserListsNS;
using System;
using System.ComponentModel.DataAnnotations;

namespace KitchenManager.API.ItemsNS.ListItemsNS
{
    public class ListItem : Item
    {

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        public DateTime ExpirationDate { get; set; }

        public DateTime DeletedDate { get; set; } = DateTime.MaxValue;

        public UserList UserList { get; set; } = new();

        public ListItem()
        {
            Discriminator = "ListItem";
        } 
    }
}
