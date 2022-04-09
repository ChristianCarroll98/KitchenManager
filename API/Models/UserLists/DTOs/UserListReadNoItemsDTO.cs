namespace KitchenManager.API.UserListsNS.DTO
{
    public class UserListReadNoItemsDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconPath { get; set; }

        public UserListReadNoItemsDTO(UserListModel userList)
        {
            Name = userList.Name ?? string.Empty;
            Description = userList.Description ?? string.Empty;
            IconPath = userList.Icon.Path ?? string.Empty;
        }
    }
}
