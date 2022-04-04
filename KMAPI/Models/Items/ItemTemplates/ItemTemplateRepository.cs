using KitchenManager.KMAPI.Data;
using KitchenManager.KMAPI.Items.ItemTemplates.DTO;
using KitchenManager.KMAPI.ItemTags;
using KitchenManager.KMAPI.Models.ItemTags.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.KMAPI.Items.ItemTemplates.Repo
{
    public interface IItemTemplateRepository
    {
        Task<ItemTemplateResponse> RetrieveById(int id);
        Task<ItemTemplateResponse> RetrieveByNameAndBrand(string name, string brand);
        Task<ItemTemplateListResponse> RetrieveByStatus(ItemStatus status);
        Task<ItemTemplateListResponse> RetrieveAll();

        Task<ItemTemplateResponse> Create(ItemTemplateDTO model);
        Task<ItemTemplateResponse> Update(ItemTemplateDTO model, string originalName, string originalBrand);
        Task<ItemTemplateResponse> SetDeleteStatus(string name, string brand);
        Task<ItemTemplateResponse> Delete(string name, string brand);
    }

    public class ItemTemplateRepository : IItemTemplateRepository
    {
        private readonly KMDbContext Context;
        private readonly ILogger<ItemTemplateController> ITLogger;

        public ItemTemplateRepository(KMDbContext context, ILogger<ItemTemplateController> iTLogger)
        {
            Context = context;
            ITLogger = iTLogger;
        }

        public async Task<ItemTemplateResponse> RetrieveById(int id)
        {
            ItemTemplateResponse response = new ItemTemplateResponse();

            try
            {
                var itemTemplate = await Context
                        .ItemTemplates
                        .Include(x => x.ItemTags)
                        .Where(it => it.Id == id)
                        .FirstOrDefaultAsync();

                if(itemTemplate == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find Item Template with the specified Id.";
                    ITLogger.LogError($"Could not find Item Template with Id: {id}");
                    return response;
                }

                response.ITResponseDTO.setValuesFromItemTemplate(itemTemplate);
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

        public async Task<ItemTemplateResponse> RetrieveByNameAndBrand(string name, string brand)
        {
            ItemTemplateResponse response = new ItemTemplateResponse();

            try
            {
                var itemTemplate = await Context
                        .ItemTemplates
                        .Include(x => x.ItemTags)
                        .Where(it => it.Name == name
                                && it.Brand == brand)
                        .FirstOrDefaultAsync();

                if (itemTemplate == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find Item Template with Name: {name} and Brand: {brand}";
                    ITLogger.LogError($"Could not find Item Template with Name: {name} and Brand: {brand}.");
                    return response;
                }

                response.ITResponseDTO.setValuesFromItemTemplate(itemTemplate);
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

        public async Task<ItemTemplateListResponse> RetrieveByStatus(ItemStatus status)
        {
            ItemTemplateListResponse response = new ItemTemplateListResponse();

            try
            {
                var itemTemplates = await Context
                        .ItemTemplates
                        .Include(x => x.ItemTags)
                        .Where(it => it.Status == status)
                        .ToListAsync();

                if (!itemTemplates.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any Item Templates with the specified status.";
                    ITLogger.LogError($"Could not find any Item Templates with status: {status.ToString()}");
                    return response;
                }

                response.SetITResponseDTOsFromITList(itemTemplates);
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

        public async Task<ItemTemplateListResponse> RetrieveAll()
        {
            ItemTemplateListResponse response = new ItemTemplateListResponse();

            try
            {
                var itemTemplates = await Context
                        .ItemTemplates
                        .Include(x => x.ItemTags)
                        .ToListAsync();

                if (!itemTemplates.Any())
                {
                    response.Success = false;
                    response.Message = $"No Item Templates were found";
                    ITLogger.LogError($"No Item Templates were found");
                    return response;
                }

                response.SetITResponseDTOsFromITList(itemTemplates);
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

        public async Task<ItemTemplateResponse> Create(ItemTemplateDTO model)
        {
            ItemTemplateResponse response = new ItemTemplateResponse();

            try
            {
                ItemTemplateResponse checkPreExisting = await RetrieveByNameAndBrand(model.Name, model.Brand);

                if(checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"An Item Template already exists with Name: {checkPreExisting.ITResponseDTO.Name} and Brand: {checkPreExisting.ITResponseDTO.Brand}.";
                    response.ITResponseDTO = checkPreExisting.ITResponseDTO;
                    ITLogger.LogError($"An Item Template already exists with Name: {checkPreExisting.ITResponseDTO.Name} and Brand: {checkPreExisting.ITResponseDTO.Brand}.");
                    return response;
                }

                var newItemTemplate = new ItemTemplate()
                {
                    Name = model.Name,
                    Brand = model.Brand,
                    Description = model.Description,
                    ExpirationDays = model.ExpirationDays,
                    Icon = model.Icon
                };

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
                        .Select(itdto => new ItemTag() { Name = itdto.Name })
                        .ToList());

                await Context.ItemTemplates.AddAsync(newItemTemplate);
                await Context.SaveChangesAsync();

                ItemTemplateResponse checkAdded = await RetrieveByNameAndBrand(model.Name, model.Brand);

                if (!checkAdded.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to save the new Item Template.";
                    ITLogger.LogError($"Failed to add new Item Template to Database.");
                    return response;
                }

                /*var itemTemplate = await Context
                        .ItemTemplates
                        .Include(x => x.ItemTags)
                        .Where(it => it.Name == model.Name
                                && it.Brand == model.Brand)
                        .FirstOrDefaultAsync();

                foreach(ItemTagDTO itemTagDTO in model.ItemTagDTOs)
                {
                    var itemTag = await Context.ItemTags.Where(it => it.Name == itemTagDTO.Name)
                            .FirstOrDefaultAsync();

                    if (itemTag == null)
                    {
                        itemTag = new ItemTag() { Name = itemTagDTO.Name, Items = new List<Item>() { itemTemplate } };
                        
                        Context.ItemTags.Add(itemTag);
                        
                        itemTag = await Context.ItemTags.Where(it => it.Name == itemTag.Name).FirstOrDefaultAsync();
                        
                        if (itemTag == null)
                        {
                            response.Success = false;
                            response.Message = $"Failed to add new Item Tag";
                            ITLogger.LogError($"Failed to add new Item Tag to Database.");
                            return response;
                        }

                        itemTemplate.ItemTags.Add(itemTag);
                    }
                    else
                    {
                        itemTemplate.ItemTags.Add(itemTag);
                        itemTag.Items.Add(itemTemplate);
                    }
                }

                await Context.SaveChangesAsync();*/
                
                response.ITResponseDTO = checkAdded.ITResponseDTO;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to create a new Item Template.";
                ITLogger.LogError($"An error occured while attempting to create a new Item Template. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully created new Item Template with Name: {response.ITResponseDTO.Name} and Brand: {response.ITResponseDTO.Brand}.";
            return response;
        }

        public async Task<ItemTemplateResponse> Update(ItemTemplateDTO model, string originalName, string originalBrand)
        {
            ItemTemplateResponse response = new ItemTemplateResponse();

            try
            {                
                ItemTemplateResponse verifyPreExisting = await RetrieveByNameAndBrand(originalName, originalBrand);

                if (!verifyPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to find the specified Item Template to update";
                    ITLogger.LogError($"Failed to find Item Template with original Name: {originalName} and original Brand: {originalBrand} to update.");
                    return response;
                }

                //need actual Item Template object now, not ItemTemplateDTO.
                var updatedItemTemplate = await Context
                        .ItemTemplates
                        .Include(x => x.ItemTags)
                        .Where(it => it.Name == originalName
                                && it.Brand == originalBrand)
                        .FirstOrDefaultAsync();

                updatedItemTemplate.Name = string.IsNullOrEmpty(model.Name) ?
                        updatedItemTemplate.Name : model.Name;

                updatedItemTemplate.Brand = string.IsNullOrEmpty(model.Brand) ?
                        updatedItemTemplate.Brand : model.Brand;

                updatedItemTemplate.Description = string.IsNullOrEmpty(model.Description) ?
                        updatedItemTemplate.Description : model.Description;

                updatedItemTemplate.ExpirationDays = model.ExpirationDays == -1 ?
                        updatedItemTemplate.ExpirationDays : model.ExpirationDays;

                updatedItemTemplate.Icon = model.Icon ?? updatedItemTemplate.Icon;

                updatedItemTemplate.ItemTags.Clear();

                //add pre-existing item tags from model // that are not already assigned to the Item Template
                updatedItemTemplate.ItemTags.AddRange(await Context.ItemTags
                        //.Where(it => !updatedItemTemplate.ItemTags
                        //        .Contains(it))
                        .Where(it => model.ItemTagDTOs
                                .Select(itdto => itdto.Name)
                                .Contains(it.Name))
                        .ToListAsync());

                //add new item tags from model // that are not already assigned to the Item Template
                updatedItemTemplate.ItemTags.AddRange(model.ItemTagDTOs
                        //.Where(itdto => !updatedItemTemplate.ItemTags
                        //        .Select(it => it.Name)
                        //        .Contains(itdto.Name))
                        .Where(itdto => !Context.ItemTags
                                .Select(it => it.Name)
                                .Contains(itdto.Name))
                        .Select(itdto => new ItemTag() { Name = itdto.Name })
                        .ToList());

                Context.ItemTemplates.Update(updatedItemTemplate);
                await Context.SaveChangesAsync();

                response.ITResponseDTO = new ItemTemplateDTO(updatedItemTemplate);
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

        public async Task<ItemTemplateResponse> SetDeleteStatus(string name, string brand)
        {
            ItemTemplateResponse response = new ItemTemplateResponse();

            try
            {
                ItemTemplateResponse verifyPreExisting = await RetrieveByNameAndBrand(name, brand);

                if (!verifyPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to find Item Template with Name: {name} and Brand: {brand} to delete.";
                    ITLogger.LogError($"Failed to find Item Template with Name: {name} and Brand: {brand} to set deleted status");
                    return response;
                }

                //need actual Item Template object now, not ItemTemplateDTO.
                var deletedStatusItemTemplate = await Context
                        .ItemTemplates
                        .Include(x => x.ItemTags)
                        .Where(it => it.Name == name
                                && it.Brand == brand)
                        .FirstOrDefaultAsync();

                deletedStatusItemTemplate.Status = ItemStatus.deleted;

                Context.ItemTemplates.Update(deletedStatusItemTemplate);
                await Context.SaveChangesAsync();

                response.ITResponseDTO = new ItemTemplateDTO(deletedStatusItemTemplate);
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

        public async Task<ItemTemplateResponse> Delete(string name, string brand)
        {
            ItemTemplateResponse response = new ItemTemplateResponse();

            try
            {
                ItemTemplateResponse verifyPreExisting = await RetrieveByNameAndBrand(name, brand);

                if (!verifyPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to find Item Template with Name: {name} and Brand: {brand} to permanently delete.";
                    ITLogger.LogError($"Failed to find Item Template with Name: {name} and Brand: {brand} to permanently delete.");
                    return response;
                }

                //need actual Item Template object now, not ItemTemplateDTO.
                var deletedItemTemplate = await Context
                        .ItemTemplates
                        .Include(x => x.ItemTags)
                        .Where(it => it.Name == name
                                && it.Brand == brand)
                        .FirstOrDefaultAsync();

                if (deletedItemTemplate.Status != ItemStatus.deleted)
                {
                    response.Success = false;
                    response.Message = $"Item Template permanent deletion failed - Status Error";
                    ITLogger.LogError($"Item Template status was not set to deleted when attempting to delete permanently.");
                    return response;
                }

                Context.ItemTemplates.Remove(deletedItemTemplate);
                await Context.SaveChangesAsync();

                ItemTemplateResponse verifyDeleted = await RetrieveById(deletedItemTemplate.Id);

                if(verifyDeleted.Success){
                    response.Success = false;
                    response.Message = $"Item Template permanent deletion failed.";
                    ITLogger.LogError($"Item Template permanent deletion from the Database failed.");
                    return response;
                }

                response.ITResponseDTO = response.ITResponseDTO = new ItemTemplateDTO(deletedItemTemplate);
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