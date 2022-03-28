using System.ComponentModel.DataAnnotations;

namespace KitchenManager.KMAPI
{
    public class ItemTagItemTemplateJoinModel
    {
        [Required]
        public int ItemTagId { get; set; }

        [Required]
        public int ItemTemplateId { get; set; }
    }
}
