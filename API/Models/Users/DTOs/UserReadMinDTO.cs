namespace KitchenManager.API.UsersNS.DTO
{
    public class UserReadMinDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public UserReadMinDTO(UserModel user = null)
        {
            UserName = user.UserName ?? string.Empty;
            Email = user.Email ?? string.Empty;
            PhoneNumber = user.PhoneNumber ?? string.Empty;
        }
    }
}