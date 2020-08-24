﻿using Domain.Core;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityFrameworkCoreSqlServer.Mappings.Core
{
    class PermissionMapping
    {
        public PermissionMapping(EntityTypeBuilder<Permission> entityTypeBuilder)
        {
            entityTypeBuilder.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entityTypeBuilder.Property(e => e.NormalizedName).IsRequired().HasMaxLength(255);
            entityTypeBuilder.Property(e => e.CreatedAt).IsRequired();
            entityTypeBuilder.Property(e => e.UpdatedAt).IsRequired();

            entityTypeBuilder.HasIndex(e => e.NormalizedName).IsUnique();
        }
    }
}
