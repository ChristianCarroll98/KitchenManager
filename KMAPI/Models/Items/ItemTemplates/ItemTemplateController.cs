using KitchenManager.KMAPI.Items.ItemTemplates.DTO;
using KitchenManager.KMAPI.Items.ItemTemplates.Repo;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KitchenManager.KMAPI.Items.ItemTemplates
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class ItemTemplateController : ControllerBase
    {
        private readonly IItemTemplateRepository ITrepo;

        public ItemTemplateController(IItemTemplateRepository repo)
        {
            ITrepo = repo;
        }

        /*
        Task<ItemTemplateResponse> RetrieveById(int id);
        Task<ItemTemplateResponse> RetrieveByNameAndBrand(string name, string brand);
        Task<ItemTemplatesResponse> RetrieveByStatus(ItemTemplateStatus status);
        Task<ItemTemplatesResponse> RetrieveAll();

        Task<ItemTemplateResponse> Create(ItemTemplateCUDModel model);
        Task<ItemTemplateResponse> Update(ItemTemplateCUDModel model);
        Task<ItemTemplateResponse> SetDeleteStatus(ItemTemplateCUDModel model);
        Task<ItemTemplateResponse> Delete(ItemTemplateCUDModel model);
        */


        [Route("RetrieveById/{id?}")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<ItemTemplateResponse> RetrieveById(int id)
        {
            try
            {
                return Ok(ITrepo.RetrieveById(id).Result.ItemTemplate);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to retrieve Item Template with Id: {id}. Message:{ex.Message}");
            }
        }
    }
}
