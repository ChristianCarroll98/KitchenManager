using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.SharedNS.StatusNS;
using KitchenManager.API.UserListsNS.DTO;
using KitchenManager.API.UserListsNS.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace KitchenManager.API.UserListsNS
{
    [ApiController]
    [Route("[controller]")]
    public class UserListController : ControllerBase
    {
        private readonly IUserListRepository ULRepo;
        private readonly ILogger<UserListController> ULLogger;

        public UserListController(IUserListRepository uLRepo, ILogger<UserListController> uLLogger)
        {
            ULRepo = uLRepo;
            ULLogger = uLLogger;
        }

        [Route("RetrieveById")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<UserListReadDTO>> RetrieveById(int id)
        {
            try
            {
                return Ok(ULRepo.RetrieveById(id).Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to retrieve User List with Id: {id}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve User List with the specified Id.");
            }
        }

        [Route("RetrieveByUserAndName")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<UserListReadDTO>> RetrieveByUserAndName(string userEmail, string name)
        {
            try
            {
                return Ok(ULRepo.RetrieveByUserAndName(userEmail, name).Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to retrieve User List named {name} for User with Email: {userEmail}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve User List named {name} for User with Email: {userEmail}.");
            }
        }

        [Route("RetrieveByUserAndStatus")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<List<UserListReadDTO>>> RetrieveByUserAndStatus(string userEmail, Status status)
        {
            try
            {
                return Ok(ULRepo.RetrieveByUserAndStatus(userEmail, status).Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to retrieve User Lists with status: {status.ToString()} for User with Email: {userEmail}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve User List with status: {status.ToString()} for User with Email: {userEmail}.");
            }
        }

        [Route("RetrieveByUser")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<List<UserListReadDTO>>> RetrieveByUser(string userEmail)
        {
            try
            {
                return Ok(ULRepo.RetrieveByUser(userEmail).Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to retrieve User Lists for User with Email: {userEmail}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve User Lists for User with Email: {userEmail}.");
            }
        }

        [Route("Create")]
        [HttpPost]
        public IActionResult Create(string userEmail, UserListCreateUpdateDTO model)
        {
            try
            {
                return Ok(ULRepo.Create(userEmail, model).Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to create new User List with Name: {model.Name} for User with Email: {userEmail}. Message: {ex}");
                return BadRequest($"Failed to create new User List with Name: {model.Name} for User with Email: {userEmail}");
            }
        }

        [Route("Update")]
        [HttpPost]
        public IActionResult Update(string userEmail, string originalName, UserListCreateUpdateDTO model)
        {
            try
            {
                return Ok(ULRepo.Update(userEmail, originalName, model).Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to update User List with original Name: {originalName} for User with Email: {userEmail}. Message: {ex}");
                return BadRequest($"Failed to update User List with original Name: {originalName} for User with Email: {userEmail}");
            }
        }

        [Route("SetActiveStatus")]
        [HttpPost]
        public IActionResult SetActiveStatus(string userEmail, string name)
        {
            try
            {
                return Ok(ULRepo.SetActiveStatus(userEmail, name).Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to activate User List with Name: {name} for User with Email: {userEmail}. Message: {ex}");
                return BadRequest($"Failed to activate User List with Name: {name} for User with Email: {userEmail}");
            }
        }

        [Route("SetDeletedStatus")]
        [HttpPost]
        public IActionResult SetDeletedStatus(string userEmail, string name)
        {
            try
            {
                return Ok(ULRepo.SetDeletedStatus(userEmail, name).Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to set delete status for User List with Name: {name} for User with Email: {userEmail}. Message: {ex}");
                return BadRequest($"Failed to delete User List with Name: {name} for User with Email: {userEmail}");
            }
        }

        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(string userEmail, string name)
        {
            try
            {
                return Ok(ULRepo.Delete(userEmail, name).Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to permanently delete User List with Name: {name} for User with Email: {userEmail}. Message: {ex}");
                return BadRequest($"Failed to permanently delete User List with Name: {name} for User with Email: {userEmail}");
            }
        }
    }
}