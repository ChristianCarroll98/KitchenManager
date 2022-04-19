using FluentEmail.Core;
using KitchenManager.API.Data;
using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.UsersNS;
using KitchenManager.API.UsersNS.DTO;
using KitchenManager.API.UsersNS.Repo;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace KitchenManager.API.UserAuthNS.Repo
{
    public interface IUserAuthenticationRepository
    {
        Task<ResponseModel<UserReadDTO>> LogInUser(string emailAddress, string password);
        Task<ResponseModel<UserReadDTO>> LogOutUser();

        Task<ResponseModel<string>> SendTestEmail();

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
    }

    public class UserAuthenticationRepository : IUserAuthenticationRepository
    {
        private readonly KMDbContext Context;
        private readonly UserManager<UserModel> UserManager;
        private readonly ILogger<IUserRepository> UALogger;
        private readonly IFluentEmail Email;

        public UserAuthenticationRepository(KMDbContext context, UserManager<UserModel> userManager, ILogger<IUserRepository> uALogger, IFluentEmail email)
        {
            Context = context;
            UserManager = userManager;
            UALogger = uALogger;
            Email = email;
        }

        public Task<ResponseModel<UserReadDTO>> LogInUser(string emailAddress, string password)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<UserReadDTO>> LogOutUser()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<string>> SendTestEmail()
        {
            ResponseModel<string> response = new();

            try
            {
                var result = await Email
                    .To("test@test.test")
                    .Subject("test email subject")
                    .Body("This is the email body")
                    .SendAsync();

                if (!result.Successful)
                {
                    response.Success = false;
                    response.Message = $"Sending test email unsuccessful.";
                    UALogger.LogError($"Sending test email unsuccessful. Messages: {string.Join("; ", result.ErrorMessages)}");
                    return response;
                }
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to send test email.";
                UALogger.LogError($"An error occured while attempting to send test email. Message: {ex.Message}");
                return response;
            }

            response.Message = "Successfully sent test email.";
            return response;
        }
    }
}
