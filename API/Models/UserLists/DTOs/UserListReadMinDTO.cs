﻿using KitchenManager.API.IconsNS.DTO;
using KitchenManager.API.ItemsNS.ListItemsNS.DTO;
using KitchenManager.API.UsersNS.DTO;
using System.Collections.Generic;
using System.Linq;

namespace KitchenManager.API.UserListsNS.DTO
{
    public class UserListReadMinDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconPath { get; set; }

        public UserListReadMinDTO(UserListModel userList)
        {
            Name = userList.Name ?? string.Empty;
            Description = userList.Description ?? string.Empty;
            IconPath = userList.Icon.Path ?? string.Empty;
        }
    }
}
