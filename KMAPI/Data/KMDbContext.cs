using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using KitchenManager.KMAPI;

namespace KitchenManager.Data
{
    public class KMDbContext : IdentityDbContext<KMUser, KMRole, int>
    {
        public DbSet<UserList> UserLists { get; set; }
        public DbSet<ListItem> ListItems { get; set; }
        public DbSet<ItemTag> ItemTags { get; set; }
        public DbSet<ItemTemplate> ItemTemplates { get; set; }

        public KMDbContext(DbContextOptions<KMDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<KMRoleClaim>(b =>
            {
                b.Property(roleClaim =>roleClaim.ClaimType)
                    .IsUnicode(false)
                    .IsFixedLength(false)
                    .HasMaxLength(256);

                b.Property(roleClaim => roleClaim.ClaimValue)
                    .IsUnicode(false)
                    .IsFixedLength(false)
                    .HasMaxLength(256);
            });

            builder.Entity<KMUserClaim>(b =>
            {
                b.Property(userClaim => userClaim.ClaimType)
                    .IsUnicode(false)
                    .IsFixedLength(false)
                    .HasMaxLength(256);

                b.Property(userClaim => userClaim.ClaimValue)
                    .IsUnicode(false)
                    .IsFixedLength(false)
                    .HasMaxLength(256);
            });

            builder.Entity<KMUserLogin>(b =>
            {
                b.Property(userLogin => userLogin.ProviderDisplayName)
                    .IsFixedLength(false)
                    .HasMaxLength(256);

            });

            builder.Entity<KMUserToken>(b =>
            {
                b.Property(userToken => userToken.Value)
                    .IsUnicode(false)
                    .IsFixedLength(false)
                    .HasMaxLength(1000);

            });

            builder.Entity<KMUser>(b =>
            {
                b.Property(user => user.FirstName)
                    .HasMaxLength(256);

                b.Property(user => user.LastName)
                    .HasMaxLength(256);

                b.Property(user => user.PhoneNumber)
                    .IsUnicode(false)
                    .IsFixedLength(false)
                    .HasMaxLength(15);

                b.Property(user => user.PasswordHash)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasMaxLength(84);

                b.Property(user => user.ConcurrencyStamp)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasMaxLength(36)
                    .IsRequired(true);

                b.Property(user => user.SecurityStamp)
                    .IsUnicode(false)
                    .IsFixedLength(false)
                    .HasMaxLength(36)
                    .IsRequired(true);

                b.Property(user => user.Email)
                    .IsRequired(true);
                
                b.Property(user => user.NormalizedEmail)
                   .IsRequired(true);
            });

            builder.Entity<KMRole>(b =>
            {
                b.Property(r => r.ConcurrencyStamp)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasMaxLength(36)
                    .IsRequired(true);
            });

            builder.Entity<UserList>(b =>
            {
                b.HasKey(ul => ul.Id);

                b.Property(ul => ul.UserId)
                    .IsRequired();

                b.HasOne<KMUser>()
                    .WithMany()
                    .HasForeignKey(ul => ul.UserId);

                b.ToTable("UserLists");
            });

            builder.Entity<ListItem>(b =>
            {
                b.HasKey(li => li.Id);

                b.Property(li => li.ListId)
                    .IsRequired();

                b.HasOne<UserList>()
                    .WithMany()
                    .HasForeignKey(li => li.ListId);

                b.ToTable("ListItems");
            });

            /*builder.Entity<ItemTagItemTemplateJoinModel>(b =>
            {
                b.HasKey(jm => new { jm.ItemTagId, jm.ItemTemplateId });

                b.HasOne<ItemTag>()
                    .WithMany()
                    .HasForeignKey(jm => jm.ItemTagId);

                b.HasOne<ItemTemplate>()
                    .WithMany()
                    .HasForeignKey(jm => jm.ItemTemplateId);

                b.ToTable("ItemTagItemTemplateJoinModels");
            });

            builder.Entity<ItemTagListItemJoinModel>(b =>
            {
                b.HasKey(jm => new { jm.ItemTagId, jm.ListItemId });

                b.HasOne<ItemTag>()
                    .WithMany()
                    .HasForeignKey(jm => jm.ItemTagId);

                b.HasOne<ListItem>()
                    .WithMany()
                    .HasForeignKey(jm => jm.ListItemId);
                    

                b.ToTable("ItemTagListItemJoinModels");
            });*/

            builder.Entity<ItemTag>(b =>
            {
                b.HasKey(it => it.Id);

                b.ToTable("ItemTags");
            });

            builder.Entity<ItemTemplate>(b =>
            {
                b.HasKey(it => it.Id);

                b.ToTable("ItemTemplates");
            });
        }
    }
}
