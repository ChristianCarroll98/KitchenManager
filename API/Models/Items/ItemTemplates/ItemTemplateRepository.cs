using KitchenManager.API.Data;
using KitchenManager.API.ItemsNS.ItemTemplatesNS.DTO;
using KitchenManager.API.ItemTagsNS;
using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.SharedNS.StatusNS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.API.ItemsNS.ItemTemplatesNS.Repo
{
    public interface IItemTemplateRepository
    {
        Task<ResponseModel<ItemTemplateReadDTO>> RetrieveById(int id);
        Task<ResponseModel<List<ItemTemplateReadDTO>>> RetrieveByName(string name);
        Task<ResponseModel<List<ItemTemplateReadDTO>>> RetrieveByBrand(string brand);
        Task<ResponseModel<ItemTemplateReadDTO>> RetrieveByNameAndBrand(string name, string brand);
        Task<ResponseModel<List<ItemTemplateReadDTO>>> RetrieveByStatus(Status status);
        Task<ResponseModel<List<ItemTemplateReadDTO>>> RetrieveByItemTags(List<string> tagNames);
        Task<ResponseModel<List<ItemTemplateReadDTO>>> RetrieveAll();

        Task<ResponseModel<ItemTemplateReadDTO>> Create(ItemTemplateCreateUpdateDTO model);
        Task<ResponseModel<ItemTemplateReadDTO>> Update(ItemTemplateCreateUpdateDTO model, string originalName, string originalBrand); //admin only
        Task<ResponseModel<ItemTemplateReadDTO>> SetActiveStatus(string name, string brand); //admin only
        Task<ResponseModel<ItemTemplateReadDTO>> SetDeletedStatus(string name, string brand); //admin only
        Task<ResponseModel<ItemTemplateReadDTO>> Delete(string name, string brand); //admin only
    }   

    public class ItemTemplateRepository : IItemTemplateRepository
    {
        private readonly KMDbContext Context;
        private readonly ILogger<ItemTemplateRepository> ITLogger;

        public ItemTemplateRepository(KMDbContext context, ILogger<ItemTemplateRepository> iTLogger)
        {
            Context = context;
            ITLogger = iTLogger;
        }

        public async Task<ResponseModel<ItemTemplateReadDTO>> RetrieveById(int id)
        {
            ResponseModel<ItemTemplateReadDTO> response = new();

            try
            {
                var itemTemplate = await Context
                        .ItemTemplates
                        .Include(it => it.ItemTags)
                        .Include(it => it.Icon)
                        .Where(it => it.Id == id)
                        .FirstOrDefaultAsync();

                if(itemTemplate is null)
                {
                    response.Success = false;
                    response.Message = $"Could not find Item Template with the specified Id.";
                    ITLogger.LogError($"Could not find Item Template with Id: {id}");
                    return response;
                }

                response.Data = new ItemTemplateReadDTO(itemTemplate);
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find Item Template with the specified Id.";
                ITLogger.LogError($"An error occured while attempting to find Item Template with Id: {id}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved Item Template with the specified Id.";
            return response;
        }

        public async Task<ResponseModel<List<ItemTemplateReadDTO>>> RetrieveByName(string name)
        {
            ResponseModel<List<ItemTemplateReadDTO>> response = new();

            try
            {
                var itemTemplates = await Context
                        .ItemTemplates
                        .Include(it => it.ItemTags)
                        .Include(it => it.Icon)
                        .Where(it => it.Name == name &&
                                it.Status == Status.active)
                        .ToListAsync();

                if (!itemTemplates.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any Item Templates with Name: {name}.";
                    ITLogger.LogError($"Could not find any Item Templates with Name: {name}.");
                    return response;
                }

                response.Data = itemTemplates.Select(it => new ItemTemplateReadDTO(it)).ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find Item Templates with Name: {name}.";
                ITLogger.LogError($"An error occured while attempting to find Item Templates with Name: {name}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved {response.Data.Count()} Item Templates with Name: {name}.";
            return response;
        }

        public async Task<ResponseModel<List<ItemTemplateReadDTO>>> RetrieveByBrand(string brand)
        {
            ResponseModel<List<ItemTemplateReadDTO>> response = new();

            try
            {
                var itemTemplates = await Context
                        .ItemTemplates
                        .Include(it => it.ItemTags)
                        .Include(it => it.Icon)
                        .Where(it => it.Brand == brand &&
                                it.Status == Status.active)
                        .ToListAsync();

                if (!itemTemplates.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any Item Templates with Brand: {brand}.";
                    ITLogger.LogError($"Could not find any Item Templates with Brand: {brand}.");
                    return response;
                }

                response.Data = itemTemplates.Select(it => new ItemTemplateReadDTO(it)).ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find Item Templates with Brand: {brand}.";
                ITLogger.LogError($"An error occured while attempting to find Item Templates with Brand: {brand}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved {response.Data.Count()} Item Templates with Brand: {brand}.";
            return response;
        }

        public async Task<ResponseModel<ItemTemplateReadDTO>> RetrieveByNameAndBrand(string name, string brand)
        {
            ResponseModel<ItemTemplateReadDTO> response = new();

            try
            {
                var itemTemplate = await Context
                        .ItemTemplates
                        .Include(it => it.ItemTags)
                        .Include(it => it.Icon)
                        .Where(it => it.Name == name &&
                                it.Brand == brand) //dont check for active here so I can update from inactive to active.
                        .FirstOrDefaultAsync();

                if (itemTemplate is null)
                {
                    response.Success = false;
                    response.Message = $"Could not find Item Template with Name: {name} and Brand: {brand}";
                    ITLogger.LogError($"Could not find Item Template with Name: {name} and Brand: {brand}.");
                    return response;
                }

                response.Data = new ItemTemplateReadDTO(itemTemplate);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find Item Template with Name: {name} and Brand: {brand}.";
                ITLogger.LogError($"An error occured while attempting to find Item Template with Name: {name} and Brand: {brand}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved Item Template with Name: {name} and Brand: {brand}.";
            return response;
        }

        public async Task<ResponseModel<List<ItemTemplateReadDTO>>> RetrieveByItemTags(List<string> tagNames)
        {
            ResponseModel<List<ItemTemplateReadDTO>> response = new();

            try
            {
                var itemTemplates = Context
                        .ItemTemplates
                        .Include(it => it.ItemTags)
                        .Include(it => it.Icon)
                        .AsEnumerable()
                        .Where(it =>
                                it.Status == Status.active &&
                                (tagNames.Any() ?
                                
                                        tagNames.All(tn => it.ItemTags
                                                .Select(itg => itg.Name)
                                                .Contains(tn)) :
                                        !it.ItemTags.Any()))
                        .ToList();

                if (!itemTemplates.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any Item Templates with tags: {string.Join(", ", tagNames)}.";
                    ITLogger.LogError($"Could not find any Item Templates with tags: {string.Join(", ", tagNames)}");
                    return response;
                }

                response.Data = itemTemplates.Select(it => new ItemTemplateReadDTO(it)).ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find Item Templates with tags: {string.Join(", ", tagNames)}.";
                ITLogger.LogError($"An error occured while attempting to find Item Templates with tags: {string.Join(", ", tagNames)}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved {response.Data.Count()} Item Templates with the specified tags: {string.Join(", ", tagNames)}.";
            return response;
        }

        public async Task<ResponseModel<List<ItemTemplateReadDTO>>> RetrieveByStatus(Status status)
        {
            ResponseModel<List<ItemTemplateReadDTO>> response = new();

            try
            {
                var itemTemplates = await Context
                        .ItemTemplates
                        .Include(it => it.ItemTags)
                        .Include(it => it.Icon)
                        .Where(it => it.Status == status)
                        .ToListAsync();

                if (!itemTemplates.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any Item Templates with the specified status.";
                    ITLogger.LogError($"Could not find any Item Templates with status: {status.ToString()}");
                    return response;
                }

                response.Data = itemTemplates.Select(it => new ItemTemplateReadDTO(it)).ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find Item Templates with the specified status.";
                ITLogger.LogError($"An error occured while attempting to find Item Templates with status: {status.ToString()}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved {response.Data.Count()} Item Templates with the specified status.";
            return response;
        }

        public async Task<ResponseModel<List<ItemTemplateReadDTO>>> RetrieveAll()
        {
            ResponseModel<List<ItemTemplateReadDTO>> response = new();

            try
            {
                var itemTemplates = await Context
                        .ItemTemplates
                        .Include(it => it.ItemTags)
                        .Include(it => it.Icon)
                        .Where(it => it.Status == Status.active)
                        .ToListAsync();

                if (!itemTemplates.Any())
                {
                    response.Success = false;
                    response.Message = $"No Item Templates were found";
                    ITLogger.LogError($"No Item Templates were found");
                    return response;
                }

                response.Data = itemTemplates.Select(it => new ItemTemplateReadDTO(it)).ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to get the list of Item Templates.";
                ITLogger.LogError($"An error occured while attempting to get the list of Item Templates. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved the list of {response.Data.Count()} Item Templates.";
            return response;
        }

        public async Task<ResponseModel<ItemTemplateReadDTO>> Create(ItemTemplateCreateUpdateDTO model)
        {
            ResponseModel<ItemTemplateReadDTO> response = new();

            try
            {
                ResponseModel<ItemTemplateReadDTO> checkPreExisting = await RetrieveByNameAndBrand(model.Name, model.Brand);

                if(checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"An Item Template already exists with Name: {model.Name} and Brand: {model.Brand}. Message: {checkPreExisting.Message}";
                    response.Data = checkPreExisting.Data;
                    ITLogger.LogError($"An Item Template already exists with Name: {model.Name} and Brand: {model.Brand}. Message: {checkPreExisting.Message}");
                    return response;
                }

                var icon = await Context.Icons.Where(i => i.Name == model.IconName).FirstOrDefaultAsync();

                if (icon is null)
                {
                    response.Success = false;
                    response.Message = $"There is no Icon with Name: {model.IconName}.";
                    ITLogger.LogError($"There is no Icon with Name: {model.IconName}.");
                    return response;
                }

                var newItemTemplate = new ItemTemplateModel()
                {
                    Name = model.Name,
                    Brand = model.Brand,
                    Description = model.Description,
                    ExpirationDays = model.ExpirationDays,
                    Icon = icon,
                    //add pre-existing item tags from model
                    ItemTags = await Context.ItemTags
                            .Where(it => model.ItemTagNames
                                    .Contains(it.Name))
                        .Concat(
                        //add new item tags from model
                        model.ItemTagNames
                                .Where(itname => !Context.ItemTags
                                        .Select(it => it.Name)
                                        .Contains(itname))
                                .Select(itname => new ItemTagModel() { Name = itname }))
                        .ToListAsync()
                };

                //make sure inactive tags are set to active when assigned to this item Template.
                var inactiveTags = newItemTemplate.ItemTags.Where(it => it.Status != Status.active).ToList();
                foreach (ItemTagModel itemTag in inactiveTags)
                {
                    itemTag.Status = Status.active;
                }

                await Context.ItemTemplates.AddAsync(newItemTemplate);
                await Context.SaveChangesAsync();

                ResponseModel<ItemTemplateReadDTO> checkAdded = await RetrieveByNameAndBrand(model.Name, model.Brand);

                if (!checkAdded.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to save the new Item Template. Message: {checkAdded.Message}";
                    ITLogger.LogError($"Failed to add new Item Template to Database. Message: {checkAdded.Message}");
                    return response;
                }
                
                response.Data = checkAdded.Data;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to create a new Item Template.";
                ITLogger.LogError($"An error occured while attempting to create a new Item Template. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully created new Item Template with Name: {response.Data.Name} and Brand: {response.Data.Brand}.";
            return response;
        }

        public async Task<ResponseModel<ItemTemplateReadDTO>> Update(ItemTemplateCreateUpdateDTO model, string originalName, string originalBrand)
        {
            ResponseModel<ItemTemplateReadDTO> response = new();

            try
            {
                ResponseModel<ItemTemplateReadDTO> verifyPreExisting = await RetrieveByNameAndBrand(originalName, originalBrand);

                if (!verifyPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to find the specified Item Template to update. Message: {verifyPreExisting.Message}";
                    ITLogger.LogError($"Failed to find Item Template with original Name: {originalName} and original Brand: {originalBrand} to update. Message: {verifyPreExisting.Message}");
                    return response;
                }

                var icon = await Context.Icons.Where(i => i.Name == model.IconName).FirstOrDefaultAsync();

                if (icon is null)
                {
                    response.Success = false;
                    response.Message = $"There is no Icon with Name: {model.IconName}.";
                    ITLogger.LogError($"There is no Icon with Name: {model.IconName}.");
                    return response;
                }

                //need actual Item Template object now, not ItemTemplateDTO.
                var updatedItemTemplate = await Context
                        .ItemTemplates
                        .Include(it => it.ItemTags)
                        .Include(it => it.Icon)
                        .Where(it => it.Name == originalName
                                && it.Brand == originalBrand)
                        .FirstOrDefaultAsync();
                
                updatedItemTemplate.Name = model.Name ?? updatedItemTemplate.Name;
                updatedItemTemplate.Brand = model.Brand ?? updatedItemTemplate.Brand;
                updatedItemTemplate.Description = model.Description ?? updatedItemTemplate.Description;
                updatedItemTemplate.ExpirationDays = model.ExpirationDays;
                updatedItemTemplate.Icon = icon;
                //add pre-existing item tags from model
                updatedItemTemplate.ItemTags = await Context.ItemTags
                        .Where(it => model.ItemTagNames
                                .Contains(it.Name))
                    .Concat(
                    //add new item tags from model
                    model.ItemTagNames
                            .Where(itname => !Context.ItemTags
                                    .Select(it => it.Name)
                                    .Contains(itname))
                            .Select(itname => new ItemTagModel() { Name = itname }))
                    .ToListAsync();

                //make sure inactive tags are set to active when assigned to this item Template.
                var inactiveTags = updatedItemTemplate.ItemTags.Where(it => it.Status != Status.active).ToList();
                foreach(ItemTagModel itemTag in inactiveTags)
                {
                    itemTag.Status = Status.active;
                }

                Context.ItemTemplates.Update(updatedItemTemplate);
                await Context.SaveChangesAsync();

                //update item tags' status to inactive if admin created or to deleted if user created
                //if any were removed from all item templates and list items.
                var noItemsItemTags = Context.ItemTags.Where(it => !it.Items.Any());
                foreach (ItemTagModel itemTag in noItemsItemTags)
                {
                    itemTag.Status = itemTag.Pinned ? Status.inactive : Status.deleted;
                }

                await Context.SaveChangesAsync();

                response.Data = new ItemTemplateReadDTO(updatedItemTemplate);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to update the specified Item Template.";
                ITLogger.LogError($"An error occured while attempting to update Item Template with original Name: {originalName} and original Brand: {originalBrand}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully updated the specified Item Template.";
            return response;
        }

        public async Task<ResponseModel<ItemTemplateReadDTO>> SetActiveStatus(string name, string brand)
        {
            ResponseModel<ItemTemplateReadDTO> response = new();

            try
            {
                ResponseModel<ItemTemplateReadDTO> verifyPreExisting = await RetrieveByNameAndBrand(name, brand);

                if (!verifyPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to find Item Template with Name: {name} and Brand: {brand} to activate. Message: {verifyPreExisting.Message}";
                    ITLogger.LogError($"Failed to find Item Template with Name: {name} and Brand: {brand} to set active status. Message: {verifyPreExisting.Message}");
                    return response;
                }

                //need actual Item Template object now, not ItemTemplateDTO.
                var activatedStatusItemTemplate = await Context
                        .ItemTemplates
                        .Include(it => it.ItemTags)
                        .Include(it => it.Icon)
                        .Where(it => it.Name == name &&
                                it.Brand == brand)
                        .FirstOrDefaultAsync();

                activatedStatusItemTemplate.DeletedDate = DateTime.MaxValue;
                activatedStatusItemTemplate.Status = Status.active;

                Context.ItemTemplates.Update(activatedStatusItemTemplate);
                await Context.SaveChangesAsync();

                response.Data = new ItemTemplateReadDTO(activatedStatusItemTemplate);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to activate Item Template with Name: {name} and Brand: {brand}.";
                ITLogger.LogError($"An error occured while attempting to set active status for Item Template with Name: {name} and Brand: {brand}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully activated Item Template with Name: {name} and Brand: {brand}.";
            return response;
        }

        public async Task<ResponseModel<ItemTemplateReadDTO>> SetDeletedStatus(string name, string brand)
        {
            ResponseModel<ItemTemplateReadDTO> response = new();

            try
            {
                ResponseModel<ItemTemplateReadDTO> verifyPreExisting = await RetrieveByNameAndBrand(name, brand);

                if (!verifyPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to find Item Template with Name: {name} and Brand: {brand} to delete. Message: {verifyPreExisting.Message}";
                    ITLogger.LogError($"Failed to find Item Template with Name: {name} and Brand: {brand} to set deleted status. Message: {verifyPreExisting.Message}");
                    return response;
                }

                //need actual Item Template object now, not ItemTemplateDTO.
                var deletedStatusItemTemplate = await Context
                        .ItemTemplates
                        .Include(it => it.ItemTags)
                        .Include(it => it.Icon)
                        .Where(it => it.Name == name &&
                                it.Brand == brand)
                        .FirstOrDefaultAsync();

                deletedStatusItemTemplate.DeletedDate = DateTime.Today;
                deletedStatusItemTemplate.Status = Status.deleted;

                Context.ItemTemplates.Update(deletedStatusItemTemplate);
                await Context.SaveChangesAsync();

                response.Data = new ItemTemplateReadDTO(deletedStatusItemTemplate);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to delete Item Template with Name: {name} and Brand: {brand}.";
                ITLogger.LogError($"An error occured while attempting to set deleted status for Item Template with Name: {name} and Brand: {brand}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully deleted Item Template with Name: {name} and Brand: {brand}.";
            return response;
        }

        public async Task<ResponseModel<ItemTemplateReadDTO>> Delete(string name, string brand)
        {
            ResponseModel<ItemTemplateReadDTO> response = new();

            try
            {
                ResponseModel<ItemTemplateReadDTO> verifyPreExisting = await RetrieveByNameAndBrand(name, brand);

                if (!verifyPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to find Item Template with Name: {name} and Brand: {brand} to permanently delete. Message: {verifyPreExisting.Message}";
                    ITLogger.LogError($"Failed to find Item Template with Name: {name} and Brand: {brand} to permanently delete. Message: {verifyPreExisting.Message}");
                    return response;
                }

                //need actual Item Template object now, not ItemTemplateDTO.
                var deletedItemTemplate = await Context
                        .ItemTemplates
                        .Include(it => it.ItemTags)
                        .Include(it => it.Icon)
                        .Where(it => it.Name == name
                                && it.Brand == brand)
                        .FirstOrDefaultAsync();

                if (deletedItemTemplate.Status != Status.deleted)
                {
                    response.Success = false;
                    response.Message = $"Item Template permanent deletion failed, not marked for deletion.";
                    ITLogger.LogError($"Item Template status was not set to deleted when attempting to delete permanently.");
                    return response;
                }

                Context.ItemTemplates.Remove(deletedItemTemplate);
                await Context.SaveChangesAsync();

                ResponseModel<ItemTemplateReadDTO> verifyDeleted = await RetrieveById(deletedItemTemplate.Id);

                if(verifyDeleted.Success){
                    response.Success = false;
                    response.Message = $"Item Template permanent deletion failed. Message: {verifyDeleted.Message}";
                    ITLogger.LogError($"Item Template permanent deletion from the Database failed. Message: {verifyDeleted.Message}");
                    return response;
                }

                //update item tags' status if admin created or delete if user created
                //if any were removed from all item templates and list items.
                var noItemsItemTags = Context.ItemTags.Where(it => !it.Items.Any());
                foreach (ItemTagModel itemTag in noItemsItemTags)
                {
                    itemTag.Status = itemTag.Pinned ? Status.inactive : Status.deleted;
                }

                await Context.SaveChangesAsync();

                response.Data = new ItemTemplateReadDTO(deletedItemTemplate);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to permanently delete Item Template with Name: {name} and Brand: {brand}.";
                ITLogger.LogError($"An error occured while attempting to permanently delete Item Template with Name: {name} and Brand: {brand}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully permanently deleted Item Template with Name: {name} and Brand: {brand}.";
            return response;
        }
    }
}