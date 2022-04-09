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

        public ListItemReadDTO(ListItemModel listItem = null)
        {
            Name = listItem.Name ?? string.Empty;
            Brand = listItem.Brand ?? string.Empty;
            Description = listItem.Description ?? string.Empty;
            Quantity = listItem == null ? 1 : listItem.Quantity;
            ExpirationDate = listItem == null ? DateTime.MaxValue : listItem.ExpirationDate;
            IconPath = listItem.Icon.Path ?? string.Empty;
            ItemTagNames = listItem.ItemTags.Select(it => it.Name).ToList() ?? new List<string>();
        }
    }
}
