using KitchenManager.API.IconsNS.DTO;
using KitchenManager.API.ItemsNS.ListItemsNS.DTO;
using KitchenManager.API.UsersNS.DTO;
using System.Collections.Generic;
using System.Linq;

namespace KitchenManager.API.UserListsNS.DTO
{
    public class UserListCreateUpdateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IconCreateUpdateDTO IconCreateUpdateDTO { get; set; }
        public string UserEmail { get; set; }
    }
}