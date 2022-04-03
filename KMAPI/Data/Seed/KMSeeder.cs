using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

using KitchenManager.KMAPI.Items.ItemTemplates;
using KitchenManager.KMAPI.Items.ListItems;
using KitchenManager.KMAPI.ItemTags;
using KitchenManager.KMAPI.KMUsers;
using KitchenManager.KMAPI.List;

namespace KitchenManager.KMAPI.Data.Seed
{
    public class KMSeeder
    {
        private readonly KMDbContext context;
        private readonly UserManager<KMUser> userManager;
        private readonly RoleManager<IdentityRole<int>> roleManager;
        public string RootPath { get; set; }
        public Random rand;

        public KMSeeder(KMDbContext ctx, UserManager<KMUser> um, RoleManager<IdentityRole<int>> rm)
        {
            context = ctx;
            userManager = um;
            roleManager = rm;
            RootPath = string.Empty;
            rand = new Random();
    }

        public async Task SeedDataAsync(bool clearData)
        {
            //seed data
            
            if (clearData)
            {
                await context.Database.EnsureDeletedAsync();
                Console.Write("Database deleted.");
            }
            
            await context.Database.EnsureCreatedAsync();
            Console.Write("Database Exists or Created.");

            if (!(await context.Roles.AnyAsync()))
            {
                await roleManager.CreateAsync(new IdentityRole<int>() { Name = "Admin" });
                await roleManager.CreateAsync(new IdentityRole<int>() { Name = "User" });

                await context.SaveChangesAsync();
            }
            
            if (!context.Users.Any())
            {
                //Add Admins
                var filePathAdmin = Path.Combine(RootPath, "KMAPI/Data/Seed/SeedData/SeedAdmins.json");
                var jsonAdmin = File.ReadAllText(filePathAdmin);
                var adminData = JsonSerializer.Deserialize<IEnumerable<KMUserSeedModel>>(jsonAdmin);

                foreach (var adminSeedModel in adminData)
                {
                    KMUser admin = await userManager.FindByEmailAsync(adminSeedModel.Email);
                    if(admin == null)
                    {
                        admin = new KMUser()
                        {
                            UserName = adminSeedModel.UserName,
                            Email = adminSeedModel.Email,
                            NormalizedEmail = adminSeedModel.Email.Normalize()
                        };

                        var result = await userManager.CreateAsync(admin, "Password!1");
                        if(result != IdentityResult.Success)
                        {
                            throw new InvalidOperationException("Could not create a new Admin in seeder: " + result.ToString());
                        }

                        //ensure Admin role exists
                        var adminRole = await roleManager.FindByNameAsync("Admin");
                        if (adminRole == null)
                        {
                            throw new InvalidOperationException("Cant find Admin role in seeder");
                        }

                        //test getting new admin and add role
                        var newAdmin = await userManager.FindByEmailAsync(admin.Email);
                        if (newAdmin != null)
                        {
                            await userManager.AddToRoleAsync(newAdmin, adminRole.Name);
                        }
                        else
                        {
                            throw new InvalidOperationException("Cant find new Admin in seeder to add Role");
                        }

                        await context.UserClaims.AddAsync(new IdentityUserClaim<int>()
                        {
                            UserId = newAdmin.Id,
                            ClaimType = "First Name",
                            ClaimValue = adminSeedModel.FirstName
                        });

                        await context.UserClaims.AddAsync(new IdentityUserClaim<int>()
                        {
                            UserId = newAdmin.Id,
                            ClaimType = "Last Name",
                            ClaimValue = adminSeedModel.LastName
                        });
                    }
                }

                //Add Users
                var filePathUser = Path.Combine(RootPath, "KMAPI/Data/Seed/SeedData/SeedUsers.json");
                var jsonUser = File.ReadAllText(filePathUser);
                var userData = JsonSerializer.Deserialize<IEnumerable<KMUserSeedModel>>(jsonUser);

                foreach (var userSeedModel in userData)
                {
                    KMUser user = await userManager.FindByEmailAsync(userSeedModel.Email);
                    if (user == null)
                    {
                        user = new KMUser()
                        {
                            UserName = userSeedModel.UserName,
                            Email = userSeedModel.Email,
                            NormalizedEmail = userSeedModel.Email.Normalize()
                        };

                        var result = await userManager.CreateAsync(user, "Password!1");
                        if (result != IdentityResult.Success)
                        {
                            throw new InvalidOperationException("Could not create a new User in seeder");
                        }

                        //ensure User role exists
                        var userRole = await roleManager.FindByNameAsync("User");
                        if (userRole == null)
                        {
                            throw new InvalidOperationException("Cant find User role in seeder");
                        }

                        //test getting new user and add role
                        var newUser = await userManager.FindByEmailAsync(user.Email);
                        if (newUser != null)
                        {
                            await userManager.AddToRoleAsync(newUser, userRole.Name);
                        }
                        else
                        {
                            throw new InvalidOperationException("Cant find new User in seeder to add Role");
                        }

                        await context.UserClaims.AddAsync(new IdentityUserClaim<int>()
                        {
                            UserId = newUser.Id,
                            ClaimType = "First Name",
                            ClaimValue = userSeedModel.FirstName
                        });

                        await context.UserClaims.AddAsync(new IdentityUserClaim<int>()
                        {
                            UserId = newUser.Id,
                            ClaimType = "Last Name",
                            ClaimValue = userSeedModel.LastName
                        });
                    }
                }

                await context.SaveChangesAsync();
            }

            if (!(await context.ItemTags.AnyAsync()))
            {
                //Add Item Tags
                await context.ItemTags.AddAsync(new ItemTag() { Name = "Fruit" });
                await context.ItemTags.AddAsync(new ItemTag() { Name = "Vegitable" });
                await context.ItemTags.AddAsync(new ItemTag() { Name = "Poultry" });
                await context.ItemTags.AddAsync(new ItemTag() { Name = "Fish" });
                await context.ItemTags.AddAsync(new ItemTag() { Name = "Meat" });
                await context.ItemTags.AddAsync(new ItemTag() { Name = "Leftovers" });
                await context.ItemTags.AddAsync(new ItemTag() { Name = "Spice" });

                await context.SaveChangesAsync();
            }

            if(!(await context.ItemTemplates.AnyAsync()))
            {
                //Add ItemTemplates
                var filePathItemTemplate = Path.Combine(RootPath, "KMAPI/Data/Seed/SeedData/SeedItemTemplates.json");
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

                    var takeNum = rand.Next(1, 4);
                    var itemTags = await context.ItemTags.OrderBy(g => Guid.NewGuid()).Skip(rand.Next(context.ItemTags.Count() - takeNum)).Take(takeNum).ToListAsync();

                    itemTemplate.ItemTags = itemTags;

                    await context.ItemTemplates.AddAsync(itemTemplate);
                }

                await context.SaveChangesAsync();
            }

