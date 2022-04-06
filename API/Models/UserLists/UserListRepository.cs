using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.UserListsNS.DTO;
using KitchenManager.API.UsersNS.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitchenManager.API.UserListsNS.Repo
{
    public interface IUserListsRepository
    {
        Task<Response<UserListDTO>> RetrieveById(int id);
        Task<Response<UserListDTO>> RetrieveByUserAndName(UserDTO user, string name);
        Task<Response<List<UserListDTO>>> RetrieveByUser(UserDTO user);

        Task<Response<UserListDTO>> Create(UserListDTO model);
        Task<Response<UserListDTO>> Update(UserListDTO model);
        Task<Response<UserListDTO>> Delete(UserListDTO model);
    }

    public class UserListRepository : IUserListsRepository
    {
        public Task<Response<UserListDTO>> RetrieveById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Response<UserListDTO>> RetrieveByUserAndName(UserDTO user, string name)
        {
            throw new System.NotImplementedException();
        }

        public Task<Response<List<UserListDTO>>> RetrieveByUser(UserDTO user)
        {
            throw new System.NotImplementedException();
        }

        public Task<Response<UserListDTO>> Create(UserListDTO model)
        {
            throw new System.NotImplementedException();
        }

        public Task<Response<UserListDTO>> Update(UserListDTO model)
        {
            throw new System.NotImplementedException();
        }

        public Task<Response<UserListDTO>> Delete(UserListDTO model)
        {
            throw new System.NotImplementedException();
        }
    }
}