using KitchenManager.API.Data;
using KitchenManager.API.SharedNS.ClaimsNS;
using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.SharedNS.StatusNS;
using KitchenManager.API.UsersNS.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.API.UsersNS.Repo
{
    public interface IUserRepository
    {
        Task<ResponseModel<UserReadDTO>> RetrieveById(int id);
        Task<ResponseModel<UserReadDTO>> RetrieveByEmailAddress(string emailAddress);
        Task<ResponseModel<List<UserReadDTO>>> RetrieveByStatus(Status status); //admin only
        Task<ResponseModel<List<UserReadDTO>>> RetrieveByRole(string role); //admin only
        Task<ResponseModel<List<UserReadDTO>>> RetrieveAll(); //admin only
        
        //calls send email confirmation. Before user is authorized, their email must be confirmed.
        Task<ResponseModel<UserReadDTO>> Create(UserCreateDTO model);
        Task<ResponseModel<UserReadDTO>> UpdateInfo(UserUpdateInfoDTO model); //wont change name, email, or status
        Task<ResponseModel<UserReadDTO>> AssignRole(string emailAddress, string role);
        //will send email to user's email address asking to confirm.
        //Task<ResponseModel<UserReadDTO>> SendEmailConfirmation(string emailAddress);
        //Never called directly, only accessed by the sent confirmation email link.
        //Task<ResponseModel<UserReadDTO>> ConfirmEmailAddress(string emailAddress);
        //will send email to user's new email address asking to confirm.
        //Task<ResponseModel<UserReadDTO>> SendUpdateEmailConfirmation(string originalEmailAddress, string newEmailAddress);
        //Never called directly, only accessed by the sent confirmation update email link.
        //Task<ResponseModel<UserReadDTO>> UpdateEmailAddress(string originalEmailAddress, string newEmailAddress); requires token, dont know how to do this yet.
        //Task<ResponseModel<UserReadDTO>> RecoverEmailAddressWithPhoneNumber(string originalEmailAddress, string newEmailAddress, string phoneNumber);
        //Task<ResponseModel<UserReadDTO>> UpdatePhoneNumber(string originalEmailAddress, string newPhoneNumber); same as update email
        Task<ResponseModel<UserReadDTO>> SetActiveStatus(string emailAddress);
        Task<ResponseModel<UserReadDTO>> SetDeletedStatus(string emailAddress);
        Task<ResponseModel<UserReadDTO>> Delete(string emailAddress);
    }

    public class UserRepository : IUserRepository
    {
        private readonly KMDbContext Context;
        private readonly UserManager<UserModel> UserManager;
        private readonly ILogger<IUserRepository> ULogger;

        public UserRepository(KMDbContext context, UserManager<UserModel> userManager, ILogger<IUserRepository> uLogger)
        {
            Context = context;
            UserManager = userManager;
            ULogger = uLogger;
        }

        public async Task<ResponseModel<UserReadDTO>> RetrieveById(int id)
        {
            ResponseModel<UserReadDTO> response = new();

            try
            {
                var user = await Context
                        .Users
                        .Where(u => u.Id == id)
                        .FirstOrDefaultAsync();

                if (user is null)
                {
                    response.Success = false;
                    response.Message = $"Could not find User with the specified Id.";
                    ULogger.LogError($"Could not find User with Id: {id}");
                    return response;
                }

                response.Data = new UserReadDTO(user);
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find User with the specified Id.";
                ULogger.LogError($"An error occured while attempting to find User with Id: {id}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved User with the specified Id.";
            return response;
        }

        public async Task<ResponseModel<UserReadDTO>> RetrieveByEmailAddress(string emailAddress)
        {
            ResponseModel<UserReadDTO> response = new();

            try
            {
                var user = await UserManager.FindByEmailAsync(emailAddress);

                if (user is null)
                {
                    response.Success = false;
                    response.Message = $"Could not find User with Email: {emailAddress}.";
                    ULogger.LogError($"Could not find User with Email: {emailAddress}.");
                    return response;
                }

                response.Data = new UserReadDTO(user);
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find User with Email: {emailAddress}.";
                ULogger.LogError($"An error occured while attempting to find User with Email: {emailAddress}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved User with Email: {emailAddress}.";
            return response;
        }
        
        public async Task<ResponseModel<List<UserReadDTO>>> RetrieveByStatus(Status status)
        {
            ResponseModel<List<UserReadDTO>> response = new();

            try
            {
                var users = await Context
                        .Users
                        .Where(u => u.Status == status)
                        .ToListAsync();

                if (!users.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any Users with status: {status.ToString()}.";
                    ULogger.LogError($"Could not find any Users with status: {status.ToString()}.");
                    return response;
                }

                response.Data = users.Select(user => new UserReadDTO(user)).ToList();
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find Users with status: {status.ToString()}.";
                ULogger.LogError($"An error occured while attempting to find Users with status: {status.ToString()}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved {response.Data.Count()} Users with status: {status.ToString()}.";
            return response;
        }

        public async Task<ResponseModel<List<UserReadDTO>>> RetrieveByRole(string role)
        {
            ResponseModel<List<UserReadDTO>> response = new();

            try
            {
                //this gets a list of strings of the actual property names in code, not values.
                //var properties = typeof(KMRoleClaimValues).GetMembers().Where(p => p.MemberType == System.Reflection.MemberTypes.Field).Select(p => p.Name).ToList();
                
                var roles = new List<string>();
                var users = new List<UserModel>();

                if (role == string.Empty || role is null)
                {
                    users = await Context
                                .Users
                                .Where(user => !Context.UserClaims
                                       .Any(uc => uc.UserId == user.Id &&
                                               uc.ClaimType == KMClaimTypes.Role) &&
                                       user.Status == Status.active)
                                .ToListAsync();
                }
                else
                {
                    //this gets list of strings of the values of the properties.
                    roles = typeof(KMRoleClaimValues).GetFields().Select(f => f.GetValue(f).ToString().ToUpperInvariant()).ToList();

                    if (!roles.Contains(role.ToUpperInvariant()))
                    {
                        response.Success = false;
                        response.Message = $"Invalid Role Name: {role}.";
                        ULogger.LogError($"Invalid Role Name: {role}.");
                        return response;
                    }

                    users = await Context
                            .Users
                            .Where(user => Context.UserClaims
                                   .Any(uc => uc.UserId == user.Id &&
                                           uc.ClaimType == KMClaimTypes.Role &&
                                           uc.ClaimValue == role) &&
                                   user.Status == Status.active)
                            .ToListAsync();
                }

                if (!users.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any Users with Role: {role}.";
                    ULogger.LogError($"Could not find any Users with Role: {role}.");
                    return response;
                }

                response.Data = users.Select(user => new UserReadDTO(user)).ToList();
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find Users with Role: {role}.";
                ULogger.LogError($"An error occured while attempting to find Users with Role: {role}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved {response.Data.Count()} Users with Role: {role}.";
            return response;
        }

        public async Task<ResponseModel<List<UserReadDTO>>> RetrieveAll()
        {
            ResponseModel<List<UserReadDTO>> response = new();

            try
            {
                var users = await Context
                        .Users
                        .Where(user => user.Status == Status.active)
                        .ToListAsync();

                if (!users.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any Users.";
                    ULogger.LogError($"Could not find any Users.");
                    return response;
                }

                response.Data = users.Select(user => new UserReadDTO(user)).ToList();
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find Users.";
                ULogger.LogError($"An error occured while attempting to find Users. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved {response.Data.Count()} Users.";
            return response;
        }

        public async Task<ResponseModel<UserReadDTO>> Create(UserCreateDTO model)
        {
            var response = new ResponseModel<UserReadDTO>();

            try
            {
                ResponseModel<UserReadDTO> checkPreExisting = await RetrieveByEmailAddress(model.Email);

                if (checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"A User already exists with Email: {model.Email}. Message: {checkPreExisting.Message}";
                    response.Data = checkPreExisting.Data;
                    ULogger.LogError($"A User already exists with Email: {model.Email}. Message: {checkPreExisting.Message}");
                    return response;
                }

                //split name to first name and last name
                var nameArr = model.FullName.Split(' ');
                var firstName = nameArr[0];
                var lastName = string.Join(' ', nameArr.TakeLast(nameArr.Length - 1));

                var user = new UserModel()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = model.Email,
                    UserName = model.Email,
                    NormalizedEmail = model.Email.ToUpperInvariant(),
                    NormalizedUserName = model.Email.ToUpperInvariant(),
                    PhoneNumber = model.PhoneNumber,
                    Birthday = model.Birthday
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                await Context.SaveChangesAsync();

                ResponseModel<UserReadDTO> checkAdded = await RetrieveByEmailAddress(model.Email);

                if (!checkAdded.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to save the new User. Message: {checkAdded.Message}";
                    ULogger.LogError($"Failed to save new User with Email: {model.Email} to Database. Message: {checkAdded.Message} Errors: {string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"))}");
                    return response;
                }

                var newUser = await UserManager.FindByEmailAsync(model.Email);

                var roles = typeof(KMRoleClaimValues).GetFields().Select(f => f.GetValue(f).ToString().ToUpperInvariant()).ToList();

                if (!roles.Contains(model.Role.ToUpperInvariant()))
                {
                    response.Success = false;
                    response.Message = $"Invalid Role Name: {model.Role}. Role unassigned.";
                    ULogger.LogError($"Invalid Role Name: {model.Role}. Role unassigned.");
                    return response;
                }

                if (model.Role == KMRoleClaimValues.SuperAdmin)
                {
                    await Context.UserClaims.AddAsync(new IdentityUserClaim<int>()
                    {
                        UserId = newUser.Id,
                        ClaimType = KMClaimTypes.Role,
                        ClaimValue = KMRoleClaimValues.SuperAdmin
                    });
                }

                if (model.Role == KMRoleClaimValues.SuperAdmin || model.Role == KMRoleClaimValues.Admin)
                {
                    await Context.UserClaims.AddAsync(new IdentityUserClaim<int>()
                    {
                        UserId = newUser.Id,
                        ClaimType = KMClaimTypes.Role,
                        ClaimValue = KMRoleClaimValues.Admin
                    });
                }

                await Context.UserClaims.AddAsync(new IdentityUserClaim<int>()
                {
                    UserId = newUser.Id,
                    ClaimType = KMClaimTypes.Role,
                    ClaimValue = KMRoleClaimValues.User
                });

                await Context.SaveChangesAsync();

                response.Data = checkAdded.Data;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to create a new User with Email: {model.Email}.";
                ULogger.LogError($"An error occured while attempting to create a new User with Email: {model.Email}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully created a new User with Email: {model.Email}.";
            return response;
        }

        public async Task<ResponseModel<UserReadDTO>> UpdateInfo(UserUpdateInfoDTO model)
        {
            var response = new ResponseModel<UserReadDTO>();

            try
            {
                ResponseModel<UserReadDTO> checkPreExisting = await RetrieveByEmailAddress(model.Email);

                if (!checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"A User does not exist with Email: {model.Email}. Message: {checkPreExisting.Message}";
                    ULogger.LogError($"A User does not exist with Email: {model.Email}. Message: {checkPreExisting.Message}");
                    return response;
                }

                //split name to first name and last name
                var nameArr = model.Name.Split(' ');
                var firstName = nameArr[0];
                var lastName = string.Join(' ', nameArr.TakeLast(nameArr.Length - 1));

                var updatedUser = await UserManager.FindByEmailAsync(model.Email);

                updatedUser.FirstName = firstName;
                updatedUser.LastName = lastName;
                updatedUser.Birthday = model.Birthday;

                await UserManager.UpdateAsync(updatedUser);
                await Context.SaveChangesAsync();

                response.Data = new UserReadDTO(updatedUser);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to update information of User with Email: {model.Email}.";
                ULogger.LogError($"An error occured while attempting to update information of User with Email: {model.Email}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully updated information of User with Email: {model.Email}.";
            return response;
        }

        public async Task<ResponseModel<UserReadDTO>> AssignRole(string emailAddress, string role)
        {
            ///TODO not changing role to admin?
            var response = new ResponseModel<UserReadDTO>();

            try
            {
                ResponseModel<UserReadDTO> checkPreExisting = await RetrieveByEmailAddress(emailAddress);

                if (!checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"A User does not exist with Email: {emailAddress}. Message: {checkPreExisting.Message}";
                    ULogger.LogError($"A User does not exist with Email: {emailAddress}. Message: {checkPreExisting.Message}");
                    return response;
                }

                var user = await UserManager.FindByEmailAsync(emailAddress);

                var roles = typeof(KMRoleClaimValues).GetFields().Select(f => f.GetValue(f).ToString().ToUpperInvariant()).ToList();

                if (role == string.Empty || role is null || !roles.Contains(role.ToUpperInvariant()))
                {
                    response.Success = false;
                    response.Message = $"Invalid Role Name: {role}. Role unchanged.";
                    ULogger.LogError($"Invalid Role Name: {role}. Role unchanged.");
                    return response;
                }

                //clear roles
                Context.UserClaims.RemoveRange(Context.UserClaims.Where(uc => uc.ClaimType == KMClaimTypes.Role && uc.UserId == user.Id));

                await Context.SaveChangesAsync();

                if (role == KMRoleClaimValues.SuperAdmin)
                {
                    await Context.UserClaims.AddAsync(new IdentityUserClaim<int>()
                    {
                        UserId = user.Id,
                        ClaimType = KMClaimTypes.Role,
                        ClaimValue = KMRoleClaimValues.SuperAdmin
                    });
                }

                if (role == KMRoleClaimValues.SuperAdmin || role == KMRoleClaimValues.Admin)
                {
                    await Context.UserClaims.AddAsync(new IdentityUserClaim<int>()
                    {
                        UserId = user.Id,
                        ClaimType = KMClaimTypes.Role,
                        ClaimValue = KMRoleClaimValues.Admin
                    });
                }

                await Context.UserClaims.AddAsync(new IdentityUserClaim<int>()
                {
                    UserId = user.Id,
                    ClaimType = KMClaimTypes.Role,
                    ClaimValue = KMRoleClaimValues.User
                });

                await Context.SaveChangesAsync();

                response.Data = new UserReadDTO(user);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to assign role {role} to User with Email: {emailAddress}.";
                ULogger.LogError($"An error occured while attempting to assign role {role} to User with Email: {emailAddress}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully assigned role {role} to User with Email: {emailAddress}.";
            return response;
        }

        /*public async Task<ResponseModel<UserReadDTO>> UpdateEmailAddress(string originalEmailAddress, string newEmailAddress)
        {
            var response = new ResponseModel<UserReadDTO>();

            try
            {
                ResponseModel<UserReadDTO> checkPreExisting = await RetrieveByEmailAddress(originalEmailAddress);

                if (!checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"A User does not exist with original Email: {originalEmailAddress}. Message: {checkPreExisting.Message}";
                    ULogger.LogError($"A User does not exist with original Email: {originalEmailAddress}. Message: {checkPreExisting.Message}");
                    return response;
                }

                var updatedUser = await UserManager.FindByEmailAsync(originalEmailAddress);

                updatedUser.Email = newEmailAddress;
                updatedUser.NormalizedEmail = newEmailAddress.ToUpperInvariant();

                await UserManager.UpdateAsync(updatedUser);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to update email Email of User with original Email: {originalEmailAddress}.";
                ULogger.LogError($"An error occured while attempting to update Email of User with original Email: {originalEmailAddress}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully updated Email of User with original Email: {originalEmailAddress}.";
            return response;
        }

        public async Task<ResponseModel<UserReadDTO>> UpdatePhoneNumber(string emailAddress, string newPhoneNumber)
        {
            var response = new ResponseModel<UserReadDTO>();

            try
            {
                ResponseModel<UserReadDTO> checkPreExisting = await RetrieveByEmailAddress(emailAddress);

                if (!checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"A User does not exist with Email: {emailAddress}. Message: {checkPreExisting.Message}";
                    ULogger.LogError($"A User does not exist with Email: {emailAddress}. Message: {checkPreExisting.Message}");
                    return response;
                }

                var updatedUser = await UserManager.FindByEmailAsync(emailAddress);

                updatedUser.PhoneNumber = newPhoneNumber;

                await UserManager.UpdateAsync(updatedUser);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to update Phone Number of User with Email: {emailAddress}.";
                ULogger.LogError($"An error occured while attempting to update Phone Number of User with Email: {emailAddress}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully updated Phone Number of User with Email: {emailAddress}.";
            return response;
        }*/

        public async Task<ResponseModel<UserReadDTO>> SetActiveStatus(string emailAddress)
        {
            var response = new ResponseModel<UserReadDTO>();

            try
            {
                ResponseModel<UserReadDTO> checkPreExisting = await RetrieveByEmailAddress(emailAddress);

                if (!checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"A User does not exist with Email: {emailAddress}. Message: {checkPreExisting.Message}";
                    ULogger.LogError($"A User does not exist with Email: {emailAddress}. Message: {checkPreExisting.Message}");
                    return response;
                }

                var updatedUser = await UserManager.FindByEmailAsync(emailAddress);

                updatedUser.Status = Status.active;

                await UserManager.UpdateAsync(updatedUser);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to activate account of User with Email: {emailAddress}.";
                ULogger.LogError($"An error occured while attempting to activate account of User with Email: {emailAddress}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully activated account of User with Email: {emailAddress}.";
            return response;
        }

        public async Task<ResponseModel<UserReadDTO>> SetDeletedStatus(string emailAddress)
        {
            var response = new ResponseModel<UserReadDTO>();

            try
            {
                ResponseModel<UserReadDTO> checkPreExisting = await RetrieveByEmailAddress(emailAddress);

                if (!checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"A User does not exist with Email: {emailAddress}. Message: {checkPreExisting.Message}";
                    ULogger.LogError($"A User does not exist with Email: {emailAddress}. Message: {checkPreExisting.Message}");
                    return response;
                }

                var updatedUser = await UserManager.FindByEmailAsync(emailAddress);

                updatedUser.Status = Status.deleted;

                await UserManager.UpdateAsync(updatedUser);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to delete account of User with Email: {emailAddress}.";
                ULogger.LogError($"An error occured while attempting to delete account of User with Email: {emailAddress}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully deleted account of User with Email: {emailAddress}.";
            return response;
        }

        public async Task<ResponseModel<UserReadDTO>> Delete(string emailAddress)
        {
            var response = new ResponseModel<UserReadDTO>();

            try
            {
                ResponseModel<UserReadDTO> checkPreExisting = await RetrieveByEmailAddress(emailAddress);

                if (!checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"A User does not exist with Email: {emailAddress}. Message: {checkPreExisting.Message}";
                    ULogger.LogError($"A User does not exist with Email: {emailAddress}. Message: {checkPreExisting.Message}");
                    return response;
                }

                var deletedUser = await UserManager.FindByEmailAsync(emailAddress);

                if (deletedUser.Status != Status.deleted)
                {
                    response.Success = false;
                    response.Message = $"Permanent deletion of User with Email: {emailAddress} failed, not marked for deletion.";
                    ULogger.LogError($"User with Email: {emailAddress} status was not set to deleted when attempting to delete permanently.");
                    return response;
                }

                await UserManager.DeleteAsync(deletedUser);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to permanently delete account of User with Email: {emailAddress}.";
                ULogger.LogError($"An error occured while attempting to permanently delete account of User with Email: {emailAddress}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully permanently deleted account of User with Email: {emailAddress}.";
            return response;
        }
    }
}
