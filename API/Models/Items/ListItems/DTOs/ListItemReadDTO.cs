using System;
using System.Collections.Generic;
using System.Linq;

namespace KitchenManager.API.ItemsNS.ListItemsNS.DTO
{
    public class ListItemReadDTO
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string IconPath { get; set; }
        public List<string> ItemTagNames { get; set; }

        public ListItemReadDTO(ListItemModel listItem)
        {
            Name = listItem.Name;
            Brand = listItem.Brand;
            Description = listItem.Description;
            Quantity = listItem.Quantity;
            ExpirationDate = listItem.ExpirationDate;
            IconPath = listItem.Icon == null ? "Null Path" : listItem.Icon.Path;
            ItemTagNames = listItem.ItemTags.Select(it => it.Name).ToList();
        }
    }
}
