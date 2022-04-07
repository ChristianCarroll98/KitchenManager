using KitchenManager.API.Data;
using KitchenManager.API.IconsNS;
using KitchenManager.API.ItemsNS.ItemTemplatesNS;
using KitchenManager.API.ItemsNS.ItemTemplatesNS.DTO;
using KitchenManager.API.ItemsNS.ItemTemplatesNS.Repo;
using KitchenManager.API.ItemsNS.ListItemsNS.DTO;
using KitchenManager.API.ItemTagsNS;
using KitchenManager.API.SharedNS.ResponseNS;
using KitchenManager.API.SharedNS.StatusNS;
using KitchenManager.API.UserListsNS;
using KitchenManager.API.UserListsNS.DTO;
using KitchenManager.API.UsersNS;
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
        Task<Response<ListItemDTO>> RetrieveById(int id);
        Task<Response<List<ListItemDTO>>> RetrieveByUserListAndBrand(string userName, string userListName, string brand);
        Task<Response<ListItemDTO>> RetrieveByUserListAndNameAndBrand(string userName, string userListName, string name, string brand);
        Task<Response<List<ListItemDTO>>> RetrieveByUserListAndStatus(string userName, string userListName, Status status);
        Task<Response<List<ListItemDTO>>> RetrieveByUserListAndTags(string userName, string userListName, List<string> tagNames);
        Task<Response<List<ListItemDTO>>> RetrieveByUserList(string userName, string userListName);

        Task<Response<ListItemDTO>> Create(string userName, string userListName, ListItemDTO model);
        Task<Response<ListItemDTO>> CreateFromItemTemplate(string userName, string userListName, ItemTemplateDTO model);
        Task<Response<ListItemDTO>> Update(string userName, string userListName, string originalName, string originalBrand, ListItemDTO model);
        Task<Response<ListItemDTO>> SetQuantity(string userName, string userListName, string name, string brand, int quantity);
        Task<Response<ListItemDTO>> SetActiveStatus(string userName, string userListName, string name, string brand);
        Task<Response<ListItemDTO>> SetDeletedStatus(string userName, string userListName, string name, string brand);
        Task<Response<ListItemDTO>> Delete(string userName, string userListName, string name, string brand);
    }

    public class ListItemRepository : IListItemRepository
    {
        private readonly KMDbContext Context;
        private readonly ILogger<ListItemRepository> LILogger;
        private readonly IItemTemplateRepository ITRepo;

        public ListItemRepository(IItemTemplateRepository iTRepo, KMDbContext context, ILogger<ListItemRepository> lILogger)
        {
            ITRepo = iTRepo;
            Context = context;
            LILogger = lILogger;
        }

        public async Task<Response<ListItemDTO>> RetrieveById(int id)
        {
            Response<ListItemDTO> response = new();

            try
            {   
                var listItem = await Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        ///TODO \/
                        //.Include(li => li.UserList).ThenInclude(ul => ul.Name).ThenInclude(u => u.UserName) 
                        //.Include(li => li.UserList).ThenInclude(ul => ul.Name)
                        .Where(li => li.Id == id)
                        .FirstOrDefaultAsync();

                if (listItem == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find List Item with the specified Id.";
                    LILogger.LogError($"Could not find List Item with Id: {id}");
                    return response;
                }

                response.Data = new ListItemDTO(listItem);
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

        public async Task<Response<List<ListItemDTO>>> RetrieveByUserListAndBrand(string userName, string userListName, string brand)
        {
            Response<List<ListItemDTO>> response = new();

            try
            {
                var listItems = await Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        .Where(li => li.UserList.User.UserName == userName &&
                                li.UserList.Name == userListName &&
                                li.Brand == brand &&
                                li.Status == Status.active)
                        .ToListAsync();

                if (!listItems.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any List Items with Brand: {brand}";
                    LILogger.LogError($"Could not find any Items on {userName}'s list: {userListName} with Brand: {brand}");
                    return response;
                }

                response.Data = listItems.Select(li => new ListItemDTO(li)).ToList();
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find List Items with Brand: {brand}.";
                LILogger.LogError($"An error occured while attempting to find Items on {userName}'s list: {userListName} with Brand: {brand}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved List Items with Brand: {brand}.";
            return response;
        }

        public async Task<Response<ListItemDTO>> RetrieveByUserListAndNameAndBrand(string userName, string userListName, string name, string brand)
        {
            Response<ListItemDTO> response = new();

            try
            {
                var listItem = await Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        .Where(li => li.UserList.User.UserName == userName &&
                                li.UserList.Name == userListName &&
                                li.Name == name &&
                                li.Brand == brand)
                        .FirstOrDefaultAsync();

                if (listItem == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find List Item with Name: {name} and Brand: {brand}.";
                    LILogger.LogError($"Could not find List Item with Name: {name} and Brand: {brand} on {userName}'s list: {userListName}.");
                    return response;
                }

                response.Data = new ListItemDTO(listItem);
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find List Item with Name: {name} and Brand: {brand}.";
                LILogger.LogError($"An error occured while attempting to find List Item with Name: {name} and Brand: {brand} for User: {userName}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved List Item with the specified status.";
            return response;
        }

        public async Task<Response<List<ListItemDTO>>> RetrieveByUserListAndStatus(string userName, string userListName, Status status)
        {
            Response<List<ListItemDTO>> response = new();

            try
            {
                var listItems = await Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        .Where(li => li.UserList.User.UserName == userName &&
                                li.UserList.Name == userListName &&
                                li.Status == status)
                        .ToListAsync();

                if (!listItems.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any List Items with the specified status.";
                    LILogger.LogError($"Could not find any Items on {userName}'s list: {userListName} with status: {status}.");
                    return response;
                }

                response.Data = listItems.Select(li => new ListItemDTO(li)).ToList();
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find List Items with the specified status.";
                LILogger.LogError($"An error occured while attempting to find Items on {userName}'s list: {userListName} with status: {status}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved List Items with the specified status.";
            return response;
        }

        public async Task<Response<List<ListItemDTO>>> RetrieveByUserListAndTags(string userName, string userListName, List<string> tagNames)
        {
            Response<List<ListItemDTO>> response = new();

            try
            {
                List<ListItem> listItems;

                if (tagNames.Any())
                {
                    listItems = await Context
                            .ListItems
                            .Include(li => li.ItemTags)
                            .Include(li => li.Icon)
                            .Where(li => li.UserList.User.UserName == userName &&
                                    li.UserList.Name == userListName &&
                                    li.ItemTags
                                            .Select(it => it.Name)
                                            .Any(itn => tagNames
                                                    .Contains(itn)) &&
                                    li.Status == Status.active)
                            .ToListAsync();
                }
                else
                {
                    listItems = await Context
                            .ListItems
                            .Include(li => li.ItemTags)
                            .Include(li => li.Icon)
                            .Where(li => li.UserList.User.UserName == userName &&
                                    li.UserList.Name == userListName &&
                                    !li.ItemTags.Any() &&
                                    li.Status == Status.active)
                            .ToListAsync();
                }

                if (!listItems.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any List Items with tags: {string.Join(", ", tagNames)}.";
                    LILogger.LogError($"Could not find any List Items on {userName}'s list: {userListName} with tags: {string.Join(", ", tagNames)}");
                    return response;
                }

                response.Data = listItems.Select(it => new ListItemDTO(it)).ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find List Items with tags: {string.Join(", ", tagNames)}.";
                LILogger.LogError($"An error occured while attempting to find List Items on {userName}'s list: {userListName} with tags: {string.Join(", ", tagNames)}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved List Items with the specified tags: {string.Join(", ", tagNames)}.";
            return response;
        }

        public async Task<Response<List<ListItemDTO>>> RetrieveByUserList(string userName, string userListName)
        {
            Response<List<ListItemDTO>> response = new();

            try
            {
                var listItems = await Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        .Where(li => li.UserList.User.UserName == userName &&
                                li.UserList.Name == userListName &&
                                li.Status == Status.active)
                        .ToListAsync();

                if (!listItems.Any())
                {
                    response.Success = false;
                    response.Message = $"Could not find any List Items.";
                    LILogger.LogError($"Could not find any Items on {userName}'s list: {userListName}.");
                    return response;
                }

                response.Data = listItems.Select(li => new ListItemDTO(li)).ToList();
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to find List Items.";
                LILogger.LogError($"An error occured while attempting to find Items on {userName}'s list: {userListName}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully retrieved List Items.";
            return response;
        }

        public async Task<Response<ListItemDTO>> Create(string userName, string userListName, ListItemDTO model)
        {
            Response<ListItemDTO> response = new();

            try
            {
                UserList userList = await Context.UserLists.Where(ul => ul.User.UserName == userName && ul.Name == userListName).FirstOrDefaultAsync();

                if (userList == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find specified User List.";
                    LILogger.LogError($"User {userName} has no list: {userListName}.");
                    return response;
                }

                Response<ItemTemplateDTO> itemTemplateCheck = ITRepo.RetrieveByNameAndBrand(model.Name, model.Brand).Result;

                if (itemTemplateCheck.Success)
                {
                    LILogger.LogInformation($"There is an Item Template with Name: {model.Name} and Brand: {model.Brand} already. Redirecting to CreateFromItemTemplate method.");
                    return CreateFromItemTemplate(userName, userListName, itemTemplateCheck.Data).Result;
                }

                Response<ListItemDTO> checkPreExisting = await RetrieveByUserListAndNameAndBrand(userName, userListName, model.Name, model.Brand);

                if (checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"A List Item already exists in this list with Name: {model.Name} and Brand: {model.Brand}. Message: {checkPreExisting.Message}";
                    response.Data = checkPreExisting.Data;
                    LILogger.LogError($"A List Item already exists in {userName}'s list: {userListName} with Name: {model.Name} and Brand: {model.Brand}. Message: {checkPreExisting.Message}");
                    return response;
                }

                var newListItem = new ListItem()
                {
                    Name = model.Name,
                    Brand = model.Brand,
                    Description = model.Description,
                    ExpirationDate = model.ExpirationDate,
                    Quantity = model.Quantity,
                    UserList = userList
                };

                //sets icon to the pre-existing icon from model in DB if it exists otherwise creates a new one.
                newListItem.Icon = await Context.Icons.Where(i => i.Name == model.IconDTO.Name).FirstOrDefaultAsync() ??
                        new Icon() { Name = model.IconDTO.Name, Path = model.IconDTO.Path };

                //add pre-existing item tags from model
                newListItem.ItemTags.AddRange(await Context.ItemTags
                        .Where(it => model.ItemTagDTOs
                                .Select(itdto => itdto.Name)
                                .Contains(it.Name))
                        .ToListAsync());

                //add new item tags from model
                newListItem.ItemTags.AddRange(model.ItemTagDTOs
                        .Where(itdto => !Context.ItemTags
                                .Select(it => it.Name)
                                .Contains(itdto.Name))
                        .Select(itdto => new ItemTag() { Name = itdto.Name })
                        .ToList());

                //make sure inactive tags are set to active when assigned to this list item.
                var inactiveTags = newListItem.ItemTags.Where(it => it.Status != Status.active).ToList();
                foreach (ItemTag itemTag in inactiveTags)
                {
                    itemTag.Status = Status.active;
                }

                //create item template from item since it was not previously a template.
                var newItemTemplate = new ItemTemplate()
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
                await Context.ListItems.AddAsync(newListItem);
                await Context.SaveChangesAsync();

                Response<ListItemDTO> checkAdded = await RetrieveByUserListAndNameAndBrand(userName, userListName, model.Name, model.Brand);

                if (!checkAdded.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to save the new List Item. Message: {checkAdded.Message}";
                    LILogger.LogError($"Failed to add new List Item to Database in {userName}'s list: {userListName}. Message: {checkAdded.Message}");
                    return response;
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

        public async Task<Response<ListItemDTO>> CreateFromItemTemplate(string userName, string userListName, ItemTemplateDTO itemTemplateModel)
        {
            Response<ListItemDTO> response = new();

            try
            {
                UserList userList = await Context.UserLists.Where(ul => ul.User.UserName == userName && ul.Name == userListName).FirstOrDefaultAsync();

                if (userList == null)
                {
                    response.Success = false;
                    response.Message = $"Could not find specified User List.";
                    LILogger.LogError($"User {userName} has no list: {userListName}.");
                    return response;
                }

                Response<ListItemDTO> checkPreExisting = await RetrieveByUserListAndNameAndBrand(userName, userListName, itemTemplateModel.Name, itemTemplateModel.Brand);

                if (checkPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"A List Item already exists in this list with Name: {itemTemplateModel.Name} and Brand: {itemTemplateModel.Brand}. Message: {checkPreExisting.Message}";
                    response.Data = checkPreExisting.Data;
                    LILogger.LogError($"A List Item already exists in {userName}'s list: {userListName} with Name: {itemTemplateModel.Name} and Brand: {itemTemplateModel.Brand}. Message: {checkPreExisting.Message}");
                    return response;
                }

                Response<ItemTemplateDTO> checkItemTemplatePreExisting = await ITRepo.RetrieveByNameAndBrand(itemTemplateModel.Name, itemTemplateModel.Brand);

                if (!checkItemTemplatePreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"No Item Template with Name: {itemTemplateModel.Name} and Brand: {itemTemplateModel.Brand} exists. Message: {checkPreExisting.Message}";
                    response.Data = checkPreExisting.Data;
                    LILogger.LogError($"No Item Template with Name: {itemTemplateModel.Name} and Brand: {itemTemplateModel.Brand} exists.");
                    return response;
                }

                var newListItem = new ListItem()
                {
                    Name = itemTemplateModel.Name,
                    Brand = itemTemplateModel.Brand,
                    Description = itemTemplateModel.Description,
                    Quantity = 1,
                    ExpirationDate = DateTime.UtcNow.AddDays(itemTemplateModel.ExpirationDays).Date,
                    UserList = userList
                };

                //sets icon to the pre-existing icon from model in DB if it exists otherwise creates a new one.
                newListItem.Icon = await Context.Icons.Where(i => i.Name == itemTemplateModel.IconDTO.Name).FirstOrDefaultAsync() ??
                        new Icon() { Name = itemTemplateModel.IconDTO.Name, Path = itemTemplateModel.IconDTO.Path };

                //add pre-existing item tags from model
                newListItem.ItemTags.AddRange(await Context.ItemTags
                        .Where(it => itemTemplateModel.ItemTagDTOs
                                .Select(itdto => itdto.Name)
                                .Contains(it.Name))
                        .ToListAsync());

                //add new item tags from model
                newListItem.ItemTags.AddRange(itemTemplateModel.ItemTagDTOs
                        .Where(itdto => !Context.ItemTags
                                .Select(it => it.Name)
                                .Contains(itdto.Name))
                        .Select(itdto => new ItemTag() { Name = itdto.Name })
                        .ToList());

                //make sure inactive tags are set to active when assigned to this list item.
                var inactiveTags = newListItem.ItemTags.Where(it => it.Status != Status.active).ToList();
                foreach (ItemTag itemTag in inactiveTags)
                {
                    itemTag.Status = Status.active;
                }

                await Context.ListItems.AddAsync(newListItem);
                await Context.SaveChangesAsync();

                Response<ListItemDTO> checkAdded = await RetrieveByUserListAndNameAndBrand(userName, userListName, itemTemplateModel.Name, itemTemplateModel.Brand);

                if (!checkAdded.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to add the new List Item from Template. Message: {checkAdded.Message}";
                    LILogger.LogError($"Failed to add new List Item from Template to Database in {userName}'s list: {userListName}. Message: {checkAdded.Message}");
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

        public async Task<Response<ListItemDTO>> Update(string userName, string userListName, string originalName, string originalBrand, ListItemDTO model)
        {
            Response<ListItemDTO> response = new();

            try
            {
                Response<ListItemDTO> verifyPreExisting = await RetrieveByUserListAndNameAndBrand(userName, userListName, originalName, originalBrand);

                if (!verifyPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to find the specified List Item to update. Message: {verifyPreExisting.Message}";
                    LILogger.LogError($"Failed to find List Item in {userName}'s list: {userListName} with original Name: {originalName} and original Brand: {originalBrand} to update. Message: {verifyPreExisting.Message}");
                    return response;
                }

                //need actual List Item object now, not ListItemDTO.
                var updatedListItem = await Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        .Where(li => li.Name == originalName
                                && li.Brand == originalBrand)
                        .FirstOrDefaultAsync();

                updatedListItem.Name = model.Name;

                updatedListItem.Brand = model.Brand;

                updatedListItem.Description = model.Description;

                updatedListItem.ExpirationDate = model.ExpirationDate;

                //sets icon to the pre-existing icon from model in DB if it exists otherwise creates a new one.
                updatedListItem.Icon = await Context.Icons.Where(i => i.Name == model.IconDTO.Name).FirstOrDefaultAsync() ??
                        new Icon() { Name = model.IconDTO.Name, Path = model.IconDTO.Path };

                updatedListItem.ItemTags.Clear();

                //add pre-existing item tags from model
                updatedListItem.ItemTags.AddRange(await Context.ItemTags
                        .Where(it => model.ItemTagDTOs
                                .Select(itdto => itdto.Name)
                                .Contains(it.Name))
                        .ToListAsync());

                //add new item tags from model
                updatedListItem.ItemTags.AddRange(model.ItemTagDTOs
                        .Where(itdto => !Context.ItemTags
                                .Select(it => it.Name)
                                .Contains(itdto.Name))
                        .Select(itdto => new ItemTag() { Name = itdto.Name })
                        .ToList());

                //make sure inactive tags are set to active when assigned to this list item.
                var inactiveTags = updatedListItem.ItemTags.Where(it => it.Status != Status.active).ToList();
                foreach (ItemTag itemTag in inactiveTags)
                {
                    itemTag.Status = Status.active;
                }

                Context.ListItems.Update(updatedListItem);
                await Context.SaveChangesAsync();

                //update item tags' status to inactive if admin created or to deleted if user created
                //if any were removed from all item templates and list items.
                var noItemsItemTags = await Context.ItemTags.Where(it => !it.Items.Any()).ToListAsync();
                foreach (ItemTag itemTag in noItemsItemTags)
                {
                    if (itemTag.UserCreated)
                    {
                        itemTag.Status = Status.deleted;
                    }
                    else
                    {
                        itemTag.Status = Status.inactive;
                    }
                }

                await Context.SaveChangesAsync();

                response.Data = new ListItemDTO(updatedListItem);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to update the specified List Item.";
                LILogger.LogError($"An error occured while attempting to update List Item in {userName}'s list: {userListName} with original Name: {originalName} and original Brand: {originalBrand}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully updated the specified List Item.";
            return response;
        }

        public async Task<Response<ListItemDTO>> SetQuantity(string userName, string userListName, string name, string brand, int quantity)
        {
            Response<ListItemDTO> response = new();

            try
            {
                Response<ListItemDTO> verifyPreExisting = await RetrieveByUserListAndNameAndBrand(userName, userListName, name, brand);

                if (!verifyPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to find the specified List Item to update quantity. Message: {verifyPreExisting.Message}";
                    LILogger.LogError($"Failed to find List Item in {userName}'s list: {userListName} with Name: {name} and Brand: {brand} to update quantity. Message: {verifyPreExisting.Message}");
                    return response;
                }

                //need actual List Item object now, not ListItemDTO.
                var updatedListItem = await Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        .Where(li => li.Name == name
                                && li.Brand == brand)
                        .FirstOrDefaultAsync();

                updatedListItem.Quantity = quantity;

                Context.ListItems.Update(updatedListItem);
                await Context.SaveChangesAsync();

                response.Data = new ListItemDTO(updatedListItem);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to update the specified List Item's quantity.";
                LILogger.LogError($"An error occured while attempting to update List Item's quantity in {userName}'s list: {userListName} with Name: {name} and Brand: {brand}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully updated the specified List Item's quantity.";
            return response;
        }

        public async Task<Response<ListItemDTO>> SetActiveStatus(string userName, string userListName, string name, string brand)
        {
            Response<ListItemDTO> response = new();

            try
            {
                Response<ListItemDTO> verifyPreExisting = await RetrieveByUserListAndNameAndBrand(userName, userListName, name, brand);

                if (!verifyPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to find List Item with Name: {name} and Brand: {brand} to activate. Message: {verifyPreExisting.Message}";
                    LILogger.LogError($"Failed to find List Item in {userName}'s list: {userListName} with Name: {name} and Brand: {brand} to set active status. Message: {verifyPreExisting.Message}");
                    return response;
                }

                //need actual List Item object now, not ListItemDTO.
                var activatedStatusListItem = await Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        .Where(it => it.Name == name
                                && it.Brand == brand)
                        .FirstOrDefaultAsync();

                activatedStatusListItem.Status = Status.active;
                activatedStatusListItem.DeletedDate = DateTime.MaxValue;

                Context.ListItems.Update(activatedStatusListItem);
                await Context.SaveChangesAsync();

                response.Data = new ListItemDTO(activatedStatusListItem);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to activate List Item with Name: {name} and Brand: {brand}.";
                LILogger.LogError($"An error occured while attempting to set active status for List Item in {userName}'s list: {userListName} with Name: {name} and Brand: {brand}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully activated List Item with Name: {name} and Brand: {brand}.";
            return response;
        }

        public async Task<Response<ListItemDTO>> SetDeletedStatus(string userName, string userListName, string name, string brand)
        {
            Response<ListItemDTO> response = new();

            try
            {
                Response<ListItemDTO> verifyPreExisting = await RetrieveByUserListAndNameAndBrand(userName, userListName, name, brand);

                if (!verifyPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to find List Item with Name: {name} and Brand: {brand} to delete. Message: {verifyPreExisting.Message}";
                    LILogger.LogError($"Failed to find List Item in {userName}'s list: {userListName} with Name: {name} and Brand: {brand} to set deleted status. Message: {verifyPreExisting.Message}");
                    return response;
                }

                //need actual List Item object now, not ListItemDTO.
                var deletedStatusListItem = await Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        .Where(it => it.Name == name
                                && it.Brand == brand)
                        .FirstOrDefaultAsync();

                deletedStatusListItem.Status = Status.deleted;

                deletedStatusListItem.DeletedDate = DateTime.UtcNow;

                Context.ListItems.Update(deletedStatusListItem);
                await Context.SaveChangesAsync();

                response.Data = new ListItemDTO(deletedStatusListItem);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to delete List Item with Name: {name} and Brand: {brand}.";
                LILogger.LogError($"An error occured while attempting to set deleted status for List Item in {userName}'s list: {userListName} with Name: {name} and Brand: {brand}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully deleted List Item with Name: {name} and Brand: {brand}.";
            return response;
        }

        public async Task<Response<ListItemDTO>> Delete(string userName, string userListName, string name, string brand)
        {
            Response<ListItemDTO> response = new();

            try
            {
                Response<ListItemDTO> verifyPreExisting = await RetrieveByUserListAndNameAndBrand(userName, userListName, name, brand);

                if (!verifyPreExisting.Success)
                {
                    response.Success = false;
                    response.Message = $"Failed to find List Item with Name: {name} and Brand: {brand} to permanently delete. Message: {verifyPreExisting.Message}";
                    LILogger.LogError($"Failed to find List Item in {userName}'s list: {userListName} with Name: {name} and Brand: {brand} to permanently delete. Message: {verifyPreExisting.Message}");
                    return response;
                }

                //need actual List Item object now, not ListItemDTO.
                var deletedListItem = await Context
                        .ListItems
                        .Include(li => li.ItemTags)
                        .Include(li => li.Icon)
                        .Where(it => it.Name == name
                                && it.Brand == brand)
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

                Response<ListItemDTO> verifyDeleted = await RetrieveById(deletedListItem.Id);

                if (verifyDeleted.Success)
                {
                    response.Success = false;
                    response.Message = $"List Item permanent deletion failed. Message: {verifyDeleted.Message}";
                    LILogger.LogError($"List Item permanent deletion from the Database failed. Message: {verifyDeleted.Message}");
                    return response;
                }

                //update item tags' status if admin created or delete if user created
                //if any were removed from all item templates and list items.
                var noItemsItemTags = await Context.ItemTags.Where(it => !it.Items.Any()).ToListAsync();
                foreach (ItemTag itemTag in noItemsItemTags)
                {
                    if (itemTag.UserCreated)
                    {
                        itemTag.Status = Status.deleted;
                    }
                    else
                    {
                        itemTag.Status = Status.inactive;
                    }
                }

                await Context.SaveChangesAsync();

                response.Data = new ListItemDTO(deletedListItem);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occured while attempting to permanently delete List Item with Name: {name} and Brand: {brand}.";
                LILogger.LogError($"An error occured while attempting to permanently delete List Item in {userName}'s list: {userListName} with Name: {name} and Brand: {brand}. Message: {ex.Message}");
                return response;
            }

            response.Message = $"Successfully permanently deleted List Item with Name: {name} and Brand: {brand}.";
            return response;
        }
    }
}