using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.UserListsNS.DTO;
using KitchenManager.API.UsersNS.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitchenManager.API.UserListsNS.Repo
{
    public interface IUserListsRepository
    {
        Task<ResponseModel<UserListReadDTO>> RetrieveById(int id);
        Task<ResponseModel<UserListReadDTO>> RetrieveByUserAndName(UserReadDTO user, string name);
        Task<ResponseModel<List<UserListReadDTO>>> RetrieveByUser(UserReadDTO user);

        Task<ResponseModel<UserListReadDTO>> Create(UserListReadDTO model);
        Task<ResponseModel<UserListReadDTO>> Update(UserListReadDTO model);
        Task<ResponseModel<UserListReadDTO>> Delete(UserListReadDTO model);
    }

    public class UserListRepository : IUserListsRepository
    {
        public Task<ResponseModel<UserListReadDTO>> RetrieveById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<UserListReadDTO>> RetrieveByUserAndName(UserReadDTO user, string name)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<List<UserListReadDTO>>> RetrieveByUser(UserReadDTO user)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<UserListReadDTO>> Create(UserListReadDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<UserListReadDTO>> Update(UserListReadDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<UserListReadDTO>> Delete(UserListReadDTO model)
        {
            throw new NotImplementedException();
        }
    }
}