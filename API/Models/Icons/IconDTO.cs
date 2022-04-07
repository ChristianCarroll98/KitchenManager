namespace KitchenManager.API.IconsNS.DTO
{
    public class IconDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;

        public IconDTO() { }
        public IconDTO(Icon icon)
        {
            Name = icon.Name;
            Path = icon.Path;
        }
    }
}
