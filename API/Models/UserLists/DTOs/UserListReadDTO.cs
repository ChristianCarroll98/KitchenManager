using KitchenManager.API.ItemsNS.ListItemsNS.DTO;
using System.Collections.Generic;
using System.Linq;

namespace KitchenManager.API.UserListsNS.DTO
{
    public class UserListReadDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconPath { get; set; }
        public List<ListItemReadDTO> ListItemReadDTOs { get; set; }

        public UserListReadDTO(UserListModel userList)
        {
            Name = userList.Name ?? string.Empty;
            Description = userList.Description ?? string.Empty;
            IconPath = userList.Icon.Path ?? string.Empty;
            ListItemReadDTOs = userList.ListItems.Select(li => new ListItemReadDTO(li)).ToList() ?? new List<ListItemReadDTO>();
        }
    }
}
