using KitchenManager.API.IconsNS;
using KitchenManager.API.ItemTagsNS.DTO;
using KitchenManager.API.UserListsNS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KitchenManager.API.ItemsNS.ListItemsNS.DTO
{
    public class ListItemDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
        public DateTime ExpirationDate { get; set; } = default;
        public Icon Icon { get; set; } = new();

        public UserList UserList { get; set; } = new();
        public List<ItemTagDTO> ItemTagDTOs { get; set; } = new List<ItemTagDTO>();

        public ListItemDTO() { }
        public ListItemDTO(ListItem listItem)
        {
            Name = listItem.Name;
            Brand = listItem.Brand;
            Description = listItem.Description;
            UserList = listItem.UserList;
            Quantity = listItem.Quantity;
            ExpirationDate = listItem.ExpirationDate;
            Icon = listItem.Icon;
            ItemTagDTOs = listItem.ItemTags.Select(it => new ItemTagDTO(it)).ToList();
        }
    }
}
