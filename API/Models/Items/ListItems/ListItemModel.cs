using System;
using System.ComponentModel.DataAnnotations;

namespace KitchenManager.API.ItemsNS.ListItemsNS
{
    public class ListItemModel : ItemModel
    {
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;
        public int UserListId { get; set; }
        public DateTime ExpirationDate { get; set; } = DateTime.MaxValue;
    }
}
