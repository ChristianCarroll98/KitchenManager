using KitchenManager.API.Data;
using KitchenManager.API.IconsNS;
using KitchenManager.API.ItemsNS.ItemTemplatesNS;
using KitchenManager.API.ItemsNS.ItemTemplatesNS.DTO;
using KitchenManager.API.ItemsNS.ListItemsNS.DTO;
using KitchenManager.API.ItemTagsNS;
using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.SharedNS.StatusNS;
using KitchenManager.API.UserListsNS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitchenManager.API.ItemsNS.ListItemsNS.Repo
{
    public interface IListItemRepository
    {
        Task<ResponseModel<ListItemReadDTO>> RetrieveById(int id);
        Task<ResponseModel<List<ListItemReadDTO>>> RetrieveByUserListAndBrand(string userEmail, string userListName, string brand);
        Task<ResponseModel<ListItemReadDTO>> RetrieveByUserListAndNameAndBrand(string userEmail, string userListName, string name, string brand);
        Task<ResponseModel<List<ListItemReadDTO>>> RetrieveByUserListAndStatus(string userEmail, string userListName, Status status);
        Task<ResponseModel<List<ListItemReadDTO>>> RetrieveByUserListAndTags(string userEmail, string userListName, List<string> tagNames);
        Task<ResponseModel<List<ListItemReadDTO>>> RetrieveByUserList(string userEmail, string userListName);

        Task<ResponseModel<ListItemReadDTO>> Create(string userEmail, string userListName, ListItemCreateUpdateDTO model);
        Task<ResponseModel<ListItemReadDTO>> CreateFromItemTemplate(string userEmail, string userListName, ItemTemplateCreateUpdateDTO model);
        Task<ResponseModel<ListItemReadDTO>> Update(string userEmail, string userListName, string originalName, string originalBrand, ListItemCreateUpdateDTO model);
        Task<ResponseModel<ListItemReadDTO>> SetQuantity(string userEmail, string userListName, string name, string brand, int quantity);
        Task<ResponseModel<ListItemReadDTO>> SetActiveStatus(string userEmail, string userListName, string name, string brand);
        Task<ResponseModel<ListItemReadDTO>> SetDeletedStatus(string userEmail, string userListName, string name, string brand);
        Task<ResponseModel<ListItemReadDTO>> Delete(string userEmail, string userListName, string name, string brand);
    }

    public class ListItemRepository : IListItemRepository
    {
        private readonly KMDbContext Context;
        private readonly ILogger<ListItemRepository> LILogger;

        public ListItemRepository(KMDbContext context, ILogger<ListItemRepository> lILogger)
        {
            Context = context;
            LILogger = lILogger;
        }

        public async Task<ResponseModel<ListItemReadDTO>> RetrieveById(int id)
        {
            ResponseModel<ListItemReadDTO> response = new();

            try
            {
                var listItem = await Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        .Where(li => li.Id == id)
                        .FirstOrDefaultAsync();

                if (listItem == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find List Item with the specified Id.";
                    LILogger.LogError($"Could not find List Item with Id: {id}");
                    return response;
                }

                response.Data = new ListItemReadDTO(listItem);
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find List Item with the specified Id.";
                LILogger.LogError($"An error occured while attempting to find List Item with Id: {id}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved List Item with the specified Id.";
            return response;
        }

        public async Task<ResponseModel<List<ListItemReadDTO>>> RetrieveByUserListAndBrand(string userEmail, string userListName, string brand)
        {
            ResponseModel<List<ListItemReadDTO>> response = new();

            try
            {
                var userListResponse = await HGetUserListFromNameAndEmail(userEmail, userListName);
                
                if (!userListResponse.Success)
                {
                    response.Success = false;
                    response.Message = userListResponse.Message;
                    return response;
                }

                var listItems = await Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        .Where(li => li.UserListId == userListResponse.Data.Id &&
                                li.Brand == brand &&
                                li.Status == Status.active)
                        .ToListAsync();

                if (!listItems.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any List Items with Brand: {brand}";
                    LILogger.LogError($"Could not find any Items on list of User with Email: {userEmail} named: {userListName} with Brand: {brand}");
                    return response;
                }

                response.Data = listItems.Select(li => new ListItemReadDTO(li)).ToList();
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find List Items with Brand: {brand}.";
                LILogger.LogError($"An error occured while attempting to find Items on list of User with Email: {userEmail} named: {userListName} with Brand: {brand}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved {response.Data.Count()} List Items with Brand: {brand}.";
            return response;
        }

        public async Task<ResponseModel<ListItemReadDTO>> RetrieveByUserListAndNameAndBrand(string userEmail, string userListName, string name, string brand)
        {
            ResponseModel<ListItemReadDTO> response = new();

            try
            {
                var userListResponse = await HGetUserListFromNameAndEmail(userEmail, userListName);

                if (!userListResponse.Success)
                {
                    response.Success = false;
                    response.Message = userListResponse.Message;
                    return response;
                }

                var listItem = await Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        .Where(li => li.UserListId == userListResponse.Data.Id &&
                                li.Name == name &&
                                li.Brand == brand &&
                                li.Status == Status.active)
                        .FirstOrDefaultAsync();

                if (listItem == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find List Item with Name: {name} and Brand: {brand}.";
                    LILogger.LogError($"Could not find List Item with Name: {name} and Brand: {brand} on {userEmail}'s list: {userListName}.");
                    return response;
                }

                response.Data = new ListItemReadDTO(listItem);
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find List Item with Name: {name} and Brand: {brand}.";
                LILogger.LogError($"An error occured while attempting to find List Item with Name: {name} and Brand: {brand} for User: {userEmail}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved List Item with the specified Name: {name} and Brand: {brand}.";
            return response;
        }

        public async Task<ResponseModel<List<ListItemReadDTO>>> RetrieveByUserListAndStatus(string userEmail, string userListName, Status status)
        {
            ResponseModel<List<ListItemReadDTO>> response = new();

            try
            {
                var userListResponse = await HGetUserListFromNameAndEmail(userEmail, userListName);

                if (!userListResponse.Success)
                {
                    response.Success = false;
                    response.Message = userListResponse.Message;
                    return response;
                }

                var listItems = await Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        .Where(li => li.UserListId == userListResponse.Data.Id &&
                                li.Status == status)
                        .ToListAsync();

                if (!listItems.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any List Items with the specified status.";
                    LILogger.LogError($"Could not find any Items on list of User with Email: {userEmail} named: {userListName} with status: {status}.");
                    return response;
                }

                response.Data = listItems.Select(li => new ListItemReadDTO(li)).ToList();
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find List Items with the specified status.";
                LILogger.LogError($"An error occured while attempting to find Items on list of User with Email: {userEmail} named: {userListName} with status: {status}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved {response.Data.Count()} List Items with the specified status.";
            return response;
        }

        public async Task<ResponseModel<List<ListItemReadDTO>>> RetrieveByUserListAndTags(string userEmail, string userListName, List<string> tagNames)
        {
            ResponseModel<List<ListItemReadDTO>> response = new();

            try
            {
                var userListResponse = await HGetUserListFromNameAndEmail(userEmail, userListName);

                if (!userListResponse.Success)
                {
                    response.Success = false;
                    response.Message = userListResponse.Message;
                    return response;
                }

                var listItems = Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        .AsEnumerable()
                        .Where(li =>
                                li.Status == Status.active &&
                                li.UserListId == userListResponse.Data.Id &&
                                (tagNames.Any() ?
                                        tagNames.All(tn => li.ItemTags
                                                .Select(itg => itg.Name)
                                                .Contains(tn)) :
                                        !li.ItemTags.Any()))
                        .ToList();

                if (!listItems.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any List Items with tags: {string.Join(", ", tagNames)}.";
                    LILogger.LogError($"Could not find any List Items on list of User with Email: {userEmail} named: {userListName} with tags: {string.Join(", ", tagNames)}");
                    return response;
                }

                response.Data = listItems.Select(it => new ListItemReadDTO(it)).ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find List Items with tags: {string.Join(", ", tagNames)}.";
                LILogger.LogError($"An error occured while attempting to find List Items on list of User with Email: {userEmail} named: {userListName} with tags: {string.Join(", ", tagNames)}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved {response.Data.Count()} List Items with the specified tags: {string.Join(", ", tagNames)}.";
            return response;
        }

        public async Task<ResponseModel<List<ListItemReadDTO>>> RetrieveByUserList(string userEmail, string userListName)
        {
            ResponseModel<List<ListItemReadDTO>> response = new();

            try
            {
                var userListResponse = await HGetUserListFromNameAndEmail(userEmail, userListName);

                if (!userListResponse.Success)
                {
                    response.Success = false;
                    response.Message = userListResponse.Message;
                    return response;
                }

                var listItems = await Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        .Where(li => li.UserListId == userListResponse.Data.Id &&
                                li.Status == Status.active)
                        .ToListAsync();

                if (!listItems.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any List Items.";
                    LILogger.LogError($"Could not find any Items on list of User with Email: {userEmail} named: {userListName}.");
                    return response;
                }

                response.Data = listItems.Select(li => new ListItemReadDTO(li)).ToList();
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find List Items.";
                LILogger.LogError($"An error occured while attempting to find Items on list of User with Email: {userEmail} named: {userListName}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved {response.Data.Count()} List Items.";
            return response;
        }

        public async Task<ResponseModel<ListItemReadDTO>> Create(string userEmail, string userListName, ListItemCreateUpdateDTO model)
        {
            ResponseModel<ListItemReadDTO> response = new();

            try
            {
                var userListResponse = await HGetUserListFromNameAndEmail(userEmail, userListName);

                if (!userListResponse.Success)
                {
                    response.Success = false;
                    response.Message = userListResponse.Message;
                    return response;
                }

                ResponseModel<ListItemReadDTO> checkPreExisting = await RetrieveByUserListAndNameAndBrand(userEmail, userListName, model.Name, model.Brand);

                if (checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"A List Item already exists in this list with Name: {model.Name} and Brand: {model.Brand}. Message: {checkPreExisting.Message}";
                    response.Data = checkPreExisting.Data;
                    LILogger.LogError($"A List Item already exists on list of User with Email: {userEmail} named: {userListName} with Name: {model.Name} and Brand: {model.Brand}. Message: {checkPreExisting.Message}");
                    return response;
                }

                var newListItem = new ListItemModel()
                {
                    Name = model.Name,
                    Brand = model.Brand,
                    Description = model.Description,
                    ExpirationDate = model.ExpirationDate ?? DateTime.MaxValue,
                    UserListId = userListResponse.Data.Id,
                    Icon = await Context.Icons.Where(i => i.Name == model.IconName).FirstOrDefaultAsync() ??
                            new IconModel() { Name = model.IconName, Path = model.IconPath },
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

                //make sure inactive tags are set to active when assigned to this list item.
                var inactiveTags = newListItem.ItemTags.Where(it => it.Status != Status.active).ToList();
                foreach (ItemTagModel itemTag in inactiveTags)
                {
                    itemTag.Status = Status.active;
                }
                
                await Context.ListItems.AddAsync(newListItem);
                await Context.SaveChangesAsync();

                ResponseModel<ListItemReadDTO> checkAdded = await RetrieveByUserListAndNameAndBrand(userEmail, userListName, model.Name, model.Brand);

                if (!checkAdded.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to save the new List Item. Message: {checkAdded.Message}";
                    LILogger.LogError($"Failed to add new List Item to Database in list of User with Email: {userEmail} with name: {userListName}. Message: {checkAdded.Message}");
                    return response;
                }

                var checkItemTemplate = await Context.ItemTemplates
                        .Where(it => it.Name == model.Name &&
                                it.Brand == model.Brand)
                        .FirstOrDefaultAsync();

                if (checkItemTemplate == null)
                {
                    //create item template from item since it was not previously a template.
                    var newItemTemplate = new ItemTemplateModel()
                    {
                        Name = newListItem.Name,
                        Brand = newListItem.Brand,
                        Description = newListItem.Description,
                        ExpirationDays = (newListItem.ExpirationDate - DateTime.UtcNow).Days,
                        Status = Status.inactive, //waiting to be edited/approved by an admin
                        Icon = newListItem.Icon,
                        ItemTags = newListItem.ItemTags
                    };

                    await Context.ItemTemplates.AddAsync(newItemTemplate);
                    await Context.SaveChangesAsync();

                    checkItemTemplate = await Context.ItemTemplates
                        .Where(it => it.Name == model.Name &&
                                it.Brand == model.Brand)
                        .FirstOrDefaultAsync();

                    if (checkItemTemplate == null)
                    {
                        response.Success = false;
                        response.Message = $"Failed to save the new Item Template from List Item. Message: {checkAdded.Message}";
                        LILogger.LogError($"Failed to add new Item Template from List Item to Database in list of User with Email: {userEmail} with name: {userListName}. Message: {checkAdded.Message}");
                        return response;
                    }
                }

                response.Data = checkAdded.Data;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to create a new List Item.";
                LILogger.LogError($"An error occured while attempting to create a new List Item. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully created new List Item with Name: {response.Data.Name} and Brand: {response.Data.Brand}.";
            return response;
        }

        public async Task<ResponseModel<ListItemReadDTO>> CreateFromItemTemplate(string userEmail, string userListName, ItemTemplateCreateUpdateDTO itemTemplateModel)
        {
            ResponseModel<ListItemReadDTO> response = new();

            try
            {
                var userListResponse = await HGetUserListFromNameAndEmail(userEmail, userListName);

                if (!userListResponse.Success)
                {
                    response.Success = false;
                    response.Message = userListResponse.Message;
                    return response;
                }

                ResponseModel<ListItemReadDTO> checkPreExisting = await RetrieveByUserListAndNameAndBrand(userEmail, userListName, itemTemplateModel.Name, itemTemplateModel.Brand);

                if (checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"A List Item already exists in this list with Name: {itemTemplateModel.Name} and Brand: {itemTemplateModel.Brand}. Message: {checkPreExisting.Message}";
                    response.Data = checkPreExisting.Data;
                    LILogger.LogError($"A List Item already exists in list of User with Email: {userEmail} with name: {userListName} with Name: {itemTemplateModel.Name} and Brand: {itemTemplateModel.Brand}. Message: {checkPreExisting.Message}");
                    return response;
                }

                //check if there is an item template.
                var itemTemplate = await Context.ItemTemplates
                        .Include(it => it.ItemTags)
                        .Include(it => it.Icon)
                        .Where(it => it.Name == itemTemplateModel.Name &&
                                it.Brand == itemTemplateModel.Brand)
                        .FirstOrDefaultAsync();

                if (itemTemplate == null)
                {
                    response.Success = false;
                    response.Message = $"No Item Template with Name: {itemTemplateModel.Name} and Brand: {itemTemplateModel.Brand} exists.";
                    response.Data = checkPreExisting.Data;
                    LILogger.LogError($"No Item Template with Name: {itemTemplateModel.Name} and Brand: {itemTemplateModel.Brand} exists.");
                    return response;
                }

                var newListItem = new ListItemModel()
                {
                    Name = itemTemplateModel.Name,
                    Brand = itemTemplateModel.Brand,
                    Description = itemTemplateModel.Description,
                    Quantity = 1,
                    ExpirationDate = DateTime.UtcNow.AddDays(itemTemplateModel.ExpirationDays).Date,
                    UserListId = userListResponse.Data.Id,
                    Icon = await Context.Icons.Where(i => i.Name == itemTemplateModel.IconName).FirstOrDefaultAsync() ??
                            new IconModel() { Name = itemTemplateModel.IconName, Path = itemTemplateModel.IconPath },
                    //add pre-existing item tags from model
                    ItemTags = await Context.ItemTags
                            .Where(it => itemTemplateModel.ItemTagNames
                                    .Contains(it.Name))
                        .Concat(
                        //add new item tags from model
                        itemTemplateModel.ItemTagNames
                                .Where(itname => !Context.ItemTags
                                        .Select(it => it.Name)
                                        .Contains(itname))
                                .Select(itname => new ItemTagModel() { Name = itname }))
                        .ToListAsync()
                };

                //make sure inactive tags are set to active when assigned to this list item.
                var inactiveTags = newListItem.ItemTags.Where(it => it.Status != Status.active).ToList();
                foreach (ItemTagModel itemTag in inactiveTags)
                {
                    itemTag.Status = Status.active;
                }

                await Context.ListItems.AddAsync(newListItem);
                await Context.SaveChangesAsync();

                ResponseModel<ListItemReadDTO> checkAdded = await RetrieveByUserListAndNameAndBrand(userEmail, userListName, itemTemplateModel.Name, itemTemplateModel.Brand);

                if (!checkAdded.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to add the new List Item from Template. Message: {checkAdded.Message}";
                    LILogger.LogError($"Failed to add new List Item from Template to Database in list of User with Email: {userEmail} with name: {userListName}. Message: {checkAdded.Message}");
                    return response;
                }

                response.Data = checkAdded.Data;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to create a new List Item from Template.";
                LILogger.LogError($"An error occured while attempting to create a new List Item from Template. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully created new List Item from Template with Name: {response.Data.Name} and Brand: {response.Data.Brand}.";
            return response;
        }

        public async Task<ResponseModel<ListItemReadDTO>> Update(string userEmail, string userListName, string originalName, string originalBrand, ListItemCreateUpdateDTO model)
        {
            ResponseModel<ListItemReadDTO> response = new();

            try
            {
                var userListResponse = await HGetUserListFromNameAndEmail(userEmail, userListName);

                if (!userListResponse.Success)
                {
                    response.Success = false;
                    response.Message = userListResponse.Message;
                    return response;
                }

                ResponseModel<ListItemReadDTO> verifyPreExisting = await RetrieveByUserListAndNameAndBrand(userEmail, userListName, originalName, originalBrand);

                if (!verifyPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to find the specified List Item to update. Message: {verifyPreExisting.Message}";
                    LILogger.LogError($"Failed to find List Item in list of User with Email: {userEmail} with name: {userListName} with original Name: {originalName} and original Brand: {originalBrand} to update. Message: {verifyPreExisting.Message}");
                    return response;
                }

                //need actual List Item object now, not ListItemDTO.
                var updatedListItem = await Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        .Where(li => li.UserListId == userListResponse.Data.Id &&
                                li.Name == originalName &&
                                li.Brand == originalBrand)
                        .FirstOrDefaultAsync();


                updatedListItem.Name = model.Name ?? updatedListItem.Name;
                updatedListItem.Brand = model.Brand ?? updatedListItem.Brand;
                updatedListItem.Description = model.Description ?? updatedListItem.Description;
                updatedListItem.ExpirationDate = model.ExpirationDate ?? DateTime.MaxValue;
                updatedListItem.Icon = await Context.Icons.Where(i => i.Name == model.IconName).FirstOrDefaultAsync() ??
                        new IconModel() { Name = model.IconName, Path = model.IconPath };
                //add pre-existing item tags from model
                updatedListItem.ItemTags = await Context.ItemTags
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


                //make sure inactive tags are set to active when assigned to this list item.
                var inactiveTags = updatedListItem.ItemTags.Where(it => it.Status != Status.active).ToList();
                foreach (ItemTagModel itemTag in inactiveTags)
                {
                    itemTag.Status = Status.active;
                }

                Context.ListItems.Update(updatedListItem);
                await Context.SaveChangesAsync();

                //update item tags' status to inactive if admin created or to deleted if user created
                //if any were removed from all item templates and list items.
                var noItemsItemTags = Context.ItemTags.Where(it => !it.Items.Any());
                foreach (ItemTagModel itemTag in noItemsItemTags)
                {
                    itemTag.Status = itemTag.Pinned ? Status.inactive : Status.deleted;
                }

                await Context.SaveChangesAsync();

                response.Data = new ListItemReadDTO(updatedListItem);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to update the specified List Item.";
                LILogger.LogError($"An error occured while attempting to update List Item in list of User with Email: {userEmail} with name: {userListName} with original Name: {originalName} and original Brand: {originalBrand}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully updated the specified List Item.";
            return response;
        }

        public async Task<ResponseModel<ListItemReadDTO>> SetQuantity(string userEmail, string userListName, string name, string brand, int quantity)
        {
            ResponseModel<ListItemReadDTO> response = new();

            try
            {
                var userListResponse = await HGetUserListFromNameAndEmail(userEmail, userListName);

                if (!userListResponse.Success)
                {
                    response.Success = false;
                    response.Message = userListResponse.Message;
                    return response;
                }

                ResponseModel<ListItemReadDTO> verifyPreExisting = await RetrieveByUserListAndNameAndBrand(userEmail, userListName, name, brand);

                if (!verifyPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to find the specified List Item to update quantity. Message: {verifyPreExisting.Message}";
                    LILogger.LogError($"Failed to find List Item in list of User with Email: {userEmail} with name: {userListName} with Name: {name} and Brand: {brand} to update quantity. Message: {verifyPreExisting.Message}");
                    return response;
                }

                //need actual List Item object now, not ListItemDTO.
                var updatedListItem = await Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        .Where(li => li.UserListId == userListResponse.Data.Id &&
                                li.Name == name &&
                                li.Brand == brand)
                        .FirstOrDefaultAsync();

                updatedListItem.Quantity = quantity;

                Context.ListItems.Update(updatedListItem);
                await Context.SaveChangesAsync();

                response.Data = new ListItemReadDTO(updatedListItem);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to update the specified List Item's quantity.";
                LILogger.LogError($"An error occured while attempting to update List Item's quantity in list of User with Email: {userEmail} with name: {userListName} with Name: {name} and Brand: {brand}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully updated the specified List Item's quantity.";
            return response;
        }

        public async Task<ResponseModel<ListItemReadDTO>> SetActiveStatus(string userEmail, string userListName, string name, string brand)
        {
            ResponseModel<ListItemReadDTO> response = new();

            try
            {
                var userListResponse = await HGetUserListFromNameAndEmail(userEmail, userListName);

                if (!userListResponse.Success)
                {
                    response.Success = false;
                    response.Message = userListResponse.Message;
                    return response;
                }

                ResponseModel<ListItemReadDTO> verifyPreExisting = await RetrieveByUserListAndNameAndBrand(userEmail, userListName, name, brand);

                if (!verifyPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to find List Item with Name: {name} and Brand: {brand} to activate. Message: {verifyPreExisting.Message}";
                    LILogger.LogError($"Failed to find List Item in list of User with Email: {userEmail} with name: {userListName} with Name: {name} and Brand: {brand} to set active status. Message: {verifyPreExisting.Message}");
                    return response;
                }

                //need actual List Item object now, not ListItemDTO.
                var activatedStatusListItem = await Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        .Where(li => li.UserListId == userListResponse.Data.Id &&
                                li.Name == name &&
                                li.Brand == brand)
                        .FirstOrDefaultAsync();

                activatedStatusListItem.Status = Status.active;
                activatedStatusListItem.DeletedDate = DateTime.MaxValue;

                Context.ListItems.Update(activatedStatusListItem);
                await Context.SaveChangesAsync();

                response.Data = new ListItemReadDTO(activatedStatusListItem);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to activate List Item with Name: {name} and Brand: {brand}.";
                LILogger.LogError($"An error occured while attempting to set active status for List Item in list of User with Email: {userEmail} with name: {userListName} with Name: {name} and Brand: {brand}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully activated List Item with Name: {name} and Brand: {brand}.";
            return response;
        }

        public async Task<ResponseModel<ListItemReadDTO>> SetDeletedStatus(string userEmail, string userListName, string name, string brand)
        {
            ResponseModel<ListItemReadDTO> response = new();

            try
            {
                var userListResponse = await HGetUserListFromNameAndEmail(userEmail, userListName);

                if (!userListResponse.Success)
                {
                    response.Success = false;
                    response.Message = userListResponse.Message;
                    return response;
                }

                ResponseModel<ListItemReadDTO> verifyPreExisting = await RetrieveByUserListAndNameAndBrand(userEmail, userListName, name, brand);

                if (!verifyPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to find List Item with Name: {name} and Brand: {brand} to delete. Message: {verifyPreExisting.Message}";
                    LILogger.LogError($"Failed to find List Item in list of User with Email: {userEmail} with name: {userListName} with Name: {name} and Brand: {brand} to set deleted status. Message: {verifyPreExisting.Message}");
                    return response;
                }

                //need actual List Item object now, not ListItemDTO.
                var deletedStatusListItem = await Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        .Where(li => li.UserListId == userListResponse.Data.Id &&
                                li.Name == name &&
                                li.Brand == brand)
                        .FirstOrDefaultAsync();

                deletedStatusListItem.Status = Status.deleted;

                deletedStatusListItem.DeletedDate = DateTime.UtcNow;

                Context.ListItems.Update(deletedStatusListItem);
                await Context.SaveChangesAsync();

                response.Data = new ListItemReadDTO(deletedStatusListItem);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to delete List Item with Name: {name} and Brand: {brand}.";
                LILogger.LogError($"An error occured while attempting to set deleted status for Item in list of User with Email: {userEmail} with name: {userListName} with Name: {name} and Brand: {brand}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully deleted List Item with Name: {name} and Brand: {brand}.";
            return response;
        }

        public async Task<ResponseModel<ListItemReadDTO>> Delete(string userEmail, string userListName, string name, string brand)
        {
            ResponseModel<ListItemReadDTO> response = new();

            try
            {
                var userListResponse = await HGetUserListFromNameAndEmail(userEmail, userListName);

                if (!userListResponse.Success)
                {
                    response.Success = false;
                    response.Message = userListResponse.Message;
                    return response;
                }

                ResponseModel<ListItemReadDTO> verifyPreExisting = await RetrieveByUserListAndNameAndBrand(userEmail, userListName, name, brand);

                if (!verifyPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to find List Item with Name: {name} and Brand: {brand} to permanently delete. Message: {verifyPreExisting.Message}";
                    LILogger.LogError($"Failed to find List Item in list of User with Email: {userEmail} with name: {userListName} with Name: {name} and Brand: {brand} to permanently delete. Message: {verifyPreExisting.Message}");
                    return response;
                }

                //need actual List Item object now, not ListItemDTO.
                var deletedListItem = await Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        .Where(li => li.UserListId == userListResponse.Data.Id &&
                                li.Name == name &&
                                li.Brand == brand)
                        .FirstOrDefaultAsync();

                if (deletedListItem.Status != Status.deleted)
                {
                    response.Success = false;
                    response.Message = $"List Item permanent deletion failed, not marked for deletion.";
                    LILogger.LogError($"List Item status was not set to deleted when attempting to delete permanently.");
                    return response;
                }

                Context.ListItems.Remove(deletedListItem);
                await Context.SaveChangesAsync();

                ResponseModel<ListItemReadDTO> verifyDeleted = await RetrieveById(deletedListItem.Id);

                if (verifyDeleted.Success)
                {
                    response.Success = false;
                    response.Message = $"List Item permanent deletion failed. Message: {verifyDeleted.Message}";
                    LILogger.LogError($"List Item permanent deletion from the Database failed. Message: {verifyDeleted.Message}");
                    return response;
                }

                //update item tags' status to inactive if admin created or to deleted if user created
                //if any were removed from all item templates and list items.
                var noItemsItemTags = Context.ItemTags.Where(it => !it.Items.Any());
                foreach (ItemTagModel itemTag in noItemsItemTags)
                {
                    itemTag.Status = itemTag.Pinned ? Status.inactive : Status.deleted;
                }

                await Context.SaveChangesAsync();

                response.Data = new ListItemReadDTO(deletedListItem);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to permanently delete List Item with Name: {name} and Brand: {brand}.";
                LILogger.LogError($"An error occured while attempting to permanently delete List Item in list of User with Email: {userEmail} with name: {userListName} with Name: {name} and Brand: {brand}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully permanently deleted List Item with Name: {name} and Brand: {brand}.";
            return response;
        }

        //helper methods
        private async Task<ResponseModel<UserListModel>> HGetUserListFromNameAndEmail(string userEmail, string userListName)
        {
            ResponseModel<UserListModel> response = new();

            try
            {
                var user = await Context.Users
                            .Where(u => u.Email == userEmail)
                            .FirstOrDefaultAsync();

                if (user == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find a User with the specified email address.";
                    LILogger.LogError($"Could not find a User with Email: {userEmail}.");
                    return response;
                }

                var userList = await Context.UserLists
                        .Where(ul => ul.UserId == user.Id &&
                                ul.Name == userListName)
                        .FirstOrDefaultAsync();

                if (userList == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find a User List with Name: {userListName} for this user.";
                    LILogger.LogError($"Could not find a User List with Name: {userListName} for user with Email: {userEmail}.");
                    return response;
                }

                response.Data = userList;
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find User by Email and User List by Name.";
                LILogger.LogError($"An error occured while attempting to find User by Email: {userEmail} and User List by Name: {userListName}. Message: {ex.Message}");
                return response;
            }

            return response;
        }
    }
}