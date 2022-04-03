using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using KitchenManager.KMAPI.KMUsers;
using KitchenManager.KMAPI.List;
using KitchenManager.KMAPI.Items;
using KitchenManager.KMAPI.Items.ListItems;
using KitchenManager.KMAPI.Items.ItemTemplates;
using KitchenManager.KMAPI.ItemTags;
using KitchenManager.KMAPI.Icons;

namespace KitchenManager.KMAPI.Data
{
    public class KMDbContext : IdentityDbContext<KMUser, IdentityRole<int>, int>
    {
        public DbSet<UserList> UserLists { get; set; }
        public DbSet<ListItem> ListItems { get; set; }
        public DbSet<ItemTag> ItemTags { get; set; }
        public DbSet<ItemTemplate> ItemTemplates { get; set; }
        public DbSet<Icon> Icons { get; set; }

        public KMDbContext(DbContextOptions<KMDbContext> options) : base(options)
        {
            //options here
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Settings for Identity classes (int for Id column and table names)
            builder.Entity<IdentityUserRole<int>>(b =>
            {
                b.ToTable("UserRoles");
            });

            builder.Entity<IdentityRole<int>>(b => 
            {
                b.Property(role => role.ConcurrencyStamp)
                   .IsUnicode(false)
                   .IsFixedLength(true)
                   .HasMaxLength(36)
                   .IsRequired(true);

                b.ToTable("Roles");
            });

            builder.Entity<IdentityRoleClaim<int>>(b => 
            {
                b.Property(roleClaim => roleClaim.ClaimType)
                    .HasMaxLength(256);

                b.Property(roleClaim => roleClaim.ClaimValue)
                    .HasMaxLength(256);

                b.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserClaim<int>>(b =>
            {
                b.Property(userClaim => userClaim.ClaimType)
                    .HasMaxLength(256);

                b.Property(userClaim => userClaim.ClaimValue)
                    .HasMaxLength(256);

                b.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<int>>(b =>
            {
                b.Property(userLogin => userLogin.ProviderDisplayName)
                        .HasMaxLength(256);

                b.ToTable("UserLogins");
            });

            builder.Entity<IdentityUserToken<int>>(b =>
            {
                b.Property(userToken => userToken.Value)
                    .IsUnicode(false)
                    .HasMaxLength(1000);

                b.ToTable("UserTokens");
            });

            //My classes
            builder.Entity<KMUser>(b =>
            {
                b.Property(user => user.PhoneNumber)
                    .IsUnicode(false)
                    .HasMaxLength(15);

                b.Property(user => user.PasswordHash)
                    .IsUnicode(false)
                    .HasMaxLength(256);

                b.Property(user => user.ConcurrencyStamp)
                    .IsUnicode(false)
                    .HasMaxLength(256)
                    .IsRequired(true);

                b.Property(user => user.SecurityStamp)
                    .IsUnicode(false)
                    .HasMaxLength(256)
                    .IsRequired(true);

                b.Property(user => user.Email)
                    .IsRequired(true);
                
                b.Property(user => user.NormalizedEmail)
                   .IsRequired(true);

                b.ToTable("Users");
            });

            builder.Entity<UserList>(b =>
            {
                b.HasKey(ul => ul.Id);

                b.Property(ul => ul.KMUserId)
                    .IsRequired();

                b.ToTable("UserLists");
            });

            builder.Entity<Item>(b =>
            {
                b.HasKey(i => i.Id);

                b.HasDiscriminator(i => i.Discriminator);

                b.ToTable("Items");
            });

            builder.Entity<ListItem>(b =>
            {
                b.Property(li => li.UserListId)
                    .IsRequired();

                b.HasBaseType<Item>();
            });

            builder.Entity<ItemTemplate>(b =>
            {
                b.HasBaseType<Item>();
            });

            builder.Entity<ItemTag>(b =>
            {
                b.HasKey(it => it.Id);

                b.ToTable("ItemTags");
            });

            builder.Entity<Icon>(b =>
            {
                b.HasKey(i => i.Id);
            });
        }
    }
}
