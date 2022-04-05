using KitchenManager.API.ItemsNS.ListItemsNS.DTO;
using KitchenManager.API.SharedNS.ResponseNS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitchenManager.API.ItemsNS.ListItemsNS.Repo
{
    public interface IListItemRepository
    {
        /*
            Name
            Brand
            UserList
            create a new itemTemplate with status inactive when creating a new item without a template.
            an admin can then at some point edit and approve it to go in the official template list
            or they can reject it and delete it.
         */
         /*
        Task<Response<ListItemDTO>> RetrieveById(int id);
        Task<Response<List<ListItemDTO>>> RetrieveByName(string name);
        Task<Response<List<ListItemDTO>>> RetrieveByBrand(string brand);
        Task<Response<ListItemDTO>> RetrieveByNameAndBrand(string name, string brand);
        Task<Response<List<ListItemDTO>>> RetrieveByStatus(ItemStatus status);
        Task<Response<List<ListItemDTO>>> RetrieveAll();

        Task<Response<ListItemDTO>> Create(ListItemDTO model);
        Task<Response<ListItemDTO>> Update(ListItemDTO model, string originalName, string originalBrand);
        Task<Response<ListItemDTO>> UpdateStatus(ItemStatus status, string name, string brand);
        Task<Response<ListItemDTO>> SetDeleteStatus(string name, string brand);
        Task<Response<ListItemDTO>> Delete(string name, string brand); */
    }

    public class ListItemRepository : IListItemRepository
    {
        
    }
}
