using KitchenManager.API.ItemsNS.ItemTemplatesNS.DTO;
using KitchenManager.API.ItemsNS.ItemTemplatesNS.Repo;
using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.SharedNS.StatusNS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace KitchenManager.API.ItemsNS.ItemTemplatesNS
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class ItemTemplateController : ControllerBase
    {
        private readonly IItemTemplateRepository ITRepo;
        private readonly ILogger<ItemTemplateController> ITLogger;

        public ItemTemplateController(IItemTemplateRepository iTRepo, ILogger<ItemTemplateController> iTLogger)
        {
            ITRepo = iTRepo;
            ITLogger = iTLogger;
        }

        [Route("RetrieveById")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<ItemTemplateCreateUpdateDTO>> RetrieveById(int id)
        {
            try
            {
                return Ok(ITRepo.RetrieveById(id).Result);
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to retrieve Item Template with Id: {id}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve Item Template with the specified Id");
            }
        }

        [Route("RetrieveByName")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<ItemTemplateCreateUpdateDTO>> RetrieveByName(string name)
        {
            try
            {
                return Ok(ITRepo.RetrieveByName(name).Result);
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to retrieve Item Templates with Name: {name}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve Item Templates with name: {name}.");
            }
        }

        [Route("RetrieveByBrand")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<ItemTemplateCreateUpdateDTO>> RetrieveByBrand(string brand)
        {
            try
            {
                return Ok(ITRepo.RetrieveByBrand(brand).Result);
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to retrieve Item Templates with Brand: {brand}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve Item Templates with brand: {brand}.");
            }
        }

        [Route("RetrieveByNameAndBrand")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<ItemTemplateCreateUpdateDTO>> RetrieveByNameAndBrand(string name, string brand)
        {
            try
            {
                return Ok(ITRepo.RetrieveByNameAndBrand(name, brand).Result);
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to retrieve Item Template with Name: {name} and Brand: {brand}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve Item Template with Name: {name} and Brand: {brand}. Message: {ex.Message}");
            }
        }

        [Route("RetrieveByStatus")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<ItemTemplateCreateUpdateDTO>> RetrieveByStatus(Status status)
        {
            try
            {
                return Ok(ITRepo.RetrieveByStatus(status).Result);
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to retrieve Item Templates with status: {status}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve Item Templates with the specified status.");
            }
        }

        [Route("RetrieveByItemTags")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<ItemTemplateCreateUpdateDTO>> RetrieveByItemTags([FromQuery]List<string> itemTags)
        {
            try
            {
                return Ok(ITRepo.RetrieveByItemTags(itemTags).Result);
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to retrieve Item Templates with tags: {itemTags.ToString()}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve Item Templates with the specified tags.");
            }
        }

        [Route("RetrieveAll")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ResponseModel<ItemTemplateCreateUpdateDTO>> RetrieveAll()
        {
            try
            {
                return Ok(ITRepo.RetrieveAll().Result);
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to retrieve the list of Item Templates. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve the list of Item Templates.");
            }
        }

        [Route("Create")]
        [HttpPost]
        public IActionResult Create(ItemTemplateCreateUpdateDTO model)
        {
            try
            {
                return Ok(ITRepo.Create(model).Result);
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to create new Item Template with Name: {model.Name} and Brand: {model.Brand}. Message: {ex}");
                return BadRequest($"Failed to create new Item Template with Name: {model.Name} and Brand: {model.Brand}.");
            }
        }

        [Route("Update")]
        [HttpPost]
        public IActionResult Update(ItemTemplateCreateUpdateDTO model, string originalName = null, string originalBrand = null)
        {
            try
            {
                return Ok(ITRepo.Update(model, originalName, originalBrand).Result);
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to update Item Template with original Name: {originalName} and original Brand: {originalBrand}. Message: {ex}");
                return BadRequest($"Failed to update the specified Item Template.");
            }
        }

        [Route("SetActiveStatus")]
        [HttpPost]
        public IActionResult SetActiveStatus(string name, string brand)
        {
            try
            {
                return Ok(ITRepo.SetActiveStatus(name, brand).Result);
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to set active status for Item Template with Name: {name} and Brand: {brand}. Message: {ex}");
                return BadRequest($"Failed to activate Item Template with Name: {name} and Brand: {brand}");
            }
        }

        [Route("SetDeletedStatus")]
        [HttpPost]
        public IActionResult SetDeletedStatus(string name, string brand)
        {
            try
            {
                return Ok(ITRepo.SetDeletedStatus(name, brand).Result);
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to set delete status for Item Template with Name: {name} and Brand: {brand}. Message: {ex}");
                return BadRequest($"Failed to delete Item Template with Name: {name} and Brand: {brand}");
            }
        }

        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(string name, string brand)
        {
            try
            {
                return Ok(ITRepo.Delete(name, brand).Result);
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to permanently delete Item Template with Name: {name} and Brand: {brand}. Message: {ex}");
                return BadRequest($"Failed to permanently delete the specified Item Template.");
            }
        }
    }
}
