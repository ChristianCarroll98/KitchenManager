﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using KitchenManager.KMAPI.KMRole;
using KitchenManager.KMAPI.KMUser;
using Microsoft.EntityFrameworkCore;

using KitchenManager.KMAPI.Data;
using KitchenManager.KMAPI.UserLists;
using KitchenManager.KMAPI.ListItems;
using KitchenManager.KMAPI.ItemTypes;
using KitchenManager.KMAPI.ItemTemplates;

namespace KitchenManager.Data
{
    public class KMDbContext : IdentityDbContext<KMUser, KMRole, int>
    {
        public DbSet<UserList> UserLists { get; set; }
        public DbSet<ListItem> ListItems { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
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
                    .IsUnicode(false)
                    .IsRequired(true);
                
                b.Property(user => user.NormalizedEmail)
                   .IsUnicode(false)
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

                b.Property(li => li.TypeId)
                    .IsRequired();

                b.HasOne<ItemType>()
                    .WithMany()
                    .HasForeignKey(li => li.TypeId);

                b.ToTable("ListItems");
            });

            builder.Entity<ItemType>(b =>
            {
                b.HasKey(it => it.Id);

                b.ToTable("ItemTypes");
            });

            builder.Entity<ItemTemplate>(b =>
            {
                b.HasKey(it => it.Id);

                b.Property(it => it.TypeId)
                    .IsRequired();

                b.HasOne<ItemType>()
                    .WithMany()
                    .HasForeignKey(it => it.TypeId);

                b.ToTable("ItemTemplates");
            });
        }
    }
}
