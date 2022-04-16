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

        [Route("RetrieveByClaim")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<UserReadDTO>> RetrieveByClaim(string claimType, string claimValue)
        {
            try//
            {
                return Ok(URepo.RetrieveByClaim(claimType, claimValue).Result);
            }
            catch (Exception ex)
            {
                ULLogger.LogError($"Failed to retrieve Users with Claim of Type: {claimType} and Value: {claimValue}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve Users with Claim of Type: {claimType} and Value: {claimValue}.");
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
    }
}