using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orpheus.Data.Models;

namespace Orpheus.Data.Models.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            //builder.HasData(CreateCategories());
        }

        public IEnumerable<Category> CreateCategories()
        {
            return new List<Category>
            {
                new Category
                {
                    Id = Guid.Parse("BEB86BB8-F8F7-4844-B25C-2DE79D077F39"),
                    CategoryName = "Wooden"
                },
                new Category
                {
                    Id = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
                    CategoryName = "Guitars"
                },
                new Category
                {
                    Id = Guid.Parse("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB"),
                    CategoryName = "Drums"
                },
                new Category
                {
                    Id = Guid.Parse("CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
                    CategoryName = "Keyboards"
                },
                new Category
                {
                    Id = Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDDDDDD"),
                    CategoryName = "Albums"
                },
                new Category
                {
                    Id = Guid.Parse("EEEEEEEE-EEEE-EEEE-EEEE-EEEEEEEEEEEE"),
                    CategoryName = "Merch"
                },
                new Category
                {
                    Id = Guid.Parse("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"),
                    CategoryName = "Accessories"
                }
            };
        }
    }
}