using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using KitchenManager.API.UsersNS;
using KitchenManager.API.UserListsNS;
using KitchenManager.API.ItemsNS;
using KitchenManager.API.ItemsNS.ListItemsNS;
using KitchenManager.API.ItemsNS.ItemTemplatesNS;
using KitchenManager.API.ItemTagsNS;
using KitchenManager.API.IconsNS;

namespace KitchenManager.API.Data
{
    public class KMDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<UserListModel> UserLists { get; set; }
        public DbSet<ListItemModel> ListItems { get; set; }
        public DbSet<ItemTagModel> ItemTags { get; set; }
        public DbSet<ItemTemplateModel> ItemTemplates { get; set; }
        public DbSet<IconModel> Icons { get; set; }

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
            builder.Entity<User>(b =>
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

            builder.Entity<UserListModel>(b =>
            {
                b.HasKey(ul => ul.Id);

                b.ToTable("UserLists");
            });

            builder.Entity<ItemModel>(b =>
            {
                b.HasKey(i => i.Id);

                b.HasDiscriminator(i => i.Discriminator);

                b.ToTable("Items");
            });

            builder.Entity<ListItemModel>(b =>
            {
                b.HasBaseType<ItemModel>();
            });

            builder.Entity<ItemTemplateModel>(b =>
            {
                b.HasBaseType<ItemModel>();
            });

            builder.Entity<ItemTagModel>(b =>
            {
                b.HasKey(it => it.Id);

                b.ToTable("ItemTags");
            });

            builder.Entity<IconModel>(b =>
            {
                b.HasKey(i => i.Id);
            });
        }
    }
}
