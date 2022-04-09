using KitchenManager.API.IconsNS;
using KitchenManager.API.ItemsNS.ListItemsNS;
using KitchenManager.API.SharedNS.StatusNS;
using KitchenManager.API.UsersNS;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenManager.API.UserListsNS
{
    public class UserListModel
    {
        [Required]
        public int Id { get; set; } = 0;

        [Required]
        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(256)]
        public string Description { get; set; } = string.Empty;

        public IconModel Icon { get; set; }

        public User User { get; set; }

        public List<ListItemModel> ListItems { get; set; } = new();
    }
}
