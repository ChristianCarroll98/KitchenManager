using KitchenManager.API.IconsNS.DTO;
using KitchenManager.API.ItemsNS.ListItemsNS.DTO;
using KitchenManager.API.UsersNS.DTO;
using System.Collections.Generic;
using System.Linq;

namespace KitchenManager.API.UserListsNS.DTO
{
    public class UserListDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IconDTO IconDTO { get; set; } = new();
        public List<ListItemDTO> ListItemDTOs { get; set; } = new();
        public UserDTO UserDTO { get; set; } = new();

        public UserListDTO() { }
        public UserListDTO(UserList userList, bool includeItems)
        {
            Name = userList.Name;
            Description = userList.Description;
            IconDTO = new IconDTO(userList.Icon);
            if (includeItems) ListItemDTOs = userList.ListItems.Select(li => new ListItemDTO(li)).ToList();
            else ListItemDTOs = new List<ListItemDTO>();
            UserDTO = new UserDTO(userList.User);
        }
    }
}
