using KitchenManager.API.ItemsNS.ItemTemplatesNS.DTO;
using KitchenManager.API.ItemsNS.ListItemsNS.DTO;
using KitchenManager.API.ItemsNS.ListItemsNS.Repo;
using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.SharedNS.StatusNS;
using KitchenManager.API.UsersNS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace KitchenManager.API.ItemsNS.ListItemsNS
{
    [ApiController]
    [Route("[controller]")]
    public class ListItemController : ControllerBase
    {
        private readonly IListItemRepository LIRepo;
        private readonly ILogger<ListItemController> LILogger;

        public ListItemController(IListItemRepository lIRepo, ILogger<ListItemController> iTLogger)
        {
            LIRepo = lIRepo;
            LILogger = iTLogger;
        }

        [Route("RetrieveById")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<Response<ListItemDTO>> RetrieveById(int id)
        {
            try
            {
                return Ok(LIRepo.RetrieveById(id).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to retrieve List Item with Id: {id}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve List Item with the specified Id");
            }
        }

        [Route("RetrieveByUserListAndBrand")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<Response<List<ListItemDTO>>> RetrieveByUserListAndBrand(string userName, string userListName, string brand)
        {
            try
            {
                return Ok(LIRepo.RetrieveByUserListAndBrand(userName, userListName, brand).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to retrieve Items on {userName}'s list: {userListName} with Brand: {brand}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve List Items with Brand: {brand}");
            }
        }

        [Route("RetrieveByUserListAndNameAndBrand")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<Response<ListItemDTO>> RetrieveByUserListAndNameAndBrand(string userName, string userListName, string name, string brand)
        {
            try
            {
                return Ok(LIRepo.RetrieveByUserListAndNameAndBrand(userName, userListName, name, brand).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to retrieve Item on {userName}'s list: {userListName} with Name: {name} and Brand: {brand}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve List Item with Name: {name} and Brand: {brand}. Message: {ex.Message}");
            }
        }

        [Route("RetrieveByUserListAndStatus")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<Response<List<ListItemDTO>>> RetrieveByUserListAndStatus(string userName, string userListName, Status status)
        {
            try
            {
                return Ok(LIRepo.RetrieveByUserListAndStatus(userName, userListName, status).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to retrieve Items on {userName}'s list: {userListName} with status: {status}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve List Items with the specified status.");
            }
        }

        [Route("RetrieveByUserListAndTags")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<Response<ListItemDTO>> RetrieveByUserListAndTags(string userName, string userListName, [FromQuery] List<string> itemTags)
        {
            try
            {
                return Ok(LIRepo.RetrieveByUserListAndTags(userName, userListName, itemTags).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to retrieve Items on {userName}'s list: {userListName} with tags: {itemTags.ToString()}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve List Items with the specified tags.");
            }
        }

        [Route("RetrieveByUserList")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<Response<List<ListItemDTO>>> RetrieveByUserList(string userName, string userListName)
        {
            try
            {
                return Ok(LIRepo.RetrieveByUserList(userName, userListName).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to retrieve Items on {userName}'s list: {userListName}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve List Items.");
            }
        }

        [Route("Create")]
        [HttpPost]
        public IActionResult Create(string userName, string userListName, ListItemDTO model)
        {
            try
            {
                return Ok(LIRepo.Create(userName, userListName, model).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to create new Item on {userName}'s list: {userListName} with Name: {model.Name} and Brand: {model.Brand}. Message: {ex}");
                return BadRequest($"Failed to create new List Item with Name: {model.Name} and Brand: {model.Brand}.");
            }
        }

        [Route("CreateFromItemTemplate")]
        [HttpPost]
        public IActionResult CreateFromItemTemplate(string userName, string userListName, ItemTemplateDTO model)
        {
            try
            {
                return Ok(LIRepo.CreateFromItemTemplate(userName, userListName, model).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to create new Item from Template on {userName}'s list: {userListName} with Name: {model.Name} and Brand: {model.Brand}. Message: {ex}");
                return BadRequest($"Failed to create new List Item from Template with Name: {model.Name} and Brand: {model.Brand}.");
            }
        }

        [Route("Update")]
        [HttpPost]
        public IActionResult Update(string userName, string userListName, string originalName, string originalBrand, ListItemDTO model)
        {
            try
            {
                return Ok(LIRepo.Update(userName, userListName, originalName, originalBrand, model).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to update Item on {userName}'s list: {userListName} with original Name: {originalName} and original Brand: {originalBrand}. Message: {ex}");
                return BadRequest($"Failed to update List Item with original Name: {originalName} and original Brand: {originalBrand}.");
            }
        }

        [Route("SetQuantity")]
        [HttpPost]
        public IActionResult SetQuantity(string userName, string userListName, string name, string brand, int quantity)
        {
            try
            {
                return Ok(LIRepo.SetQuantity(userName, userListName, name, brand, quantity).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to set quantity of Items on {userName}'s list: {userListName} with Name: {name} and Brand: {brand}. Message: {ex}");
                return BadRequest($"Failed to set quantity of Items with Name: {name} and Brand: {brand}.");
            }
        }

        [Route("SetActiveStatus")]
        [HttpPost]
        public IActionResult SetActiveStatus(string userName, string userListName, string name, string brand)
        {
            try
            {
                return Ok(LIRepo.SetActiveStatus(userName, userListName, name, brand).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to set active status for List Item on {userName}'s list: {userListName} with Name: {name} and Brand: {brand}. Message: {ex}");
                return BadRequest($"Failed to activate List Item with Name: {name} and Brand: {brand}");
            }
        }

        [Route("SetDeletedStatus")]
        [HttpPost]
        public IActionResult SetDeletedStatus(string userName, string userListName, string name, string brand)
        {
            try
            {
                return Ok(LIRepo.SetDeletedStatus(userName, userListName, name, brand).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to set delete status for List Item on {userName}'s list: {userListName} with Name: {name} and Brand: {brand}. Message: {ex}");
                return BadRequest($"Failed to delete List Item with Name: {name} and Brand: {brand}");
            }
        }

        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(string userName, string userListName, string name, string brand)
        {
            try
            {
                return Ok(LIRepo.Delete(userName, userListName, name, brand).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to permanently delete List Item on {userName}'s list: {userListName} with Name: {name} and Brand: {brand}. Message: {ex}");
                return BadRequest($"Failed to permanently delete List Item with Name: {name} and Brand: {brand}");
            }
        }
    }
}