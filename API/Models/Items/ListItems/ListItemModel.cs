using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenManager.API.ItemsNS.ListItemsNS
{
    public class ListItemModel : ItemModel
    {
        [Required]
        [ForeignKey("UserList")]
        public int UserListId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        public DateTime ExpirationDate { get; set; } = DateTime.MaxValue;

        public DateTime DeletedDate { get; set; } = DateTime.MaxValue;

        public ListItemModel()
        {
            Discriminator = "ListItem";
        } 
    }
}
