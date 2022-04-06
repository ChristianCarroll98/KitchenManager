using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.SharedNS.StatusNS;
using KitchenManager.API.UsersNS.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitchenManager.API.UsersNS.Repo
{
    public interface IUserRepository
    {
        Task<Response<UserDTO>> RetrieveById(int id);
        Task<Response<UserDTO>> RetrieveByUserName(string userName);
        Task<Response<UserDTO>> RetrieveByEmailAddress(string emailAddress);
        Task<Response<List<UserDTO>>> RetrieveByStatus(Status status); //admin only

        Task<Response<UserDTO>> Update(UserDTO model);
        Task<Response<UserDTO>> UpdateStatus(UserDTO model);
        Task<Response<UserDTO>> UpdateEmailAddress(UserDTO model);
        Task<Response<UserDTO>> Delete(UserDTO model);
        Task<Response<UserDTO>> ConfirmEmailAddress(UserDTO model);
    }

    public class UserRepository : IUserRepository
    {
        public Task<Response<UserDTO>> RetrieveById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Response<UserDTO>> RetrieveByUserName(string userName)
        {
            throw new System.NotImplementedException();
        }

        public Task<Response<UserDTO>> RetrieveByEmailAddress(string emailAddress)
        {
            throw new System.NotImplementedException();
        }
        
        public Task<Response<List<UserDTO>>> RetrieveByStatus(Status status)
        {
            throw new System.NotImplementedException();
        }
        
        public Task<Response<UserDTO>> Update(UserDTO model)
        {
            throw new System.NotImplementedException();
        }

        public Task<Response<UserDTO>> UpdateStatus(UserDTO model)
        {
            throw new System.NotImplementedException();
        }

        public Task<Response<UserDTO>> UpdateEmailAddress(UserDTO model)
        {
            throw new System.NotImplementedException();
        }

        public Task<Response<UserDTO>> Delete(UserDTO model)
        {
            throw new System.NotImplementedException();
        }

        public Task<Response<UserDTO>> ConfirmEmailAddress(UserDTO model)
        {
            throw new System.NotImplementedException();
        }
    }
}
