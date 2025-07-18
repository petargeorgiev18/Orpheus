using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Orpheus.Data.Models.Configurations
{
    public class ItemImagesConfiguration : IEntityTypeConfiguration<ItemImage>
    {
        public void Configure(EntityTypeBuilder<ItemImage> builder)
        {
            builder.HasData(GetItems());
        }
        public IEnumerable<ItemImage> GetItems()
        {
            return new List<ItemImage>()
            {
                new ItemImage
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Url = "https://muzikercdn.com/uploads/products/19437/1943741/main_260b9357.jpg",
                    ItemId = Guid.Parse("44444444-4444-4444-4444-444444444444")
                },
                new ItemImage
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    Url = "https://muzikercdn.com/uploads/product_gallery/19437/1943742/main_9bea70a7.jpg",
                    ItemId = Guid.Parse("44444444-4444-4444-4444-444444444444")
                },
                new ItemImage
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    Url = "/images/albums/bornthisway.jpg",
                    ItemId = Guid.Parse("66666666-6666-6666-6666-666666666666")
                },
                new ItemImage
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    Url = "/images/albums/masterofpuppets.jpg",
                    ItemId = Guid.Parse("77777777-7777-7777-7777-777777777777")
                },
                new ItemImage
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    Url = "/images/albums/thedarksideofthemoon.jpg",
                    ItemId = Guid.Parse("88888888-8888-8888-8888-888888888888")
                }
            };

        }
    }
}
