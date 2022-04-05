using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

using KitchenManager.API.ItemsNS.ItemTemplatesNS;
using KitchenManager.API.ItemsNS.ListItemsNS;
using KitchenManager.API.ItemTagsNS;
using KitchenManager.API.UsersNS;
using KitchenManager.API.UserListsNS;
using KitchenManager.API.SharedNS.StatusNS;

namespace KitchenManager.API.Data.Seed
{
    public class KMSeeder
    {
        private readonly KMDbContext Context;
        private readonly UserManager<User> UserManager;
        private readonly RoleManager<IdentityRole<int>> RoleManager;
        public string RootPath { get; set; }
        public Random Random;

        public KMSeeder(KMDbContext context, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            Context = context;
            UserManager = userManager;
            RoleManager = roleManager;
            RootPath = string.Empty;
            Random = new Random();
    }

        public async Task SeedDataAsync(bool clearData)
        {
            //seed data
            
            if (clearData)
            {
                await Context.Database.EnsureDeletedAsync();
                Console.Write("Database deleted.");
            }
            
            await Context.Database.EnsureCreatedAsync();
            Console.Write("Database Exists or Created.");

            if (!(await Context.Roles.AnyAsync()))
            {
                await RoleManager.CreateAsync(new IdentityRole<int>() { Name = "Admin" });
                await RoleManager.CreateAsync(new IdentityRole<int>() { Name = "User" });

                await Context.SaveChangesAsync();
            }
            
            if (!Context.Users.Any())
            {
                //Add Admins
                var filePathAdmin = Path.Combine(RootPath, "API/Data/Seed/SeedData/SeedAdmins.json");
                var jsonAdmin = File.ReadAllText(filePathAdmin);
                var adminData = JsonSerializer.Deserialize<IEnumerable<KMUserSeedModel>>(jsonAdmin);

                foreach (var adminSeedModel in adminData)
                {
                    User admin = await UserManager.FindByEmailAsync(adminSeedModel.Email);
                    if(admin == null)
                    {
                        admin = new User()
                        {
                            UserName = adminSeedModel.UserName,
                            Email = adminSeedModel.Email,
                            NormalizedEmail = adminSeedModel.Email.Normalize()
                        };

                        var result = await UserManager.CreateAsync(admin, "Password!1");
                        if(result != IdentityResult.Success)
                        {
                            throw new InvalidOperationException("Could not create a new Admin in seeder: " + result.ToString());
                        }

                        //ensure Admin role exists
                        var adminRole = await RoleManager.FindByNameAsync("Admin");
                        if (adminRole == null)
                        {
                            throw new InvalidOperationException("Cant find Admin role in seeder");
                        }

                        //test getting new admin and add role
                        var newAdmin = await UserManager.FindByEmailAsync(admin.Email);
                        if (newAdmin != null)
                        {
                            await UserManager.AddToRoleAsync(newAdmin, adminRole.Name);
                        }
                        else
                        {
                            throw new InvalidOperationException("Cant find new Admin in seeder to add Role");
                        }

                        await Context.UserClaims.AddAsync(new IdentityUserClaim<int>()
                        {
                            UserId = newAdmin.Id,
                            ClaimType = "First Name",
                            ClaimValue = adminSeedModel.FirstName
                        });

                        await Context.UserClaims.AddAsync(new IdentityUserClaim<int>()
                        {
                            UserId = newAdmin.Id,
                            ClaimType = "Last Name",
                            ClaimValue = adminSeedModel.LastName
                        });
                    }
                }

                //Add Users
                var filePathUser = Path.Combine(RootPath, "API/Data/Seed/SeedData/SeedUsers.json");
                var jsonUser = File.ReadAllText(filePathUser);
                var userData = JsonSerializer.Deserialize<IEnumerable<KMUserSeedModel>>(jsonUser);

                foreach (var userSeedModel in userData)
                {
                    User user = await UserManager.FindByEmailAsync(userSeedModel.Email);
                    if (user == null)
                    {
                        user = new User()
                        {
                            UserName = userSeedModel.UserName,
                            Email = userSeedModel.Email,
                            NormalizedEmail = userSeedModel.Email.Normalize()
                        };

                        var result = await UserManager.CreateAsync(user, "Password!1");
                        if (result != IdentityResult.Success)
                        {
                            throw new InvalidOperationException("Could not create a new User in seeder");
                        }

                        //ensure User role exists
                        var userRole = await RoleManager.FindByNameAsync("User");
                        if (userRole == null)
                        {
                            throw new InvalidOperationException("Cant find User role in seeder");
                        }

                        //test getting new user and add role
                        var newUser = await UserManager.FindByEmailAsync(user.Email);
                        if (newUser != null)
                        {
                            await UserManager.AddToRoleAsync(newUser, userRole.Name);
                        }
                        else
                        {
                            throw new InvalidOperationException("Cant find new User in seeder to add Role");
                        }

                        await Context.UserClaims.AddAsync(new IdentityUserClaim<int>()
                        {
                            UserId = newUser.Id,
                            ClaimType = "First Name",
                            ClaimValue = userSeedModel.FirstName
                        });

                        await Context.UserClaims.AddAsync(new IdentityUserClaim<int>()
                        {
                            UserId = newUser.Id,
                            ClaimType = "Last Name",
                            ClaimValue = userSeedModel.LastName
                        });
                    }
                }

                await Context.SaveChangesAsync();
            }

            if (!(await Context.ItemTags.AnyAsync()))
            {
                //Add Item Tags
                await Context.ItemTags.AddAsync(new ItemTag() { Name = "Fruit", UserCreated = false });
                await Context.ItemTags.AddAsync(new ItemTag() { Name = "Vegitable", UserCreated = false });
                await Context.ItemTags.AddAsync(new ItemTag() { Name = "Poultry", UserCreated = false });
                await Context.ItemTags.AddAsync(new ItemTag() { Name = "Fish", UserCreated = false });
                await Context.ItemTags.AddAsync(new ItemTag() { Name = "Meat", UserCreated = false });
                await Context.ItemTags.AddAsync(new ItemTag() { Name = "Leftovers", UserCreated = false });
                await Context.ItemTags.AddAsync(new ItemTag() { Name = "Spice", UserCreated = false });

                await Context.SaveChangesAsync();
            }

            if(!(await Context.ItemTemplates.AnyAsync()))
            {
                //Add ItemTemplates
                var filePathItemTemplate = Path.Combine(RootPath, "API/Data/Seed/SeedData/SeedItemTemplates.json");
                var jsonItemTemplate = File.ReadAllText(filePathItemTemplate);
                var itemTemplateData = JsonSerializer.Deserialize<IEnumerable<ItemTemplateSeedModel>>(jsonItemTemplate);

                foreach (var itemTemplateSeedModel in itemTemplateData)
                {

                    var itemTemplate = new ItemTemplate()
                    {
                        Name = itemTemplateSeedModel.Name,
                        Brand = itemTemplateSeedModel.Brand,
                        Description = itemTemplateSeedModel.Description,
                        ExpirationDays = itemTemplateSeedModel.ExpirationDays
                    };

                    var takeNum = Random.Next(1, 4);
                    var itemTags = await Context.ItemTags.OrderBy(g => Guid.NewGuid()).Skip(Random.Next(Context.ItemTags.Count() - takeNum)).Take(takeNum).ToListAsync();

                    itemTemplate.ItemTags = itemTags;

                    await Context.ItemTemplates.AddAsync(itemTemplate);
                }

                await Context.SaveChangesAsync();
            }

            if(!(await Context.UserLists.AnyAsync()))
            {
                foreach (var user in Context.Users.ToList())
                {
                    var firstName = (await Context.UserClaims.Where(uc => uc.UserId == user.Id).FirstOrDefaultAsync()).ClaimValue;

                    var userList = new UserList()
                    {
                        Name = firstName + "'s first list",
                        Description = "A list of random fake ingredient items.",
                        User = user
                    };

                    await Context.UserLists.AddAsync(userList);

                    //test multiple lists for some test users
                    if( Random.Next() % 3 == 0 ){

                        userList = new UserList()
                        {
                            Name = firstName + "'s second list",
                            Description = "A list of random fake ingredient items.",
                            User = user
                        };

                        await Context.UserLists.AddAsync(userList);
                    }
                }

                await Context.SaveChangesAsync();
            }

            if (!(await Context.ListItems.AnyAsync())){
                //Add ListItems
                foreach (var userList in (await Context.UserLists.ToListAsync()))
                {
                    //add random number of items to each list
                    foreach (var i in Enumerable.Range(0, Random.Next(20)))
                    {
                        //get random itemTemplate
                        var itemTemplate = await Context.ItemTemplates.Skip(Random.Next(Context.ItemTemplates.Count() - 1)).FirstOrDefaultAsync();

                        var listItem = new ListItem()
                        {
                            Name = itemTemplate.Name,
                            Brand = itemTemplate.Brand,
                            Description = itemTemplate.Description,
                            Quantity = Random.Next(1, 5),
                            ExpirationDate = DateTime.UtcNow.AddDays(itemTemplate.ExpirationDays).Date,
                            UserList = userList
                        };

                        var itemTags = await Context.ItemTags.Where(it => it.Items.Contains(itemTemplate)).ToListAsync();

                        listItem.ItemTags = itemTags;

                        await Context.ListItems.AddAsync(listItem);
                    }
                }

                await Context.SaveChangesAsync();
            }

            //set item tag status to active for each one that was assigned to an item.
            foreach (ItemTag itemTag in (await Context.ItemTags.Where(it => it.Items.Any()).ToListAsync()))
            {
                itemTag.Status = Status.active;
            }

            await Context.SaveChangesAsync();
        }
    }
}
