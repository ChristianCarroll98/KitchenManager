namespace KitchenManager.API.UserListsNS.DTO
{
    public class UserListReadDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconPath { get; set; }

        public UserListReadDTO(UserListModel userList)
        {
            Name = userList.Name;
            Description = userList.Description ?? string.Empty;
            IconPath = userList.Icon.Path ?? string.Empty;
        }
    }
}