            if(!(await context.UserLists.AnyAsync()))
            {
                foreach (var user in context.Users.ToList())
                {
                    var firstName = (await context.UserClaims.Where(uc => uc.UserId == user.Id).FirstOrDefaultAsync()).ClaimValue;

                    var userList = new UserList()
                    {
                        KMUserId = user.Id,
                        Name = firstName + "'s first list",
                        Description = "A list of random fake ingredient items."
                        
                    };

                    await context.UserLists.AddAsync(userList);

                    //test multiple lists for some test users
                    if( rand.Next() % 3 == 0 ){

                        userList = new UserList()
                        {
                            KMUserId = user.Id,
                            Name = firstName + "'s second list",
                            Description = "A list of random fake ingredient items."

                        };

                        await context.UserLists.AddAsync(userList);
                    }
                }

                await context.SaveChangesAsync();
            }

            if (!(await context.ListItems.AnyAsync())){
                //Add ListItems
                foreach (var userList in (await context.UserLists.ToListAsync()))
                {
                    //add random number of items to each list
                    foreach (var i in Enumerable.Range(0, rand.Next(20)))
                    {
                        //get random itemTemplate
                        var itemTemplate = await context.ItemTemplates.Skip(rand.Next(context.ItemTemplates.Count() - 1)).FirstOrDefaultAsync();

                        var listItem = new ListItem()
                        {
                            UserListId = userList.Id,
                            Name = itemTemplate.Name,
                            Brand = itemTemplate.Brand,
                            Description = itemTemplate.Description,
                            Quantity = rand.Next(1, 5),
                            ExpirationDate = DateTime.UtcNow.AddDays(itemTemplate.ExpirationDays).Date,
                        };

                        var itemTags = await context.ItemTags.Where(it => it.Items.Contains(itemTemplate)).ToListAsync();

                        listItem.ItemTags = itemTags;

                        await context.ListItems.AddAsync(listItem);
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
