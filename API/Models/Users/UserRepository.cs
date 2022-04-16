using KitchenManager.API.Data;
using KitchenManager.API.SharedNS.ClaimTypesNS;
using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.SharedNS.StatusNS;
using KitchenManager.API.UsersNS.DTO;
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
        Task<ResponseModel<List<UserReadDTO>>> RetrieveByClaim(string claimType, string claimValue); //admin only
        Task<ResponseModel<List<UserReadDTO>>> RetrieveAll(); //admin only
        /*
        //calls send email confirmation. Before user is authorized, their email must be confirmed.
        Task<ResponseModel<UserReadDTO>> Create(UserCreateDTO model);
        Task<ResponseModel<UserReadDTO>> Update(string emailAddress, UserCreateUpdateDTO model); //wont change name, email, or status
        Task<ResponseModel<UserReadDTO>> UpdateName(string emailAddress, string newFirstName, string newLastName);
        //will send email to user's email address asking to confirm.
        Task<ResponseModel<UserReadDTO>> SendEmailConfirmation(string emailAddress);
        //Never called directly, only accessed by the sent confirmation email link.
        Task<ResponseModel<UserReadDTO>> ConfirmEmailAddress(string emailAddress);
        //will send email to user's new email address asking to confirm.
        Task<ResponseModel<UserReadDTO>> SendUpdateEmailConfirmation(string originalEmailAddress, string newEmailAddress);
        //Never called directly, only accessed by the sent confirmation update email link.
        Task<ResponseModel<UserReadDTO>> UpdateEmailAddress(string originalEmailAddress, string newEmailAddress);
        Task<ResponseModel<UserReadDTO>> SetActiveStatus(string emailAddress);
        Task<ResponseModel<UserReadDTO>> SetDeletedStatus(string emailAddress);
        Task<ResponseModel<UserReadDTO>> Delete(string emailAddress);
        */
    }

    public class UserRepository : IUserRepository
    {
        private readonly KMDbContext Context;
        private readonly ILogger<IUserRepository> ULogger;

        public UserRepository(KMDbContext context, ILogger<IUserRepository> uLogger)
        {
            Context = context;
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

                response.Data =
                    new UserReadDTO(
                        user, // user

                        Context.UserClaims // first name
                        .Where(uc => uc.UserId == user.Id &&
                                uc.ClaimType == KMClaimTypes.FirstName)
                        .FirstOrDefault()
                        .ClaimValue,

                        Context.UserClaims // last name
                        .Where(uc => uc.UserId == user.Id &&
                                uc.ClaimType == KMClaimTypes.LastName)
                        .FirstOrDefault()
                        .ClaimValue
                    );
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
                var user = await Context
                        .Users
                        .Where(u => u.NormalizedEmail == emailAddress.ToUpperInvariant())
                        .FirstOrDefaultAsync();

                if (user is null)
                {
                    response.Success = false;
                    response.Message = $"Could not find User with Email: {emailAddress}.";
                    ULogger.LogError($"Could not find User with Email: {emailAddress}.");
                    return response;
                }

                response.Data = 
                    new UserReadDTO(
                        user, // user

                        Context.UserClaims // first name
                        .Where(uc => uc.UserId == user.Id &&
                                uc.ClaimType == KMClaimTypes.FirstName)
                        .FirstOrDefault()
                        .ClaimValue,

                        Context.UserClaims // last name
                        .Where(uc => uc.UserId == user.Id &&
                                uc.ClaimType == KMClaimTypes.LastName)
                        .FirstOrDefault()
                        .ClaimValue
                    );
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

                response.Data = users.Select(user => 
                    new UserReadDTO(
                        user, // user

                        Context.UserClaims // first name
                        .Where(uc => uc.UserId == user.Id &&
                                uc.ClaimType == KMClaimTypes.FirstName)
                        .FirstOrDefault()
                        .ClaimValue,

                        Context.UserClaims // last name
                        .Where(uc => uc.UserId == user.Id &&
                                uc.ClaimType == KMClaimTypes.LastName)
                        .FirstOrDefault()
                        .ClaimValue
                    )).ToList();
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

        public async Task<ResponseModel<List<UserReadDTO>>> RetrieveByClaim(string claimType, string claimValue)
        {
            ResponseModel<List<UserReadDTO>> response = new();

            try
            {
                var users = await Context
                        .Users
                        .Where( user => Context.UserClaims
                                .Any(uc => uc.UserId == user.Id &&
                                        uc.ClaimType == claimType &&
                                        uc.ClaimValue == claimValue))
                        .ToListAsync();

                if (!users.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any Users with Claim of Type: {claimType} and Value: {claimValue}.";
                    ULogger.LogError($"Could not find any Users with Claim of Type: {claimType} and Value: {claimValue}.");
                    return response;
                }

                response.Data = users.Select(user =>
                    new UserReadDTO(
                        user, // user

                        Context.UserClaims // first name
                        .Where(uc => uc.UserId == user.Id &&
                                uc.ClaimType == KMClaimTypes.FirstName)
                        .FirstOrDefault()
                        .ClaimValue,

                        Context.UserClaims // last name
                        .Where(uc => uc.UserId == user.Id &&
                                uc.ClaimType == KMClaimTypes.LastName)
                        .FirstOrDefault()
                        .ClaimValue
                    )).ToList();
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find Users with Claim of Type: {claimType} and Value: {claimValue}.";
                ULogger.LogError($"An error occured while attempting to find Users with Claim of Type: {claimType} and Value: {claimValue}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved {response.Data.Count()} Users with Claim of Type: {claimType} and Value: {claimValue}.";
            return response;
        }

        public async Task<ResponseModel<List<UserReadDTO>>> RetrieveAll()
        {
            ResponseModel<List<UserReadDTO>> response = new();

            try
            {
                var users = await Context
                        .Users
                        .ToListAsync();

                if (!users.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any Users.";
                    ULogger.LogError($"Could not find any Users.");
                    return response;
                }

                response.Data = users.Select(user =>
                    new UserReadDTO(
                        user, // user

                        Context.UserClaims // first name
                        .Where(uc => uc.UserId == user.Id &&
                                uc.ClaimType == KMClaimTypes.FirstName)
                        .FirstOrDefault()
                        .ClaimValue,

                        Context.UserClaims // last name
                        .Where(uc => uc.UserId == user.Id &&
                                uc.ClaimType == KMClaimTypes.LastName)
                        .FirstOrDefault()
                        .ClaimValue
                    )).ToList();
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find Users.";
                ULogger.LogError($"An error occured while attempting to find Users.");
                return response;
            }

            response.Message = $"Successfully retrieved {response.Data.Count()} Users.";
            return response;
        }

    }
}
