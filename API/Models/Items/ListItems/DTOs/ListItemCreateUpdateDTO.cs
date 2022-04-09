using KitchenManager.API.IconsNS.DTO;
using KitchenManager.API.ItemTagsNS.DTO;
using KitchenManager.API.UserListsNS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KitchenManager.API.ItemsNS.ListItemsNS.DTO
{
    public class ListItemCreateUpdateDTO
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string IconPath { get; set; }
        public string UserEmail { get; set; }
        public string UserListName { get; set; }
        public List<string> ItemTagNames { get; set; }
    }
}
