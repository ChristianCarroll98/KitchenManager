using KitchenManager.API.ItemsNS.ListItemsNS.DTO;
using KitchenManager.API.ItemsNS.ListItemsNS.Repo;
using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.UsersNS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace KitchenManager.API.ItemsNS.ListItemsNS
{
    [ApiController]
    [Route("[controller]")]
    public class ListItemController : ControllerBase
    {
        private readonly UserManager<User> UserManager;
        private readonly IListItemRepository LIRepo;
        private readonly ILogger<ListItemController> LILogger;

        public ListItemController(UserManager<User> userManager, IListItemRepository lIRepo, ILogger<ListItemController> iTLogger)
        {
            UserManager = userManager;
            LIRepo = lIRepo;
            LILogger = iTLogger;
        }

        [Route("Test")]
        [HttpGet()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<Response<ListItemDTO>> Test(int id)
        {
            try
            {
                return Ok($"User is null: {User == null}, User.Identity.Name: {User.Identity.Name}");
            }
            catch (Exception ex)
            {
                LILogger.LogError($"failed to get cur User Id: {ex.Message}");
                return BadRequest($"failed to get cur User Id: {ex.Message}");
            }
        }
    }
}