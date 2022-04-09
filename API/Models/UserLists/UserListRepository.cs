using KitchenManager.API.Data;
using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.UserListsNS.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.API.UserListsNS.Repo
{
    public interface IUserListsRepository
    {
        Task<ResponseModel<UserListReadDTO>> RetrieveById(int id);
        Task<ResponseModel<UserListReadDTO>> RetrieveByUserAndName(string userEmail, string name);
        Task<ResponseModel<List<UserListReadNoItemsDTO>>> RetrieveByUser(string userEmail);
        
        Task<ResponseModel<UserListReadNoItemsDTO>> Create(string userEmail, UserListCreateUpdateDTO model);
        Task<ResponseModel<UserListReadDTO>> Update(string userEmail, UserListCreateUpdateDTO model);
        Task<ResponseModel<UserListReadDTO>> Delete(string userEmail, UserListCreateUpdateDTO model);
    }

    public class UserListRepository : IUserListsRepository
    {
        private readonly KMDbContext Context;
        private readonly ILogger<IUserListsRepository> ULLogger;

        public UserListRepository(KMDbContext context, ILogger<IUserListsRepository> uLLogger)
        {
            Context = context;
            ULLogger = uLLogger;
        }

        public async Task<ResponseModel<UserListReadDTO>> RetrieveById(int id)
        {
            ResponseModel<UserListReadDTO> response = new();

            try
            {
                var userList = await Context
                        .UserLists
                        .Where(ul => ul.Id == id)
                        .FirstOrDefaultAsync();

                if (userList == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find User List with the specified Id.";
                    ULLogger.LogError($"Could not find User List with Id: {id}");
                    return response;
                }

                response.Data = new UserListReadDTO(userList);
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find User List with the specified Id.";
                ULLogger.LogError($"An error occured while attempting to find User List with Id: {id}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved User List with the specified Id.";
            return response;
        }

        public Task<ResponseModel<UserListReadDTO>> RetrieveByUserAndName(string userEmail, string name)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<List<UserListReadNoItemsDTO>>> RetrieveByUser(string userEmail)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<UserListReadNoItemsDTO>> Create(string userEmail, UserListCreateUpdateDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<UserListReadDTO>> Update(string userEmail, UserListCreateUpdateDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<UserListReadDTO>> Delete(string userEmail, UserListCreateUpdateDTO model)
        {
            throw new NotImplementedException();
        }
    }
}