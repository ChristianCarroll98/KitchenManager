using KitchenManager.API.Data;
using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.SharedNS.StatusNS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.API.ItemTagsNS.Repo
{
    public interface IItemTagRepository
    {
        Task<ResponseModel<string>> RetrieveById(int id);
        Task<ResponseModel<string>> RetrieveByName(string name);
        Task<ResponseModel<List<string>>> RetrieveByStatus(Status status);
        Task<ResponseModel<List<string>>> RetrieveAll();

        Task<ResponseModel<string>> Create(string name); //admin only
        Task<ResponseModel<string>> Delete(string name); //admin only
    }

    public class ItemTagRepository : IItemTagRepository
    {
        private readonly KMDbContext Context;
        private readonly ILogger<ItemTagRepository> ITLogger;

        public ItemTagRepository(KMDbContext context, ILogger<ItemTagRepository> iTLogger)
        {
            Context = context;
            ITLogger = iTLogger;
        }

        public async Task<ResponseModel<string>> RetrieveById(int id)
        {
            ResponseModel<string> response = new();

            try
            {
                var itemTag = await Context
                        .ItemTags
                        .Where(it => it.Id == id)
                        .FirstOrDefaultAsync();

                if (itemTag == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find Item Tag with the specified Id.";
                    ITLogger.LogError($"Could not find Item Tag with Id: {id}");
                    return response;
                }

                response.Data = itemTag.Name;
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find Item Tag with the specified Id.";
                ITLogger.LogError($"An error occured while attempting to find Item Tag with Id: {id}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved Item Tag with the specified Id.";
            return response;
        }

        public async Task<ResponseModel<string>> RetrieveByName(string name)
        {
            ResponseModel<string> response = new();

            try
            {
                var itemTag = await Context
                        .ItemTags
                        .Where(it => it.Name == name)
                        .FirstOrDefaultAsync();

                if (itemTag == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find Item Tag with Name: {name}.";
                    ITLogger.LogError($"Could not find Item Tag with Name: {name}");
                    return response;
                }

                response.Data = itemTag.Name;
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find Item Tag with Name: {name}";
                ITLogger.LogError($"An error occured while attempting to find Item Tag with Name: {name}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved Item Tag with Name: {name}";
            return response;
        }

        public async Task<ResponseModel<List<string>>> RetrieveByStatus(Status status)
        {
            ResponseModel<List<string>> response = new();

            try
            {
                var itemTags = await Context
                        .ItemTags
                        .Where(it => it.Status == status)
                        .ToListAsync();

                if (!itemTags.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any Item Tags with the specified status.";
                    ITLogger.LogError($"Could not find any Item Tags with status: {status.ToString()}");
                    return response;
                }

                response.Data = itemTags.Select(it => it.Name).ToList();
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find Item Tags with the specified status.";
                ITLogger.LogError($"An error occured while attempting to find Item Tags with status: {status.ToString()}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved {response.Data.Count()} Item Tags with the specified status.";
            return response;
        }

        public async Task<ResponseModel<List<string>>> RetrieveAll()
        {
            ResponseModel<List<string>> response = new();

            try
            {
                var itemTags = await Context
                        .ItemTags
                        .Where(it => it.Status != Status.deleted)
                        .ToListAsync();

                if (!itemTags.Any())
                {
                    response.Success = false;
                    response.Message = $"No Item Tags were found";
                    ITLogger.LogError($"No Item Tags were found");
                    return response;
                }

                response.Data = itemTags.Select(it => it.Name).ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to get the list of Item Tags.";
                ITLogger.LogError($"An error occured while attempting to get the list of Item Tags. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved the list of {response.Data.Count()} Item Tags.";
            return response;
        }

        public async Task<ResponseModel<string>> Create(string name)
        {
            ResponseModel<string> response = new();

            try
            {
                ResponseModel<string> checkPreExisting = await RetrieveByName(name);

                if (checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"An Item Tag already exists with Name: {checkPreExisting.Data}.";
                    response.Data = checkPreExisting.Data;
                    ITLogger.LogError($"An Item Tag already exists with Name: {checkPreExisting.Data}.");
                    return response;
                }

                var newItemTag = new ItemTagModel()
                {
                    Name = name,
                    Pinned = false
                };

                await Context.ItemTags.AddAsync(newItemTag);
                await Context.SaveChangesAsync();

                ResponseModel<string> checkAdded = await RetrieveByName(name);

                if (!checkAdded.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to save the new Item Tag.";
                    ITLogger.LogError($"Failed to add new Item Tag to Database.");
                    return response;
                }

                response.Data = checkAdded.Data;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to create a new Item Tag.";
                ITLogger.LogError($"An error occured while attempting to create a new Item Tag. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully created new Item Template with Name: {name}.";
            return response;
        }

        public async Task<ResponseModel<string>> Delete(string name)
        {
            ResponseModel<string> response = new();

            try
            {
                ResponseModel<string> verifyPreExisting = await RetrieveByName(name);

                if (verifyPreExisting.Data == null)
                {
                    response.Success = false;
                    response.Message = $"Failed to find Item Tag with Name: {name} to permanently delete.";
                    ITLogger.LogError($"Failed to find Item Tag with Name: {name} to permanently delete.");
                    return response;
                }

                //need actual Item Tag object now, not string.
                var deletedItemTag = await Context
                        .ItemTags
                        .Where(it => it.Name == name)
                        .FirstOrDefaultAsync();

                Context.ItemTags.Remove(deletedItemTag);
                await Context.SaveChangesAsync();

                ResponseModel<string> verifyDeleted = await RetrieveById(deletedItemTag.Id);

                if (verifyDeleted.Success)
                {
                    response.Success = false;
                    response.Message = $"Item Tag permanent deletion failed.";
                    ITLogger.LogError($"Item Tag permanent deletion from the Database failed.");
                    return response;
                }

                response.Data = deletedItemTag.Name;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to permanently delete Item Tag with Name: {name}.";
                ITLogger.LogError($"An error occured while attempting to permanently delete Item Tag with Name: {name}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully permanently deleted Item Tag with Name: {name}.";
            return response;
        }
    }
}
