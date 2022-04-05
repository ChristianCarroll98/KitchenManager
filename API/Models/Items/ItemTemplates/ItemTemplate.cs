namespace KitchenManager.API.ItemsNS.ItemTemplatesNS
{
    public class ItemTemplate : Item
    {
        public int ExpirationDays { get; set; } = -1;

        public ItemTemplate()
        {
            Discriminator = "ItemTemplate";
        }
    }
}
