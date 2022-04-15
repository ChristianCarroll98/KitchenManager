using KitchenManager.API.ItemsNS.ListItemsNS.DTO;
using KitchenManager.API.ItemsNS.ListItemsNS.Repo;
using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.SharedNS.StatusNS;
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
        public ActionResult<ResponseModel<ListItemCreateUpdateDTO>> RetrieveById(int id)
        {
            try
            {
                return Ok(LIRepo.RetrieveById(id).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to retrieve List Item with Id: {id}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve List Item with the specified Id.");
            }
        }

        [Route("RetrieveByUserListAndBrand")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<List<ListItemCreateUpdateDTO>>> RetrieveByUserListAndBrand(string userEmail, string userListName, string brand)
        {
            try
            {
                return Ok(LIRepo.RetrieveByUserListAndBrand(userEmail, userListName, brand).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to retrieve Items on list of User with Email: {userEmail} named: {userListName} with Brand: {brand}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve List Items with Brand: {brand}");
            }
        }

        [Route("RetrieveByUserListAndNameAndBrand")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<ListItemCreateUpdateDTO>> RetrieveByUserListAndNameAndBrand(string userEmail, string userListName, string name, string brand)
        {
            try
            {
                return Ok(LIRepo.RetrieveByUserListAndNameAndBrand(userEmail, userListName, name, brand).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to retrieve Item on list of User with Email: {userEmail} named: {userListName} with Name: {name} and Brand: {brand}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve List Item with Name: {name} and Brand: {brand}. Message: {ex.Message}");
            }
        }

        [Route("RetrieveByUserListAndStatus")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<List<ListItemCreateUpdateDTO>>> RetrieveByUserListAndStatus(string userEmail, string userListName, Status status)
        {
            try
            {
                return Ok(LIRepo.RetrieveByUserListAndStatus(userEmail, userListName, status).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to retrieve Items on list of User with Email: {userEmail} named: {userListName} with status: {status}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve List Items with the specified status.");
            }
        }

        [Route("RetrieveByUserListAndTags")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<ListItemCreateUpdateDTO>> RetrieveByUserListAndTags(string userEmail, string userListName, [FromQuery] List<string> itemTags)
        {
            try
            {
                return Ok(LIRepo.RetrieveByUserListAndTags(userEmail, userListName, itemTags).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to retrieve Items on list of User with Email: {userEmail} named: {userListName} with tags: {itemTags.ToString()}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve List Items with the specified tags.");
            }
        }

        [Route("RetrieveByUserList")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<List<ListItemCreateUpdateDTO>>> RetrieveByUserList(string userEmail, string userListName)
        {
            try
            {
                return Ok(LIRepo.RetrieveByUserList(userEmail, userListName).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to retrieve Items on list of User with Email: {userEmail} named: {userListName}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve List Items.");
            }
        }

        [Route("Create")]
        [HttpPost]
        public IActionResult Create(string userEmail, string userListName, ListItemCreateUpdateDTO model)
        {
            try
            {
                return Ok(LIRepo.Create(userEmail, userListName, model).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to create new Item on list of User with Email: {userEmail} named: {userListName} with Name: {model.Name} and Brand: {model.Brand}. Message: {ex}");
                return BadRequest($"Failed to create new List Item with Name: {model.Name} and Brand: {model.Brand}.");
            }
        }

        [Route("CreateFromItemTemplate")]
        [HttpPost]
        public IActionResult CreateFromItemTemplate(string userEmail, string userListName, string itemTemplateName, string itemTemplateBrand)
        {
            try
            {
                return Ok(LIRepo.CreateFromItemTemplate(userEmail, userListName, itemTemplateName, itemTemplateBrand).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to create new Item from Template on list of User with Email: {userEmail} named: {userListName} with Name: {itemTemplateName} and Brand: {itemTemplateBrand}. Message: {ex}");
                return BadRequest($"Failed to create new List Item from Template with Name: {itemTemplateName} and Brand: {itemTemplateBrand}.");
            }
        }

        [Route("Update")]
        [HttpPost]
        public IActionResult Update(string userEmail, string userListName, string originalName, string originalBrand, ListItemCreateUpdateDTO model)
        {
            try
            {
                return Ok(LIRepo.Update(userEmail, userListName, originalName, originalBrand, model).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to update Item on list of User with Email: {userEmail} named: {userListName} with original Name: {originalName} and original Brand: {originalBrand}. Message: {ex}");
                return BadRequest($"Failed to update List Item with original Name: {originalName} and original Brand: {originalBrand}.");
            }
        }

        [Route("SetQuantity")]
        [HttpPost]
        public IActionResult SetQuantity(string userEmail, string userListName, string name, string brand, int quantity)
        {
            try
            {
                return Ok(LIRepo.SetQuantity(userEmail, userListName, name, brand, quantity).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to set quantity of Items on list of User with Email: {userEmail} named: {userListName} with Name: {name} and Brand: {brand}. Message: {ex}");
                return BadRequest($"Failed to set quantity of Items with Name: {name} and Brand: {brand}.");
            }
        }

        [Route("SetActiveStatus")]
        [HttpPost]
        public IActionResult SetActiveStatus(string userEmail, string userListName, string name, string brand)
        {
            try
            {
                return Ok(LIRepo.SetActiveStatus(userEmail, userListName, name, brand).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to set active status for Item on list of User with Email: {userEmail} named: {userListName} with Name: {name} and Brand: {brand}. Message: {ex}");
                return BadRequest($"Failed to activate List Item with Name: {name} and Brand: {brand}");
            }
        }

        [Route("SetDeletedStatus")]
        [HttpPost]
        public IActionResult SetDeletedStatus(string userEmail, string userListName, string name, string brand)
        {
            try
            {
                return Ok(LIRepo.SetDeletedStatus(userEmail, userListName, name, brand).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to set delete status for Item on list of User with Email: {userEmail} named: {userListName} with Name: {name} and Brand: {brand}. Message: {ex}");
                return BadRequest($"Failed to delete List Item with Name: {name} and Brand: {brand}");
            }
        }

        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(string userEmail, string userListName, string name, string brand)
        {
            try
            {
                return Ok(LIRepo.Delete(userEmail, userListName, name, brand).Result);
            }
            catch (Exception ex)
            {
                LILogger.LogError($"Failed to permanently delete Item on list of User with Email: {userEmail} named: {userListName} with Name: {name} and Brand: {brand}. Message: {ex}");
                return BadRequest($"Failed to permanently delete List Item with Name: {name} and Brand: {brand}");
            }
        }
    }
}