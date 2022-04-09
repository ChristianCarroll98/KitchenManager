﻿using KitchenManager.API.Data;
using KitchenManager.API.IconsNS;
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
        Task<ResponseModel<ItemTemplateCreateUpdateDTO>> RetrieveById(int id);
        Task<ResponseModel<List<ItemTemplateCreateUpdateDTO>>> RetrieveByName(string name);
        Task<ResponseModel<List<ItemTemplateCreateUpdateDTO>>> RetrieveByBrand(string brand);
        Task<ResponseModel<ItemTemplateCreateUpdateDTO>> RetrieveByNameAndBrand(string name, string brand);
        Task<ResponseModel<List<ItemTemplateCreateUpdateDTO>>> RetrieveByStatus(Status status);
        Task<ResponseModel<List<ItemTemplateCreateUpdateDTO>>> RetrieveByItemTags(List<string> tagNames);
        Task<ResponseModel<List<ItemTemplateCreateUpdateDTO>>> RetrieveAll();

        Task<ResponseModel<ItemTemplateCreateUpdateDTO>> Create(ItemTemplateCreateUpdateDTO model);
        Task<ResponseModel<ItemTemplateCreateUpdateDTO>> Update(ItemTemplateCreateUpdateDTO model, string originalName, string originalBrand); //admin only
        Task<ResponseModel<ItemTemplateCreateUpdateDTO>> SetActiveStatus(string name, string brand); //admin only
        Task<ResponseModel<ItemTemplateCreateUpdateDTO>> SetDeletedStatus(string name, string brand); //admin only
        Task<ResponseModel<ItemTemplateCreateUpdateDTO>> Delete(string name, string brand); //admin only
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

        public async Task<ResponseModel<ItemTemplateCreateUpdateDTO>> RetrieveById(int id)
        {
            ResponseModel<ItemTemplateCreateUpdateDTO> response = new();

            try
            {
                var itemTemplate = await Context
                        .ItemTemplates
                        .Include(it => it.ItemTags)
                        .Include(it => it.Icon)
                        .Where(itmp => itmp.Id == id)
                        .FirstOrDefaultAsync();

                if(itemTemplate == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find Item Template with the specified Id.";
                    ITLogger.LogError($"Could not find Item Template with Id: {id}");
                    return response;
                }

                response.Data = new ItemTemplateDTO(itemTemplate);
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

        public async Task<ResponseModel<List<ItemTemplateCreateUpdateDTO>>> RetrieveByName(string name)
        {
            ResponseModel<List<ItemTemplateCreateUpdateDTO>> response = new();

            try
            {
                var itemTemplates = await Context
                        .ItemTemplates
                        .Include(it => it.ItemTags)
                        .Include(it => it.Icon)
                        .Where(itmp => itmp.Name == name &&
                                itmp.Status == Status.active)
                        .ToListAsync();

                if (!itemTemplates.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any Item Templates with Name: {name}.";
                    ITLogger.LogError($"Could not find any Item Templates with Name: {name}.");
                    return response;
                }

                response.Data = itemTemplates.Select(it => new ItemTemplateDTO(it)).ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find Item Templates with Name: {name}.";
                ITLogger.LogError($"An error occured while attempting to find Item Templates with Name: {name}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved Item Templates with Name: {name}.";
            return response;
        }

        public async Task<ResponseModel<List<ItemTemplateCreateUpdateDTO>>> RetrieveByBrand(string brand)
        {
            ResponseModel<List<ItemTemplateCreateUpdateDTO>> response = new();

            try
            {
                var itemTemplates = await Context
                        .ItemTemplates
                        .Include(it => it.ItemTags)
                        .Include(it => it.Icon)
                        .Where(itmp => itmp.Brand == brand &&
                                itmp.Status == Status.active)
                        .ToListAsync();

                if (!itemTemplates.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any Item Templates with Brand: {brand}.";
                    ITLogger.LogError($"Could not find any Item Templates with Brand: {brand}.");
                    return response;
                }

                response.Data = itemTemplates.Select(it => new ItemTemplateDTO(it)).ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find Item Templates with Brand: {brand}.";
                ITLogger.LogError($"An error occured while attempting to find Item Templates with Brand: {brand}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved Item Templates with Brand: {brand}.";
            return response;
        }

        public async Task<ResponseModel<ItemTemplateCreateUpdateDTO>> RetrieveByNameAndBrand(string name, string brand)
        {
            ResponseModel<ItemTemplateCreateUpdateDTO> response = new();

            try
            {
                var itemTemplate = await Context
                        .ItemTemplates
                        .Include(it => it.ItemTags)
                        .Include(it => it.Icon)
                        .Where(itmp => itmp.Name == name &&
                                itmp.Brand == brand &&
                                itmp.Status == Status.active)
                        .FirstOrDefaultAsync();

                if (itemTemplate == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find Item Template with Name: {name} and Brand: {brand}";
                    ITLogger.LogError($"Could not find Item Template with Name: {name} and Brand: {brand}.");
                    return response;
                }

                response.Data = new ItemTemplateDTO(itemTemplate);
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

        public async Task<ResponseModel<List<ItemTemplateCreateUpdateDTO>>> RetrieveByItemTags(List<string> tagNames)
        {
            ResponseModel<List<ItemTemplateCreateUpdateDTO>> response = new();

            try
            {
                List<ItemTemplateModel> itemTemplates;

                if (tagNames.Any())
                {
                    itemTemplates = await Context
                            .ItemTemplates
                            .Include(it => it.ItemTags)
                            .Include(it => it.Icon)
                            .Where(itmp => itmp.ItemTags
                                    .Select(it => it.Name)
                                    .Any(itn => tagNames
                                            .Contains(itn)) &&
                                    itmp.Status == Status.active)
                            .ToListAsync();
                }
                else
                {
                    itemTemplates = await Context
                            .ItemTemplates
                            .Include(it => it.ItemTags)
                            .Include(it => it.Icon)
                            .Where(itmp => !itmp.ItemTags.Any() &&
                                    itmp.Status == Status.active)
                            .ToListAsync();
                }

                if (!itemTemplates.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any Item Templates with tags: {string.Join(", ", tagNames)}.";
                    ITLogger.LogError($"Could not find any Item Templates with tags: {string.Join(", ", tagNames)}");
                    return response;
                }

                response.Data = itemTemplates.Select(it => new ItemTemplateCreateUpdateDTO(it)).ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find Item Templates with tags: {string.Join(", ", tagNames)}.";
                ITLogger.LogError($"An error occured while attempting to find Item Templates with tags: {string.Join(", ", tagNames)}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved Item Templates with the specified tags: {string.Join(", ", tagNames)}.";
            return response;
        }

        public async Task<ResponseModel<List<ItemTemplateCreateUpdateDTO>>> RetrieveByStatus(Status status)
        {
            ResponseModel<List<ItemTemplateCreateUpdateDTO>> response = new();

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

                response.Data = itemTemplates.Select(it => new ItemTemplateDTO(it)).ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find Item Templates with the specified status.";
                ITLogger.LogError($"An error occured while attempting to find Item Templates with status: {status.ToString()}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved Item Templates with the specified status.";
            return response;
        }

        public async Task<ResponseModel<List<ItemTemplateCreateUpdateDTO>>> RetrieveAll()
        {
            ResponseModel<List<ItemTemplateCreateUpdateDTO>> response = new();

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

                response.Data = itemTemplates.Select(it => new ItemTemplateDTO(it)).ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to get the list of Item Templates.";
                ITLogger.LogError($"An error occured while attempting to get the list of Item Templates. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved the list of Item Templates.";
            return response;
        }

        public async Task<ResponseModel<ItemTemplateCreateUpdateDTO>> Create(ItemTemplateCreateUpdateDTO model)
        {
            ResponseModel<ItemTemplateCreateUpdateDTO> response = new();

            try
            {
                ResponseModel<ItemTemplateCreateUpdateDTO> checkPreExisting = await RetrieveByNameAndBrand(model.Name, model.Brand);

                if(checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"An Item Template already exists with Name: {model.Name} and Brand: {model.Brand}. Message: {checkPreExisting.Message}";
                    response.Data = checkPreExisting.Data;
                    ITLogger.LogError($"An Item Template already exists with Name: {model.Name} and Brand: {model.Brand}. Message: {checkPreExisting.Message}");
                    return response;
                }

                var newItemTemplate = new ItemTemplateModel()
                {
                    Name = model.Name,
                    Brand = model.Brand,
                    Description = model.Description,
                    ExpirationDays = model.ExpirationDays,
                };

                //sets icon to the pre-existing icon from model in DB if it exists otherwise creates a new one.
                newItemTemplate.Icon = await Context.Icons.Where(i => i.Name == model.IconDTO.Name).FirstOrDefaultAsync() ??
                        new IconModel() { Name = model.IconDTO.Name, Path = model.IconDTO.Path };

                //add pre-existing item tags from model
                newItemTemplate.ItemTags.AddRange(await Context.ItemTags
                        .Where(it => model.ItemTagDTOs
                                .Select(itdto => itdto.Name)
                                .Contains(it.Name))
                        .ToListAsync());

                //add new item tags from model
                newItemTemplate.ItemTags.AddRange(model.ItemTagDTOs
                        .Where(itdto => !Context.ItemTags
                                .Select(it => it.Name)
                                .Contains(itdto.Name))
                        .Select(itdto => new ItemTagModel() { Name = itdto.Name })
                        .ToList());

                //make sure inactive tags are set to active when assigned to this item Template.
                var inactiveTags = newItemTemplate.ItemTags.Where(it => it.Status != Status.active).ToList();
                foreach (ItemTagModel itemTag in inactiveTags)
                {
                    itemTag.Status = Status.active;
                }

                await Context.ItemTemplates.AddAsync(newItemTemplate);
                await Context.SaveChangesAsync();

                ResponseModel<ItemTemplateCreateUpdateDTO> checkAdded = await RetrieveByNameAndBrand(model.Name, model.Brand);

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

        public async Task<ResponseModel<ItemTemplateCreateUpdateDTO>> Update(ItemTemplateCreateUpdateDTO model, string originalName, string originalBrand)
        {
            ResponseModel<ItemTemplateCreateUpdateDTO> response = new();

            try
            {
                ResponseModel<ItemTemplateCreateUpdateDTO> verifyPreExisting = await RetrieveByNameAndBrand(originalName, originalBrand);

                if (!verifyPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to find the specified Item Template to update. Message: {verifyPreExisting.Message}";
                    ITLogger.LogError($"Failed to find Item Template with original Name: {originalName} and original Brand: {originalBrand} to update. Message: {verifyPreExisting.Message}");
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

                updatedItemTemplate.Name = model.Name;

                updatedItemTemplate.Brand = model.Brand;

                updatedItemTemplate.Description = model.Description;

                updatedItemTemplate.ExpirationDays = model.ExpirationDays;

                //sets icon to the pre-existing icon from model in DB if it exists otherwise creates a new one.
                updatedItemTemplate.Icon = await Context.Icons.Where(i => i.Name == model.IconDTO.Name).FirstOrDefaultAsync() ?? 
                        new IconModel() { Name = model.IconDTO.Name, Path = model.IconDTO.Path };

                updatedItemTemplate.ItemTags.Clear();

                //add pre-existing item tags from model
                updatedItemTemplate.ItemTags.AddRange(await Context.ItemTags
                        .Where(it => model.ItemTagDTOs
                                .Select(itdto => itdto.Name)
                                .Contains(it.Name))
                        .ToListAsync());

                //add new item tags from model
                updatedItemTemplate.ItemTags.AddRange(model.ItemTagDTOs
                        .Where(itdto => !Context.ItemTags
                                .Select(it => it.Name)
                                .Contains(itdto.Name))
                        .Select(itdto => new ItemTagModel() { Name = itdto.Name })
                        .ToList());

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
                var noItemsItemTags = await Context.ItemTags.Where(it => !it.Items.Any()).ToListAsync();
                foreach(ItemTagModel itemTag in noItemsItemTags)
                {
                    if (itemTag.Pinned)
                    {
                        itemTag.Status = Status.deleted;
                    }
                    else
                    {
                        itemTag.Status = Status.inactive;
                    }
                }

                await Context.SaveChangesAsync();

                response.Data = new ItemTemplateDTO(updatedItemTemplate);
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

        public async Task<ResponseModel<ItemTemplateCreateUpdateDTO>> SetActiveStatus(string name, string brand)
        {
            ResponseModel<ItemTemplateCreateUpdateDTO> response = new();

            try
            {
                ResponseModel<ItemTemplateCreateUpdateDTO> verifyPreExisting = await RetrieveByNameAndBrand(name, brand);

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
                        .Where(it => it.Name == name
                                && it.Brand == brand)
                        .FirstOrDefaultAsync();

                activatedStatusItemTemplate.Status = Status.active;

                Context.ItemTemplates.Update(activatedStatusItemTemplate);
                await Context.SaveChangesAsync();

                response.Data = new ItemTemplateDTO(activatedStatusItemTemplate);
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

        public async Task<ResponseModel<ItemTemplateCreateUpdateDTO>> SetDeletedStatus(string name, string brand)
        {
            ResponseModel<ItemTemplateCreateUpdateDTO> response = new();

            try
            {
                ResponseModel<ItemTemplateCreateUpdateDTO> verifyPreExisting = await RetrieveByNameAndBrand(name, brand);

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
                        .Where(it => it.Name == name
                                && it.Brand == brand)
                        .FirstOrDefaultAsync();

                deletedStatusItemTemplate.Status = Status.deleted;

                Context.ItemTemplates.Update(deletedStatusItemTemplate);
                await Context.SaveChangesAsync();

                response.Data = new ItemTemplateDTO(deletedStatusItemTemplate);
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

        public async Task<ResponseModel<ItemTemplateCreateUpdateDTO>> Delete(string name, string brand)
        {
            ResponseModel<ItemTemplateCreateUpdateDTO> response = new();

            try
            {
                ResponseModel<ItemTemplateCreateUpdateDTO> verifyPreExisting = await RetrieveByNameAndBrand(name, brand);

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

                ResponseModel<ItemTemplateCreateUpdateDTO> verifyDeleted = await RetrieveById(deletedItemTemplate.Id);

                if(verifyDeleted.Success){
                    response.Success = false;
                    response.Message = $"Item Template permanent deletion failed. Message: {verifyDeleted.Message}";
                    ITLogger.LogError($"Item Template permanent deletion from the Database failed. Message: {verifyDeleted.Message}");
                    return response;
                }

                //update item tags' status if admin created or delete if user created
                //if any were removed from all item templates and list items.
                var noItemsItemTags = await Context.ItemTags.Where(it => !it.Items.Any()).ToListAsync();
                foreach (ItemTagModel itemTag in noItemsItemTags)
                {
                    if (itemTag.Pinned)
                    {
                        itemTag.Status = Status.deleted;
                    }
                    else
                    {
                        itemTag.Status = Status.inactive;
                    }
                }

                await Context.SaveChangesAsync();

                response.Data = new ItemTemplateDTO(deletedItemTemplate);
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