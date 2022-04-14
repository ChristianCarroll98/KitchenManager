using KitchenManager.API.IconsNS;
using KitchenManager.API.ItemsNS.ListItemsNS;
using KitchenManager.API.UsersNS;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenManager.API.UserListsNS
{
    public class UserListModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;

        public IconModel Icon { get; set; }
        public List<ListItemModel> ListItems { get; set; } = new();
    }
}
