using KitchenManager.API.IconsNS;
using KitchenManager.API.ItemsNS.ListItemsNS;
using KitchenManager.API.SharedNS.StatusNS;
using System;
using System.Collections.Generic;

namespace KitchenManager.API.UserListsNS
{
    public class UserListModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; } = Status.active;
        public DateTime DeletedDate { get; set; } = DateTime.MaxValue;

        public IconModel Icon { get; set; }
        public List<ListItemModel> ListItems { get; set; } = new();
    }
}
