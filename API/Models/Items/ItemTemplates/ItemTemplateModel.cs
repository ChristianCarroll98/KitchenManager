namespace KitchenManager.API.ItemsNS.ItemTemplatesNS
{
    public class ItemTemplateModel : ItemModel
    {
        public int ExpirationDays { get; set; } = -1;

        public ItemTemplateModel()
        {
            Discriminator = "ItemTemplate";
        }
    }
}
