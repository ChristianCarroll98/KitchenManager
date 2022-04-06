using KitchenManager.API.IconsNS;
using KitchenManager.API.ItemsNS.ListItemsNS;
using KitchenManager.API.SharedNS.StatusNS;
using KitchenManager.API.UsersNS;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KitchenManager.API.UserListsNS
{
    public class UserList
    {
        [Required]
        public int Id { get; set; } = 0;

        [Required]
        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(256)]
        public string Description { get; set; } = string.Empty;

        public Icon Icon { get; set; } = new();

        public List<ListItem> ListItems { get; set; } = new();

        public User User { get; set; } = new();
    }
}
