using KitchenManager.API.UserAuthNS.Repo;
using KitchenManager.API.UsersNS.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace KitchenManager.API.UserAuthNS
{
    [ApiController]
    [Route("[controller]")]
    public class UserAuthenticationController : ControllerBase
    {
        private readonly IUserAuthenticationRepository UARepo;
        private readonly ILogger<UserAuthenticationController> UALogger;

        public UserAuthenticationController(IUserAuthenticationRepository uARepo, ILogger<UserAuthenticationController> uALogger)
        {
            UARepo = uARepo;
            UALogger = uALogger;
        }

        

        [Route("SendTestEmail")]
        [HttpPost]
        public IActionResult SendTestEmail()
        {
            try
            {
                return Ok(UARepo.SendTestEmail().Result);
            }
            catch (Exception ex)
            {
                UALogger.LogError($"Failed to send test Email. Message: {ex}");
                return BadRequest($"Failed to send test Email.");
            }
        }
    }
}