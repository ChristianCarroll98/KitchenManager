namespace KitchenManager.API.IconsNS.DTO
{
    public class IconDTO
    {
        public int Name { get; set; }
        public string Path { get; set; }

        public IconDTO() { }
        public IconDTO(Icon icon)
        {
            Name = icon.Name;
            Path = icon.Path;
        }
    }
}
