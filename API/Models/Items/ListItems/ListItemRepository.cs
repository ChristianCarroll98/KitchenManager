using KitchenManager.API.Data;
using KitchenManager.API.ItemsNS.ListItemsNS.DTO;
using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.SharedNS.StatusNS;
using KitchenManager.API.UserListsNS.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.API.ItemsNS.ListItemsNS.Repo
{
    public interface IListItemRepository
    {
        Task<Response<ListItemDTO>> RetrieveById(int id);
        Task<Response<List<ListItemDTO>>> RetrieveByUserListAndBrand(string userName, string userListName, string brand);
        Task<Response<ListItemDTO>> RetrieveByUserListAndNameAndBrand(string userName, string userListName, string name, string brand);
        Task<Response<List<ListItemDTO>>> RetrieveByUserListAndStatus(string userName, string userListName, Status status);
        Task<Response<List<ListItemDTO>>> RetrieveByUserListAndTags(string userName, string userListName, List<string> tagNames);
        Task<Response<List<ListItemDTO>>> RetrieveByUserList(string userName, string userListName);

        Task<Response<ListItemDTO>> Create(string userName, string userListName, ListItemDTO model);
        Task<Response<ListItemDTO>> Update(string userName, string userListName, string originalName, string originalBrand, ListItemDTO model);
        Task<Response<ListItemDTO>> SetQuantity(string userName, string userListName, string name, string brand, int quantity);
        Task<Response<ListItemDTO>> SetActiveStatus(string userName, string userListName, string name, string brand);
        Task<Response<ListItemDTO>> SetDeleteStatus(string userName, string userListName, string name, string brand);
        Task<Response<ListItemDTO>> Delete(string userName, string userListName, string name, string brand);
    }

    public class ListItemRepository : IListItemRepository
    {
        private readonly KMDbContext Context;
        private readonly ILogger<ListItemRepository> LILogger;

        public ListItemRepository(KMDbContext context, ILogger<ListItemRepository> lILogger)
        {
            Context = context;
            LILogger = lILogger;
        }

        public async Task<Response<ListItemDTO>> RetrieveById(int id)
        {
            Response<ListItemDTO> response = new();

            try
            {
                var listItem = await Context
                        .ListItems
                        .Include(x => x.ItemTags)
                        .Where(li => li.Id == id)
                        .FirstOrDefaultAsync();

                if (listItem == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find List Item with the specified Id.";
                    LILogger.LogError($"Could not find List Item with Id: {id}");
                    return response;
                }

                response.Data = new ListItemDTO(listItem);
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find List Item with the specified Id.";
                LILogger.LogError($"An error occured while attempting to find List Item with Id: {id}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved List Item with the specified Id.";
            return response;
        }

        public async Task<Response<List<ListItemDTO>>> RetrieveByUserListAndBrand(string userName, string userListName, string brand)
        {
            Response<List<ListItemDTO>> response = new();

            try
            {
                var listItems = await Context
                        .ListItems
                        .Include(x => x.ItemTags)
                        .Where(li => li.UserList.User.UserName == userName &&
                                li.UserList.Name == userListName &&
                                li.Brand == brand)
                        .ToListAsync();

                if (!listItems.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any List Items with Brand: {brand}";
                    LILogger.LogError($"Could not find any Items on {userName}'s list: {userListName} with Brand: {brand}");
                    return response;
                }

                response.Data = listItems.Select(li => new ListItemDTO(li)).ToList();
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find List Items with Brand: {brand}.";
                LILogger.LogError($"An error occured while attempting to find Items on {userName}'s list: {userListName} with Brand: {brand}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved List Items with Brand: {brand}.";
            return response;
        }

        public Task<Response<ListItemDTO>> RetrieveByUserListAndNameAndBrand(string userName, string userListName, string name, string brand)
        {
            /*Response<ListItemDTO> response = new();

            try
            {
                var listItem = await Context
                        .ListItems
                        .Include(x => x.ItemTags)
                        .Where(li => li.Id == id)
                        .FirstOrDefaultAsync();

                if (listItem == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find List Item with the specified Id.";
                    LILogger.LogError($"Could not find List Item with Id: {id}");
                    return response;
                }

                response.Data = new ListItemDTO(listItem);
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find List Item with the specified Id.";
                LILogger.LogError($"An error occured while attempting to find List Item with Id: {id}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved List Item with the specified Id.";
            return response;*/
            throw new NotImplementedException();
        }

        public Task<Response<List<ListItemDTO>>> RetrieveByUserListAndStatus(string userName, string userListName, Status status)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<ListItemDTO>>> RetrieveByUserListAndTags(string userName, string userListName, List<string> tagNames)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<ListItemDTO>>> RetrieveByUserList(string userName, string userListName)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ListItemDTO>> Create(string userName, string userListName, ListItemDTO model)
        {
            throw new NotImplementedException();
        }
        
        public Task<Response<ListItemDTO>> Update(string userName, string userListName, string originalName, string originalBrand, ListItemDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ListItemDTO>> SetQuantity(string userName, string userListName, string name, string brand, int quantity)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ListItemDTO>> SetActiveStatus(string userName, string userListName, string name, string brand)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ListItemDTO>> SetDeleteStatus(string userName, string userListName, string name, string brand)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ListItemDTO>> Delete(string userName, string userListName, string name, string brand)
        {
            throw new NotImplementedException();
        }
    }
}
