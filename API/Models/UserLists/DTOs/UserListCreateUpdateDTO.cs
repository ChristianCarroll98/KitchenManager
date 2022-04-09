namespace KitchenManager.API.UserListsNS.DTO
{
    public class UserListCreateUpdateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconName { get; set; }
        public string IconPath { get; set; }
        public string UserEmail { get; set; }
    }
}