using KitchenManager.KMAPI.Data;
using KitchenManager.KMAPI.Items.ItemTemplates.DTO;
using KitchenManager.KMAPI.ItemTags;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.KMAPI.Items.ItemTemplates.Repo
{
    public interface IItemTemplateRepository
    {
        Task<ItemTemplateResponse> RetrieveById(int id);
        Task<ItemTemplateResponse> RetrieveByNameAndBrand(string name, string brand);
        Task<ItemTemplatesResponse> RetrieveByStatus(ItemTemplateStatus status);
        Task<ItemTemplatesResponse> RetrieveAll();

        Task<ItemTemplateResponse> Create(ItemTemplateCUDModel model);
        Task<ItemTemplateResponse> Update(ItemTemplateCUDModel model);
        Task<ItemTemplateResponse> SetDeleteStatus(ItemTemplateCUDModel model);
        Task<ItemTemplateResponse> Delete(ItemTemplateCUDModel model);
    }

    public class ItemTemplateRepository : IItemTemplateRepository
    {
        private readonly KMDbContext context;

        public ItemTemplateRepository(KMDbContext ctx)
        {
            context = ctx;
        }

        public async Task<ItemTemplateResponse> RetrieveById(int id)
        {
            ItemTemplateResponse response = new ItemTemplateResponse();

            try
            {
                var itemTemplate = await context
                        .ItemTemplates
                        .Include(x => x.ItemTags)
                        .Where(it => it.Id == id)
                        .FirstOrDefaultAsync();

                if(itemTemplate == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find Item Template with Id: {id}";
                    return response;
                }

                response.ItemTemplate = itemTemplate;
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error while attempting to find Item Template with Id: {id}. Message: {ex.Message}";
                return response;
            }

            return response;
        }

        public async Task<ItemTemplateResponse> RetrieveByNameAndBrand(string name, string brand)
        {
            ItemTemplateResponse response = new ItemTemplateResponse();

            try
            {
                var itemTemplate = await context
                        .ItemTemplates
                        .Include(x => x.ItemTags)
                        .Where(it => it.Name == name
                                && it.Brand == brand)
                        .FirstOrDefaultAsync();

                if (itemTemplate == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find Item Template with Name: {name} and Brand: {brand}";
                    return response;
                }

                response.ItemTemplate = itemTemplate;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error while attempting to find Item Template with Name: {name} and Brand: {brand}. Message: {ex.Message}";
                return response;
            }

            return response;
        }

        public async Task<ItemTemplatesResponse> RetrieveByStatus(ItemTemplateStatus status)
        {
            ItemTemplatesResponse response = new ItemTemplatesResponse();

            try
            {
                var itemTemplates = await context
                        .ItemTemplates
                        .Include(x => x.ItemTags)
                        .Where(it => it.Status == status)
                        .ToListAsync();

                if (!itemTemplates.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any Item Templates with status: {status}";
                    return response;
                }

                response.ItemTemplates = itemTemplates;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error while attempting to find Item Templates with status: {status}. Message: {ex.Message}";
                return response;
            }

            return response;
        }

        public async Task<ItemTemplatesResponse> RetrieveAll()
        {
            ItemTemplatesResponse response = new ItemTemplatesResponse();

            try
            {
                var itemTemplates = await context
                        .ItemTemplates
                        .Include(x => x.ItemTags)
                        .ToListAsync();

                if (!itemTemplates.Any())
                {
                    response.Success = false;
                    response.Message = $"There are no Item Templates";
                    return response;
                }

                response.ItemTemplates = itemTemplates;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error while attempting to get list of Item Templates. Message: {ex.Message}";
                return response;
            }

            return response;
        }

        public async Task<ItemTemplateResponse> Create(ItemTemplateCUDModel model)
        {
            ItemTemplateResponse response = new ItemTemplateResponse();

            try
            {
                ItemTemplateResponse checkPreExisting = await RetrieveByNameAndBrand(model.Name, model.Brand);

                if(checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"An Item Template already exists with Name: {checkPreExisting.ItemTemplate.Name} and Brand: {checkPreExisting.ItemTemplate.Brand}.";
                    response.ItemTemplate = checkPreExisting.ItemTemplate;
                    return response;
                }

                var newItemTemplate = new ItemTemplate()
                {
                    Name = model.Name,
                    Brand = model.Brand,
                    Description = model.Description,
                    ExpirationDays = model.ExpirationDays,
                    Icon = model.Icon,
                    ItemTags = model.ItemTags
                };

                await context.ItemTemplates.AddAsync(newItemTemplate);
                await context.SaveChangesAsync();

                ItemTemplateResponse checkAdded = await RetrieveByNameAndBrand(model.Name, model.Brand);

                if (!checkAdded.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to add new Item Template to Database";
                    return response;
                }

                response.ItemTemplate = checkAdded.ItemTemplate;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error while attempting to create a new Item Template. Message: {ex.Message}";
                return response;
            }

            return response;
        }

        public async Task<ItemTemplateResponse> Update(ItemTemplateCUDModel model)
        {
            ItemTemplateResponse response = new ItemTemplateResponse();

            try
            {                
                ItemTemplateResponse verifyPreExisting = await RetrieveById(model.Id);

                if (!verifyPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to find Item Template with Id: {model.Id} to update";
                    return response;
                }

                var updatedItemTemplate = verifyPreExisting.ItemTemplate;

                updatedItemTemplate.Name = string.IsNullOrEmpty(model.Name) ?
                        updatedItemTemplate.Name : model.Name;

                updatedItemTemplate.Brand = string.IsNullOrEmpty(model.Brand) ?
                        updatedItemTemplate.Brand : model.Brand;

                updatedItemTemplate.Description = string.IsNullOrEmpty(model.Description) ?
                        updatedItemTemplate.Description : model.Description;

                updatedItemTemplate.ExpirationDays = model.ExpirationDays == -1 ?
                        updatedItemTemplate.ExpirationDays : model.ExpirationDays;

                updatedItemTemplate.Icon = model.Icon == null ?
                        updatedItemTemplate.Icon : model.Icon;

                updatedItemTemplate.ItemTags = model.ItemTags == null ?
                        updatedItemTemplate.ItemTags : model.ItemTags;

                context.ItemTemplates.Update(updatedItemTemplate);
                await context.SaveChangesAsync();

                response.ItemTemplate = (await RetrieveById(updatedItemTemplate.Id)).ItemTemplate;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error while attempting to update Item Template with Id: {model.Id}. Message: {ex.Message}";
                return response;
            }

            return response;
        }

        public async Task<ItemTemplateResponse> SetDeleteStatus(ItemTemplateCUDModel model)
        {
            ItemTemplateResponse response = new ItemTemplateResponse();

            try
            {
                ItemTemplateResponse verifyPreExisting = await RetrieveById(model.Id);

                if (model.Id != 0)
                {
                    verifyPreExisting = await RetrieveById(model.Id);

                    if (!verifyPreExisting.Success)
                    {
                        response.Success = false;
                        response.Message = $"Failed to find Item Template with Id: {model.Id} to set deleted status";
                        return response;
                    }  
                }
                else
                {
                    verifyPreExisting = await RetrieveByNameAndBrand(model.Name, model.Brand);

                    if (!verifyPreExisting.Success)
                    {
                        response.Success = false;
                        response.Message = $"Item Template to set deleted status for does not exist!";
                        return response;
                    }
                }
                
                var deleteStatusItemTemplate = verifyPreExisting.ItemTemplate;

                deleteStatusItemTemplate.Status = ItemTemplateStatus.deleted;

                context.ItemTemplates.Update(deleteStatusItemTemplate);
                await context.SaveChangesAsync();

                response.ItemTemplate = (await RetrieveById(deleteStatusItemTemplate.Id)).ItemTemplate;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error while attempting to set deleted status for Item Template with Id: {model.Id}, Name: {model.Name} and Brand: {model.Brand}. Message: {ex.Message}";
                return response;
            }

            return response;
        }

        public async Task<ItemTemplateResponse> Delete(ItemTemplateCUDModel model)
        {
            ItemTemplateResponse response = new ItemTemplateResponse();

            try
            {
                ItemTemplateResponse verifyPreExisting = await RetrieveById(model.Id);

                if (model.Id != 0)
                {
                    verifyPreExisting = await RetrieveById(model.Id);

                    if (!verifyPreExisting.Success)
                    {
                        response.Success = false;
                        response.Message = $"Failed to find Item Template with Id: {model.Id} to delete";
                        return response;
                    }
                }
                else
                {
                    verifyPreExisting = await RetrieveByNameAndBrand(model.Name, model.Brand);

                    if (!verifyPreExisting.Success)
                    {
                        response.Success = false;
                        response.Message = $"Item Template to delete does not exist!";
                        return response;
                    }
                }

                var deletedItemTemplate = verifyPreExisting.ItemTemplate;

                if(deletedItemTemplate.Status != ItemTemplateStatus.deleted)
                {
                    response.Success = false;
                    response.Message = $"Item Template status was not set to deleted when attempting to delete permanently.";
                    return response;
                }

                context.ItemTemplates.Remove(deletedItemTemplate);
                await context.SaveChangesAsync();

                ItemTemplateResponse verifyDeleted = await RetrieveById(deletedItemTemplate.Id);

                if(verifyDeleted.Success){
                    response.Success = false;
                    response.Message = $"Item Template deletion failed.";
                    return response;
                }

                response.ItemTemplate = deletedItemTemplate;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error while attempting to delete Item Template with Id: {model.Id}, Name: {model.Name} and Brand: {model.Brand}. Message: {ex.Message}";
                return response;
            }

            return response;
        }
    }
}