using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using KitchenManager.API.ItemsNS.ItemTemplatesNS;
using KitchenManager.API.ItemsNS.ListItemsNS;
using KitchenManager.API.ItemTagsNS;
using KitchenManager.API.UsersNS;
using KitchenManager.API.UserListsNS;
using KitchenManager.API.SharedNS.StatusNS;
using KitchenManager.API.IconsNS;
using KitchenManager.API.SharedNS.ClaimsNS;

namespace KitchenManager.API.Data.Seed
{
    public class KMSeeder
    {
        private readonly KMDbContext Context;
        private readonly UserManager<UserModel> UserManager;
        private readonly ILogger<KMSeeder> SLogger;
        public string RootPath { get; set; }
        public Random Random;

        public KMSeeder(KMDbContext context, UserManager<UserModel> userManager, ILogger<KMSeeder> sLogger)
        {
            Context = context;
            UserManager = userManager;
            SLogger = sLogger;
            RootPath = string.Empty;
            Random = new Random();
    }

        public async Task SeedDataAsync(bool clearData)
        {
            //seed data

            if (clearData)
            {
                await Context.Database.EnsureDeletedAsync();
                SLogger.LogInformation("Database deleted.");
            }

            await Context.Database.EnsureCreatedAsync();
            SLogger.LogInformation("Database Exists or Created.");

            if (!Context.Users.Any())
            {
                //Add Users
                var filePathUser = Path.Combine(RootPath, "API/Data/Seed/SeedData/SeedUsers.json");
                var jsonUser = File.ReadAllText(filePathUser);
                var userData = JsonSerializer.Deserialize<IEnumerable<SeedUserModel>>(jsonUser);

                foreach (var seedUserModel in userData)
                {
                    UserModel user = await UserManager.FindByEmailAsync(seedUserModel.Email);
                    if (user is null)
                    {
                        user = new UserModel()
                        {
                            UserName = seedUserModel.Email,
                            Email = seedUserModel.Email,
                            NormalizedEmail = seedUserModel.Email.ToUpperInvariant(),
                            NormalizedUserName = seedUserModel.Email.ToUpperInvariant(),
                            PhoneNumber = $"{Random.Next(100, 1000)}-{Random.Next(100, 1000)}-{Random.Next(1000, 10000)}",
                            FirstName = seedUserModel.FirstName,
                            LastName = seedUserModel.LastName,
                            Birthday = DateTime.Today.AddDays(-Random.Next(18*365, 65*365))
                        };

                        var result = await UserManager.CreateAsync(user, "Password!1");
                        if (result != IdentityResult.Success)
                        {
                            throw new InvalidOperationException("Could not create a new User in seeder");
                        }

                        var newUser = await UserManager.FindByEmailAsync(user.Email);
                        if (newUser is null)
                        {
                            throw new InvalidOperationException("Cant find new User in seeder");
                        }

                        if (seedUserModel.Role == KMRoleClaimValues.SuperAdmin)
                        {
                            await Context.UserClaims.AddAsync(new IdentityUserClaim<int>()
                            {
                                UserId = newUser.Id,
                                ClaimType = KMClaimTypes.Role,
                                ClaimValue = KMRoleClaimValues.SuperAdmin
                            });
                        }

                        if (seedUserModel.Role == KMRoleClaimValues.SuperAdmin || seedUserModel.Role == KMRoleClaimValues.Admin)
                        {
                            await Context.UserClaims.AddAsync(new IdentityUserClaim<int>()
                            {
                                UserId = newUser.Id,
                                ClaimType = KMClaimTypes.Role,
                                ClaimValue = KMRoleClaimValues.Admin
                            });
                        }

                        await Context.UserClaims.AddAsync(new IdentityUserClaim<int>()
                        {
                            UserId = newUser.Id,
                            ClaimType = KMClaimTypes.Role,
                            ClaimValue = KMRoleClaimValues.User
                        });
                    }
                }

                await Context.SaveChangesAsync();
            }

            if (!(await Context.ItemTags.AnyAsync()))
            {
                //Add Item Tags
                await Context.ItemTags.AddAsync(new ItemTagModel() { Name = "Fruit", Pinned = true });
                await Context.ItemTags.AddAsync(new ItemTagModel() { Name = "Vegitable", Pinned = true });
                await Context.ItemTags.AddAsync(new ItemTagModel() { Name = "Poultry", Pinned = true });
                await Context.ItemTags.AddAsync(new ItemTagModel() { Name = "Fish", Pinned = true });
                await Context.ItemTags.AddAsync(new ItemTagModel() { Name = "Meat", Pinned = true });
                await Context.ItemTags.AddAsync(new ItemTagModel() { Name = "Leftovers", Pinned = true });
                await Context.ItemTags.AddAsync(new ItemTagModel() { Name = "Spice", Pinned = true });

                await Context.SaveChangesAsync();
            }

            if (!(await Context.Icons.AnyAsync()))
            {
                //Add test Icons
                await Context.Icons.AddAsync(new IconModel() { Name = "Icon1", Path = "Icon1 Path" });
                await Context.Icons.AddAsync(new IconModel() { Name = "Icon2", Path = "Icon2 Path" });
                await Context.Icons.AddAsync(new IconModel() { Name = "Icon3", Path = "Icon3 Path" });
                await Context.Icons.AddAsync(new IconModel() { Name = "Icon4", Path = "Icon4 Path" });
                await Context.Icons.AddAsync(new IconModel() { Name = "Icon5", Path = "Icon5 Path" });
                await Context.Icons.AddAsync(new IconModel() { Name = "Icon6", Path = "Icon6 Path" });
                await Context.Icons.AddAsync(new IconModel() { Name = "Icon7", Path = "Icon7 Path" });

                await Context.SaveChangesAsync();
            }

            if (!(await Context.ItemTemplates.AnyAsync()))
            {
                //Add ItemTemplates
                var filePathItemTemplate = Path.Combine(RootPath, "API/Data/Seed/SeedData/SeedItemTemplates.json");
                var jsonItemTemplate = File.ReadAllText(filePathItemTemplate);
                var itemTemplateData = JsonSerializer.Deserialize<IEnumerable<SeedItemTemplateModel>>(jsonItemTemplate);

                foreach (var itemTemplateSeedModel in itemTemplateData)
                {
                    var itemTemplate = new ItemTemplateModel()
                    {
                        Name = itemTemplateSeedModel.Name,
                        Brand = itemTemplateSeedModel.Brand,
                        Description = itemTemplateSeedModel.Description,
                        ExpirationDays = itemTemplateSeedModel.ExpirationDays,
                        Icon = Context.Icons.OrderBy(g => Guid.NewGuid()).FirstOrDefault(),
                        ItemTags = Context.ItemTags.OrderBy(g => Guid.NewGuid()).Take(Random.Next(0, 3)).ToList()
                    };

                    await Context.ItemTemplates.AddAsync(itemTemplate);
                }

                await Context.SaveChangesAsync();
            }

            if (!(await Context.UserLists.AnyAsync()))
            {
                var users = await Context.Users.ToListAsync();
                foreach (var user in users)
                {
                    //Add 1 or 2 lists to each user
                    for (int i = 1; i < Random.Next(2, 4); i++)
                    {
                        var firstName = (await Context.UserClaims.Where(uc => uc.UserId == user.Id).FirstOrDefaultAsync()).ClaimValue;

                        var userList = new UserListModel()
                        {
                            Name = $"{firstName}'s list #{i}",
                            Description = "A list of random fake ingredient items.",
                            UserId = user.Id,
                            Icon = Context.Icons.OrderBy(g => Guid.NewGuid()).FirstOrDefault(),
                        };

                        await Context.UserLists.AddAsync(userList);
                    }
                }

                await Context.SaveChangesAsync();
            }

            if (!(await Context.ListItems.AnyAsync()))
            {
                //Add ListItems
                var userLists = await Context.UserLists.ToListAsync();
                foreach (var userList in userLists)
                {
                    //add random number of items between 2 and 5 to each list
                    for (int i = 0; i < Random.Next(2, 6); i++)
                    {
                        //get random itemTemplate
                        var itemTemplate = Context.ItemTemplates.OrderBy(g => Guid.NewGuid()).FirstOrDefault();

                        var listItem = new ListItemModel()
                        {
                            UserListId = userList.Id,
                            Name = itemTemplate.Name,
                            Brand = itemTemplate.Brand,
                            Description = itemTemplate.Description,
                            Quantity = Random.Next(1, 6),
                            ExpirationDate = itemTemplate.ExpirationDays > 0 ? DateTime.UtcNow.AddDays(itemTemplate.ExpirationDays).Date : DateTime.MaxValue,
                            Icon = itemTemplate.Icon,
                            ItemTags = itemTemplate.ItemTags
                        };

                        await Context.ListItems.AddAsync(listItem);
                    }
                }

                await Context.SaveChangesAsync();
            }

            //set item tag status to active for each one that was assigned to an item.
            var activateTags = await Context.ItemTags.Where(it => it.Items.Any()).ToListAsync();
            foreach (ItemTagModel itemTag in activateTags)
            {
                itemTag.Status = Status.active;
            }

            await Context.SaveChangesAsync();
        }
    }
}
