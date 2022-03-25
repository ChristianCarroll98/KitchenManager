using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using KitchenManager.KMAPI.KMRole;
using KitchenManager.KMAPI.KMUser;
using Microsoft.EntityFrameworkCore;

using KitchenManager.KMAPI.Data;

namespace KitchenManager.Data
{
    public class KMDbContext : IdentityDbContext<KMUser, KMRole, int>
    {
        public KMDbContext(DbContextOptions<KMDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<KMRoleClaim>(u =>
            {
                u.Property(roleClaim =>roleClaim.ClaimType)
                    .IsUnicode(false)
                    .IsFixedLength(false)
                    .HasMaxLength(256);

                u.Property(roleClaim => roleClaim.ClaimValue)
                    .IsUnicode(false)
                    .IsFixedLength(false)
                    .HasMaxLength(256);
            });

            builder.Entity<KMUserClaim>(u =>
            {
                u.Property(userClaim => userClaim.ClaimType)
                    .IsUnicode(false)
                    .IsFixedLength(false)
                    .HasMaxLength(256);

                u.Property(userClaim => userClaim.ClaimValue)
                    .IsUnicode(false)
                    .IsFixedLength(false)
                    .HasMaxLength(256);
            });

            builder.Entity<KMUserLogin>(u =>
            {
                u.Property(userLogin => userLogin.ProviderDisplayName)
                    .IsFixedLength(false)
                    .HasMaxLength(256);

            });

            builder.Entity<KMUserToken>(u =>
            {
                u.Property(userToken => userToken.Value)
                    .IsUnicode(false)
                    .IsFixedLength(false)
                    .HasMaxLength(1000);

            });

            builder.Entity<KMUser>(u =>
            {
                u.Property(user => user.FirstName)
                    .HasMaxLength(256);

                u.Property(user => user.LastName)
                    .HasMaxLength(256);

                u.Property(user => user.PhoneNumber)
                    .IsUnicode(false)
                    .IsFixedLength(false)
                    .HasMaxLength(15);

                u.Property(user => user.PasswordHash)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasMaxLength(84);

                u.Property(user => user.ConcurrencyStamp)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasMaxLength(36)
                    .IsRequired(true);

                u.Property(user => user.SecurityStamp)
                    .IsUnicode(false)
                    .IsFixedLength(false)
                    .HasMaxLength(36)
                    .IsRequired(true);
            });

            builder.Entity<KMRole>(u =>
            {
                u.Property(user => user.ConcurrencyStamp)
                    .IsUnicode(false)
                    .IsFixedLength(true)
                    .HasMaxLength(36)
                    .IsRequired(true);
            });
        }
    }
}
