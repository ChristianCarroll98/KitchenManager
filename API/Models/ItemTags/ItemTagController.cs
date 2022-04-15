using KitchenManager.API.ItemTagsNS.Repo;
using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.SharedNS.StatusNS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace KitchenManager.API.ItemTagsNS
{
    [ApiController]
    [Route("[controller]")]
    public class ItemTagController : ControllerBase
    {
        private readonly IItemTagRepository ITRepo;
        private readonly ILogger<ItemTagController> ITLogger;

        public ItemTagController(IItemTagRepository iTRepo, ILogger<ItemTagController> iTLogger)
        {
            ITRepo = iTRepo;
            ITLogger = iTLogger;
        }

        [Route("RetrieveByName")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<List<string>>> RetrieveByName(string name)
        {
            try
            {
                return Ok(ITRepo.RetrieveByName(name).Result);
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to retrieve the Item Tag with Name: {name}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve the Item Tag with Name: {name}.");
            }
        }

        [Route("RetrieveByStatus")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<List<string>>> RetrieveByStatus(Status status)
        {
            try
            {
                return Ok(ITRepo.RetrieveByStatus(status).Result);
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to retrieve the list of Item Tag with Status: {status.ToString()}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve the list of Item Tag with Status: {status.ToString()}.");
            }
        }

        [Route("RetrieveAll")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<List<string>>> RetrieveAll()
        {
            try
            {
                return Ok(ITRepo.RetrieveAll().Result);
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to retrieve the list of Item Tags. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve the list of Item Tags.");
            }
        }

        [Route("Create")]
        [HttpPost]
        public IActionResult Create(string name)
        {
            try
            {
                return Ok(ITRepo.Create(name).Result);
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to create new Item Tag with Name: {name}. Message: {ex}");
                return BadRequest($"Failed to create new Item Tag with Name: {name}.");
            }
        }

        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(string name)
        {
            try
            {
                return Ok(ITRepo.Delete(name).Result);
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to delete Item Tag with Name: {name}. Message: {ex}");
                return BadRequest($"Failed to delete Item Tag with Name: {name}.");
            }
        }
    }
}