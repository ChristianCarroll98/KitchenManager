using KitchenManager.API.IconsNS;
using KitchenManager.API.ItemsNS.ListItemsNS;
using KitchenManager.API.UsersNS;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitchenManager.API.UserListsNS
{
    public class UserList
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Description { get; set; } = string.Empty;

        public Icon Icon { get; set; }

        public List<ListItem> ListItems { get; set; }

        public User User { get; set; } = new();
    }
}
