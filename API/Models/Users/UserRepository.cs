using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.SharedNS.StatusNS;
using KitchenManager.API.UsersNS.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitchenManager.API.UsersNS.Repo
{
    public interface IUserRepository
    {
        Task<ResponseModel<UserReadDTO>> RetrieveById(int id);
        Task<ResponseModel<UserReadDTO>> RetrieveByEmailAddress(string emailAddress);
        Task<ResponseModel<List<UserReadDTO>>> RetrieveByStatus(Status status); //admin only

        Task<ResponseModel<UserReadDTO>> Update(UserReadDTO model);
        Task<ResponseModel<UserReadDTO>> UpdateStatus(UserReadDTO model);
        Task<ResponseModel<UserReadDTO>> UpdateEmailAddress(UserReadDTO model);
        Task<ResponseModel<UserReadDTO>> Delete(UserReadDTO model);
        Task<ResponseModel<UserReadDTO>> ConfirmEmailAddress(UserReadDTO model);
    }

    public class UserRepository : IUserRepository
    {
        public Task<ResponseModel<UserReadDTO>> RetrieveById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<UserReadDTO>> RetrieveByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<UserReadDTO>> RetrieveByEmailAddress(string emailAddress)
        {
            throw new NotImplementedException();
        }
        
        public Task<ResponseModel<List<UserReadDTO>>> RetrieveByStatus(Status status)
        {
            throw new NotImplementedException();
        }
        
        public Task<ResponseModel<UserReadDTO>> Update(UserReadDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<UserReadDTO>> UpdateStatus(UserReadDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<UserReadDTO>> UpdateEmailAddress(UserReadDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<UserReadDTO>> Delete(UserReadDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<UserReadDTO>> ConfirmEmailAddress(UserReadDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
