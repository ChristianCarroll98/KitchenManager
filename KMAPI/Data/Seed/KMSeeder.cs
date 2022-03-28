using KitchenManager.Data;
using KitchenManager.KMAPI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace KitchenManager.Seed
{
    public class KMSeeder
    {
        private readonly KMDbContext context;
        private readonly UserManager<KMUser> userManager;
        private readonly RoleManager<KMRole> roleManager;
        public string RootPath { get; set; }
        public Random rand;

        public KMSeeder(KMDbContext ctx, UserManager<KMUser> um, RoleManager<KMRole> rm)
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
                context.Database.EnsureDeleted();
            }
            
            context.Database.EnsureCreated();

            if (!(context.Roles.Any()))
            {
                await roleManager.CreateAsync(new KMRole("Admin"));
                await roleManager.CreateAsync(new KMRole("User"));
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
                            FirstName = adminSeedModel.FirstName,
                            LastName = adminSeedModel.LastName,
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
                            FirstName = userSeedModel.FirstName,
                            LastName = userSeedModel.LastName,
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
                    }
                }
            }

            if (!(context.ItemTypes.Any()))
            {
                //Add Item Types
                context.ItemTypes.Add(new ItemType() { Name = "Fruit" });
                context.ItemTypes.Add(new ItemType() { Name = "Vegitable" });
                context.ItemTypes.Add(new ItemType() { Name = "Poultry" });
                context.ItemTypes.Add(new ItemType() { Name = "Fish" });
                context.ItemTypes.Add(new ItemType() { Name = "Meat" });
                context.ItemTypes.Add(new ItemType() { Name = "Leftovers", Description = "Saving it for later!" });
                context.ItemTypes.Add(new ItemType() { Name = "Other", Description = "Anything that doesnt quite fit into other categories." });
            }

            if(!(context.ItemTemplates.Any()))
            {
                //Add ItemTemplates
                var filePathItemTemplate = Path.Combine(RootPath, "KMAPI/Data/Seed/SeedData/SeedItemTemplates.json");
                var jsonItemTemplate = File.ReadAllText(filePathItemTemplate);
                var itemTemplateData = JsonSerializer.Deserialize<IEnumerable<ItemTemplateSeedModel>>(jsonItemTemplate);

                foreach (var itemTemplateSeedModel in itemTemplateData)
                {
                    
                    var itemTemplate = new ItemTemplate()
                    {
                        TypeId = itemTemplateSeedModel.TypeId,
                        Name = itemTemplateSeedModel.Name,
                        Description = itemTemplateSeedModel.Description,
                        ExpirationDays = itemTemplateSeedModel.ExpirationDays
                    };

                    context.ItemTemplates.Add(itemTemplate);
                }
            }

            if(!(context.UserLists.Any()))
            {
                foreach (var user in context.Users)
                {
                    var userList = new UserList()
                    {
                        UserId = user.Id,
                        Name = user.FirstName + "'s first list",
                        Description = "A list of random fake ingredient items."
                        
                    };

                    context.UserLists.Add(userList);

                    //test multiple lists for some test users
                    if( rand.Next() % 3 == 0 ){

                        userList = new UserList()
                        {
                            UserId = user.Id,
                            Name = user.FirstName + "'s second list",
                            Description = "A list of random fake ingredient items."

                        };

                        context.UserLists.Add(userList);
                    }
                }
            }

            if (!(context.ListItems.Any())){
                //Add ListItems
                foreach (var userList in context.UserLists.ToList())
                {
                    //add random number of items to each list
                    foreach (var i in Enumerable.Range(0, rand.Next(20)))
                    {
                        //get random itemTemplate
                        var itemTemplate = context.ItemTemplates.OrderBy(r => r.Id).Skip(rand.Next(context.ItemTemplates.Count() - 1)).FirstOrDefault();

                        var listItem = new ListItem()
                        {
                            ListId = userList.Id,
                            TypeId = itemTemplate.TypeId,
                            Name = itemTemplate.Name,
                            Description = itemTemplate.Description,
                            ExpirationDate = DateTime.UtcNow.AddDays(itemTemplate.ExpirationDays).Date
                        };

                        context.ListItems.Add(listItem);
                    }
                }
            }
                context.SaveChanges();
        }
    }
}
