using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Orpheus.Data.Models.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            //builder.HasData(CreateItems());
        }
        public IEnumerable<Item> CreateItems()
        {
            return new List<Item>
            {
                new Item
                {
                    Id = Guid.NewGuid(),
                    Brand = new Brand
                    {
                        Id = Guid.NewGuid(),
                        Name = "ESP LTD",
                        Description = "A leading brand in musical instruments."
                    },
                    Name = "ESP LTD EC-256",
                    Description = "This is a sample item description.",
                    Price = 19.99m,
                    CategoryId = Guid.NewGuid(),
                    BrandId = Guid.NewGuid(),
                    IsAvailable = true
                }
            };
        }
    }
}
