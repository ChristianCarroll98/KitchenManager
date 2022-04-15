using KitchenManager.API.Data;
using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.SharedNS.StatusNS;
using KitchenManager.API.UserListsNS.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.API.UserListsNS.Repo
{
    public interface IUserListRepository
    {
        Task<ResponseModel<UserListReadDTO>> RetrieveById(int id);
        Task<ResponseModel<UserListReadDTO>> RetrieveByUserAndName(string userEmail, string name);
        Task<ResponseModel<List<UserListReadDTO>>> RetrieveByUserAndStatus(string userEmail, Status status);
        Task<ResponseModel<List<UserListReadDTO>>> RetrieveByUser(string userEmail);

        Task<ResponseModel<UserListReadDTO>> Create(string userEmail, UserListCreateUpdateDTO model);
        Task<ResponseModel<UserListReadDTO>> Update(string userEmail, string originalName, UserListCreateUpdateDTO model);
        Task<ResponseModel<UserListReadDTO>> SetActiveStatus(string userEmail, string name);
        Task<ResponseModel<UserListReadDTO>> SetDeletedStatus(string userEmail, string name);
        Task<ResponseModel<UserListReadDTO>> Delete(string userEmail, string name);
    }

    public class UserListRepository : IUserListRepository
    {
        private readonly KMDbContext Context;
        private readonly ILogger<IUserListRepository> ULLogger;

        public UserListRepository(KMDbContext context, ILogger<IUserListRepository> uLLogger)
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
                        .Include(ul => ul.Icon)
                        .Include(ul => ul.ListItems)
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

        public async Task<ResponseModel<UserListReadDTO>> RetrieveByUserAndName(string userEmail, string name)
        {
            ResponseModel<UserListReadDTO> response = new();

            try
            {
                var user = await Context.Users
                            .Where(u => u.NormalizedEmail == userEmail.ToUpperInvariant() &&
                                    u.Status == Status.active)
                            .FirstOrDefaultAsync();

                if (user == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find a User with the specified email address.";
                    ULLogger.LogError($"Could not find a User with Email: {userEmail}.");
                    return response;
                }

                var userList = await Context.UserLists
                        .Include(ul => ul.Icon)
                        .Where(ul => ul.UserId == user.Id &&
                                ul.Name == name)
                        .FirstOrDefaultAsync();

                if (userList == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find User List named {name} for User with Email: {userEmail}.";
                    ULLogger.LogError($"Could not find User List named {name} for User with Email: {userEmail}.");
                    return response;
                }

                response.Data = new UserListReadDTO(userList);
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find User List named {name} for User with Email: {userEmail}.";
                ULLogger.LogError($"An error occured while attempting to find User List named {name} for User with Email: {userEmail}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved User List named {name} for User with Email: {userEmail}.";
            return response;
        }

        public async Task<ResponseModel<List<UserListReadDTO>>> RetrieveByUserAndStatus(string userEmail, Status status)
        {
            ResponseModel<List<UserListReadDTO>> response = new();

            try
            {
                var user = await Context.Users
                            .Where(u => u.NormalizedEmail == userEmail.ToUpperInvariant() &&
                                    u.Status == Status.active)
                            .FirstOrDefaultAsync();

                if (user == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find a User with the specified email address.";
                    ULLogger.LogError($"Could not find a User with Email: {userEmail}.");
                    return response;
                }

                var userLists = await Context
                        .UserLists
                        .Include(ul => ul.Icon)
                        .Where(ul => ul.UserId == user.Id &&
                                    ul.Status == status)
                        .ToListAsync();

                if (!userLists.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any User Lists for User with Email: {userEmail} with status: {status.ToString()}.";
                    ULLogger.LogError($"Could not find any User Lists for User with Email: {userEmail} with status: {status.ToString()}.");
                    return response;
                }

                response.Data = userLists.Select(ul => new UserListReadDTO(ul)).ToList();
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find User Lists for User with Email: {userEmail} with status: {status.ToString()}.";
                ULLogger.LogError($"An error occured while attempting to find User Lists for User with Email: {userEmail} with status: {status.ToString()}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved {response.Data.Count()} User Lists for User with Email: {userEmail} with status: {status.ToString()}.";
            return response;
        }

        public async Task<ResponseModel<List<UserListReadDTO>>> RetrieveByUser(string userEmail)
        {
            ResponseModel<List<UserListReadDTO>> response = new();

            try
            {
                var user = await Context.Users
                            .Where(u => u.NormalizedEmail == userEmail.ToUpperInvariant() &&
                                    u.Status == Status.active)
                            .FirstOrDefaultAsync();

                if (user == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find a User with the specified email address.";
                    ULLogger.LogError($"Could not find a User with Email: {userEmail}.");
                    return response;
                }

                var userLists = await Context
                        .UserLists
                        .Include(ul => ul.Icon)
                        .Where(ul => ul.UserId == user.Id)
                        .ToListAsync();

                if (!userLists.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any User Lists for User with Email: {userEmail}.";
                    ULLogger.LogError($"Could not find any User Lists for User with Email: {userEmail}.");
                    return response;
                }

                response.Data = userLists.Select(ul => new UserListReadDTO(ul)).ToList();
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find User Lists for User with Email: {userEmail}.";
                ULLogger.LogError($"An error occured while attempting to find User Lists for User with Email: {userEmail}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved {response.Data.Count()} User Lists for User with Email: {userEmail}.";
            return response;
        }

        public async Task<ResponseModel<UserListReadDTO>> Create(string userEmail, UserListCreateUpdateDTO model)
        {
            ResponseModel<UserListReadDTO> response = new();

            try
            {
                var user = await Context.Users
                            .Where(u => u.NormalizedEmail == userEmail.ToUpperInvariant() &&
                                    u.Status == Status.active)
                            .FirstOrDefaultAsync();

                if (user == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find a User with the specified email address.";
                    ULLogger.LogError($"Could not find a User with Email: {userEmail}.");
                    return response;
                }

                ResponseModel<UserListReadDTO> checkPreExisting = await RetrieveByUserAndName(userEmail, model.Name);

                if (checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"A User List already exists for User with Email: {userEmail} with Name: {model.Name}. Message: {checkPreExisting.Message}";
                    response.Data = checkPreExisting.Data;
                    ULLogger.LogError($"A User List already exists for User with Email: {userEmail} with Name: {model.Name}. Message: {checkPreExisting.Message}");
                    return response;
                }

                var icon = await Context.Icons.Where(i => i.Name == model.IconName).FirstOrDefaultAsync();

                if (icon == null)
                {
                    response.Success = false;
                    response.Message = $"There is no Icon with Name: {model.IconName}.";
                    ULLogger.LogError($"There is no Icon with Name: {model.IconName}.");
                    return response;
                }

                var userList = new UserListModel()
                {
                    Name = model.Name,
                    Description = model.Description,
                    UserId = user.Id,
                    Icon = icon
                };

                await Context.UserLists.AddAsync(userList);
                await Context.SaveChangesAsync();

                ResponseModel<UserListReadDTO> checkAdded = await RetrieveByUserAndName(userEmail, model.Name);

                if (!checkAdded.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to save the new User List. Message: {checkAdded.Message}";
                    ULLogger.LogError($"Failed to add new User List to Database for User with Email: {userEmail}. Message: {checkAdded.Message}");
                    return response;
                }

                response.Data = checkAdded.Data;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to create a User List for User with Email: {userEmail}.";
                ULLogger.LogError($"An error occured while attempting to create a User List for User with Email: {userEmail}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully created new User List with Name: {model.Name} for User with Email: {userEmail}.";
            return response;
        }

        public async Task<ResponseModel<UserListReadDTO>> Update(string userEmail, string originalName, UserListCreateUpdateDTO model)
        {
            ResponseModel<UserListReadDTO> response = new();

            try
            {
                var user = await Context.Users
                            .Where(u => u.NormalizedEmail == userEmail.ToUpperInvariant() &&
                                    u.Status == Status.active)
                            .FirstOrDefaultAsync();

                if (user == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find a User with the specified email address.";
                    ULLogger.LogError($"Could not find a User with Email: {userEmail}.");
                    return response;
                }

                ResponseModel<UserListReadDTO> checkPreExisting = await RetrieveByUserAndName(userEmail, originalName);

                if (!checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"A User List does not exist for User with Email: {userEmail} with original Name: {originalName}. Message: {checkPreExisting.Message}";
                    ULLogger.LogError($"A User List does not exist for User with Email: {userEmail} with original Name: {originalName}. Message: {checkPreExisting.Message}");
                    return response;
                }

                var icon = await Context.Icons.Where(i => i.Name == model.IconName).FirstOrDefaultAsync();

                if (icon == null)
                {
                    response.Success = false;
                    response.Message = $"There is no Icon with Name: {model.IconName}.";
                    ULLogger.LogError($"There is no Icon with Name: {model.IconName}.");
                    return response;
                }

                var userList = await Context.UserLists
                        .Include(ul => ul.Icon)
                        .Where(ul => ul.UserId == user.Id &&
                                ul.Name == originalName)
                        .FirstOrDefaultAsync();

                userList.Name = model.Name;
                userList.Description = model.Description;
                userList.Icon = icon;

                Context.UserLists.Update(userList);
                await Context.SaveChangesAsync();

                response.Data = new UserListReadDTO(userList);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to update User List with original Name: {originalName} for User with Email: {userEmail}.";
                ULLogger.LogError($"An error occured while attempting to update User List with original Name: {originalName} for User with Email: {userEmail}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully updated User List with original Name: {originalName} for User with Email: {userEmail}.";
            return response;
        }

        public async Task<ResponseModel<UserListReadDTO>> SetActiveStatus(string userEmail, string name)
        {
            ResponseModel<UserListReadDTO> response = new();

            try
            {
                var user = await Context.Users
                            .Where(u => u.NormalizedEmail == userEmail.ToUpperInvariant() &&
                                    u.Status == Status.active)
                            .FirstOrDefaultAsync();

                if (user == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find a User with the specified email address.";
                    ULLogger.LogError($"Could not find a User with Email: {userEmail}.");
                    return response;
                }

                ResponseModel<UserListReadDTO> checkPreExisting = await RetrieveByUserAndName(userEmail, name);

                if (!checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"A User List does not exist for User with Email: {userEmail} with Name: {name}. Message: {checkPreExisting.Message}";
                    ULLogger.LogError($"A User List does not exist for User with Email: {userEmail} with Name: {name}. Message: {checkPreExisting.Message}");
                    return response;
                }

                var userList = await Context.UserLists
                        .Include(ul => ul.Icon)
                        .Include(ul => ul.ListItems)
                        .Where(ul => ul.UserId == user.Id &&
                                ul.Name == name)
                        .FirstOrDefaultAsync();

                userList.DeletedDate = DateTime.MaxValue;
                userList.Status = Status.active;

                Context.UserLists.Update(userList);
                await Context.SaveChangesAsync();

                response.Data = new UserListReadDTO(userList);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to activate User List with Name: {name} for User with Email: {userEmail}.";
                ULLogger.LogError($"An error occured while attempting to activate User List with Name: {name} for User with Email: {userEmail}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully activated User List with Name: {name} for User with Email: {userEmail}.";
            return response;
        }

        public async Task<ResponseModel<UserListReadDTO>> SetDeletedStatus(string userEmail, string name)
        {
            ResponseModel<UserListReadDTO> response = new();

            try
            {
                var user = await Context.Users
                            .Where(u => u.NormalizedEmail == userEmail.ToUpperInvariant() &&
                                    u.Status == Status.active)
                            .FirstOrDefaultAsync();

                if (user == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find a User with the specified email address.";
                    ULLogger.LogError($"Could not find a User with Email: {userEmail}.");
                    return response;
                }

                ResponseModel<UserListReadDTO> checkPreExisting = await RetrieveByUserAndName(userEmail, name);

                if (!checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"A User List does not exist for User with Email: {userEmail} with Name: {name}. Message: {checkPreExisting.Message}";
                    ULLogger.LogError($"A User List does not exist for User with Email: {userEmail} with Name: {name}. Message: {checkPreExisting.Message}");
                    return response;
                }

                var userList = await Context.UserLists
                        .Include(ul => ul.Icon)
                        .Include(ul => ul.ListItems)
                        .Where(ul => ul.UserId == user.Id &&
                                ul.Name == name)
                        .FirstOrDefaultAsync();

                userList.DeletedDate = DateTime.Today;
                userList.Status = Status.deleted;

                Context.UserLists.Update(userList);
                await Context.SaveChangesAsync();

                response.Data = new UserListReadDTO(userList);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to delete User List with Name: {name} for User with Email: {userEmail}.";
                ULLogger.LogError($"An error occured while attempting to delete User List with Name: {name} for User with Email: {userEmail}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully deleted User List with Name: {name} for User with Email: {userEmail}.";
            return response;
        }

        public async Task<ResponseModel<UserListReadDTO>> Delete(string userEmail, string name)
        {
            ResponseModel<UserListReadDTO> response = new();

            try
            {
                var user = await Context.Users
                            .Where(u => u.NormalizedEmail == userEmail.ToUpperInvariant() &&
                                    u.Status == Status.active)
                            .FirstOrDefaultAsync();

                if (user == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find a User with the specified email address.";
                    ULLogger.LogError($"Could not find a User with Email: {userEmail}.");
                    return response;
                }

                ResponseModel<UserListReadDTO> checkPreExisting = await RetrieveByUserAndName(userEmail, name);

                if (!checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"A User List does not exist for User with Email: {userEmail} with Name: {name}. Message: {checkPreExisting.Message}";
                    ULLogger.LogError($"A User List does not exist for User with Email: {userEmail} with Name: {name}. Message: {checkPreExisting.Message}");
                    return response;
                }

                var userList = await Context.UserLists
                        .Include(ul => ul.Icon)
                        .Include(ul => ul.ListItems)
                        .Where(ul => ul.UserId == user.Id &&
                                ul.Name == name)
                        .FirstOrDefaultAsync();

                if (userList.Status != Status.deleted)
                {
                    response.Success = false;
                    response.Message = $"User List permanent deletion failed, not marked for deletion.";
                    ULLogger.LogError($"User List status was not set to deleted when attempting to delete permanently.");
                    return response;
                }

                Context.UserLists.Remove(userList);
                await Context.SaveChangesAsync();

                response.Data = new UserListReadDTO(userList);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to permanently delete User List with Name: {name} for User with Email: {userEmail}.";
                ULLogger.LogError($"An error occured while attempting to permanently delete User List with Name: {name} for User with Email: {userEmail}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully permanently deleted User List with Name: {name} for User with Email: {userEmail}.";
            return response;
        }
    }
}