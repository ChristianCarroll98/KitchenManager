using System;
using System.Collections.Generic;

namespace KitchenManager.API.ItemsNS.ListItemsNS.DTO
{
    public class ListItemCreateUpdateDTO
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string IconName { get; set; }
        public List<string> ItemTagNames { get; set; }
    }
}
