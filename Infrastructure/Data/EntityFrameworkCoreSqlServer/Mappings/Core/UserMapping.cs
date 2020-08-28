using Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityFrameworkCoreSqlServer.Mappings.Core
{
    class UserMapping
    {
        public UserMapping(EntityTypeBuilder<User> entityTypeBuilder)
        {
            // Override AspNetUsers mapping in IdentityDbContext
            entityTypeBuilder.ToTable("Users");
            entityTypeBuilder.Property(e => e.UserName).IsRequired();
            entityTypeBuilder.Property(e => e.NormalizedUserName).IsRequired();
            entityTypeBuilder.Property(e => e.Email).IsRequired();
            entityTypeBuilder.Property(e => e.NormalizedEmail).IsRequired();
            entityTypeBuilder.Property(e => e.ChangePassword).HasDefaultValue(false);
            entityTypeBuilder.Property(e => e.IsSuspended).HasDefaultValue(false);
            entityTypeBuilder.Property(e => e.CreatedAt).IsRequired();
            entityTypeBuilder.Property(e => e.UpdatedAt).IsRequired();

            entityTypeBuilder.Ignore(e => e.ReadOnlyRoleUsers);

            entityTypeBuilder.HasIndex(e => e.NormalizedEmail).IsUnique();

            entityTypeBuilder.OwnsOne(e => e.Profile, profileTypeBuilder =>
            {
                // profileTypeBuilder.Property(p => p.Gender).HasConversion<string>() could have been used to auto-convert a non-nullable Gender enum to string when saving and back to Gender enum when retriveing
                // but the converters don't convert null and for most user registration processes, gender is usually not required.
                profileTypeBuilder.Ignore(p => p.Gender);
                profileTypeBuilder.Property<string>("gender").HasColumnName("Gender");

                // Force storage of owned entity in a different table
                profileTypeBuilder.ToTable("Profiles");
            });

            entityTypeBuilder.HasMany(typeof(RoleUser), "RoleUsers")
               .WithOne("User")
               .HasForeignKey("UserId")
               .IsRequired();
        }
    }
}
