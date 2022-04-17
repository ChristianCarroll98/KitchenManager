using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.SharedNS.StatusNS;
using KitchenManager.API.UsersNS.DTO;
using KitchenManager.API.UsersNS.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace KitchenManager.API.UsersNS
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository URepo;
        private readonly ILogger<UserController> ULLogger;

        public UserController(IUserRepository uRepo, ILogger<UserController> uLLogger)
        {
            URepo = uRepo;
            ULLogger = uLLogger;
        }

        [Route("RetrieveById")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<UserReadDTO>> RetrieveById(int id)
        {
            try
            {
                return Ok(URepo.RetrieveById(id).Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to retrieve User with Id: {id}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve User with the specified Id.");
            }
        }

        [Route("RetrieveByEmailAddress")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<UserReadDTO>> RetrieveByEmailAddress(string email)
        {
            try
            {
                return Ok(URepo.RetrieveByEmailAddress(email).Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to retrieve User with Email: {email}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve User with Email: {email}.");
            }
        }

        [Route("RetrieveByStatus")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<UserReadDTO>> RetrieveByStatus(Status status)
        {
            try
            {
                return Ok(URepo.RetrieveByStatus(status).Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to retrieve Users with status: {status.ToString()}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve Users with the specified status.");
            }
        }

        [Route("RetrieveByRole")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<UserReadDTO>> RetrieveByRole(string role)
        {
            try//
            {
                return Ok(URepo.RetrieveByRole(role).Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to retrieve Users with Role: {role}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve Users with Role: {role}.");
            }
        }

        [Route("RetrieveAll")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<UserReadDTO>> RetrieveAll()
        {
            try
            {
                return Ok(URepo.RetrieveAll().Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to retrieve Users. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve Users.");
            }
        }

        [Route("Create")]
        [HttpPost]
        public IActionResult Create(UserCreateDTO model)
        {
            try
            {
                return Ok(URepo.Create(model).Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to create new User with Email: {model.Email}. Message: {ex}");
                return BadRequest($"Failed to create new User with Email: {model.Email}.");
            }
        }

        [Route("UpdateInfo")]
        [HttpPost]
        public IActionResult UpdateInfo(UserUpdateInfoDTO model)
        {
            try
            {
                return Ok(URepo.UpdateInfo(model).Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to update information for User with Email: {model.Email}. Message: {ex}");
                return BadRequest($"Failed to update information for User with Email: {model.Email}.");
            }
        }

        [Route("AssignRole")]
        [HttpPost]
        public IActionResult AssignRole(string emailAddress, string role)
        {
            try
            {
                return Ok(URepo.AssignRole(emailAddress, role).Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to assign role {role} to User with Email: {emailAddress}. Message: {ex}");
                return BadRequest($"Failed to assign role {role} to User with Email: {emailAddress}.");
            }
        }
        
        [Route("SetActiveStatus")]
        [HttpPost]
        public IActionResult SetActiveStatus(string emailAddress)
        {
            try
            {
                return Ok(URepo.SetActiveStatus(emailAddress).Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to activate account for User with Email: {emailAddress}. Message: {ex}");
                return BadRequest($"Failed to activate account for User with Email: {emailAddress}.");
            }
        }

        [Route("SetDeletedStatus")]
        [HttpPost]
        public IActionResult SetDeletedStatus(string emailAddress)
        {
            try
            {
                return Ok(URepo.SetDeletedStatus(emailAddress).Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to delete account for User with Email: {emailAddress}. Message: {ex}");
                return BadRequest($"Failed to delete account for User with Email: {emailAddress}.");
            }
        }

        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(string emailAddress)
        {
            try
            {
                return Ok(URepo.Delete(emailAddress).Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to permanently delete account for User with Email: {emailAddress}. Message: {ex}");
                return BadRequest($"Failed to permanently delete account for User with Email: {emailAddress}.");
            }
        }
    }
}