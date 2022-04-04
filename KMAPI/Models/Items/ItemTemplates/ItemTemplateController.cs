using KitchenManager.KMAPI.Items.ItemTemplates.DTO;
using KitchenManager.KMAPI.Items.ItemTemplates.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace KitchenManager.KMAPI.Items.ItemTemplates
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
        public ActionResult<ItemTemplateResponse> RetrieveById(int id)
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

        [Route("RetrieveByNameAndBrand")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ItemTemplateResponse> RetrieveByNameAndBrand(string name, string brand)
        {
            try
            {
                return Ok(ITRepo.RetrieveByNameAndBrand(name, brand).Result);
            }
            catch (Exception ex)
            {

                return BadRequest($"Failed to retrieve Item Template with Name: {name} and Brand: {brand}. Message: {ex.Message}");
            }
        }

        [Route("RetrieveByStatus")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ItemTemplateListResponse> RetrieveByStatus(ItemStatus status)
        {
            try
            {
                return Ok(ITRepo.RetrieveByStatus(status).Result);
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to retrieve Item Template with status: {status}. Message: {ex.Message}");
                return BadRequest($"Failed to retrieve Item Template with the specified status.");
            }
        }

        [Route("RetrieveAll")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ItemTemplateListResponse> RetrieveAll()
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
        public IActionResult CreateUpdate(ItemTemplateDTO model)
        {
            try
            {
                return Ok(ITRepo.Create(model));
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to create new Item Template with Name: {model.Name} and Brand: {model.Brand}. Message: {ex}");
                return BadRequest($"Failed to create new Item Template with Name: {model.Name} and Brand: {model.Brand}.");
            }
        }

        [Route("Update")]
        [HttpPost]
        public IActionResult CreateUpdate(ItemTemplateDTO model, string originalName = null, string originalBrand = null)
        {
            try
            {
                return Ok(ITRepo.Update(model, originalName, originalBrand));
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to update Item Template with original Name: {originalName} and original Brand: {originalBrand}. Message: {ex}");
                return BadRequest($"Failed to update the specified Item Template.");
            }
        }

        [Route("SetDeleteStatus")]
        [HttpPost]
        public IActionResult SetDeleteStatus(string name, string brand)
        {
            try
            {
                return Ok(ITRepo.SetDeleteStatus(name, brand));
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to set delete status for Item Template with Name: {name} and Brand: {brand}. Message: {ex}");
                return BadRequest($"Failed to delete Item Template with the specified Id");
            }
        }

        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(string name, string brand)
        {
            try
            {
                return Ok(ITRepo.Delete(name, brand));
            }
            catch (Exception ex)
            {
                ITLogger.LogError($"Failed to permanently delete Item Template with Name: {name} and Brand: {brand}. Message: {ex}");
                return BadRequest($"Failed to permanently delete the specified Item Template.");
            }
        }
    }
}
