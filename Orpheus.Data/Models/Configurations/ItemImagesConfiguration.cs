﻿using System;
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
                    ItemId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    IsMain = true
                },
                new ItemImage
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    Url = "https://muzikercdn.com/uploads/product_gallery/19437/1943742/main_9bea70a7.jpg",
                    ItemId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    IsMain = false
                },
                new ItemImage
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    Url = "/images/albums/bornthisway.jpg",
                    ItemId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    IsMain = true
                },
                new ItemImage
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    Url = "/images/albums/masterofpuppets.jpg",
                    ItemId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    IsMain = true
                },
                new ItemImage
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    Url = "/images/albums/thedarksideofthemoon.jpg",
                    ItemId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    IsMain = true
                },
                new ItemImage
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Url = "https://m.media-amazon.com/images/I/61KduVSVYlL._SX425_.jpg",
                    ItemId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    IsMain = false
                },
                new ItemImage
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Url = "https://m.media-amazon.com/images/I/61lELUFnnkL._UF1000,1000_QL80_.jpg",
                    ItemId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    IsMain = true
                },
                new ItemImage
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Url = "https://m.media-amazon.com/images/I/61crwsvMPcL._AC_SL1200_.jpg",
                    ItemId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-bbbbbbbbbbbb"),
                    IsMain = true
                },
                new ItemImage
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Url = "https://m.media-amazon.com/images/I/61Rx712UAbL._UF894,1000_QL80_.jpg",
                    ItemId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    IsMain = true
                },
                new ItemImage
                {
                    Id = Guid.Parse("ccccccc1-cccc-cccc-cccc-cccccccccccc"),
                    Url = "https://www.emp.ie/dw/image/v2/BBQV_PRD/on/demandware.static/-/Sites-master-emp/default/dwa9ad3e79/images/1/0/5/8/105863a.jpg?sfrm=png",
                    ItemId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0001"), // ✅ fixed
                    IsMain = true
                },
                new ItemImage
                {
                    Id = Guid.Parse("ccccccc2-cccc-cccc-cccc-cccccccccccc"),
                    Url = "https://www.emp.ie/dw/image/v2/BBQV_PRD/on/demandware.static/-/Sites-master-emp/default/dwe630cd96/images/1/0/5/8/105863b.jpg?sfrm=png",
                    ItemId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0001"),
                    IsMain = false
                },
                new ItemImage
                {
                    Id = Guid.Parse("ccccccc3-cccc-cccc-cccc-cccccccccccc"),
                    Url = "https://muzikercdn.com/uploads/products/3972/397246/thumb_large_d_gallery_base_dc6883f7.jpg",
                    ItemId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0002"), // ✅ fixed
                    IsMain = true
                },
                new ItemImage
                {
                    Id = Guid.Parse("ccccccc4-cccc-cccc-cccc-cccccccccccc"),
                    Url = "https://muzikercdn.com/uploads/product_gallery/3972/397247/main_692dc8f6.jpg",
                    ItemId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0002"),
                    IsMain = false
                },
                new ItemImage
                {
                    Id = Guid.Parse("ccccccc5-cccc-cccc-cccc-cccccccccccc"),
                    Url = "https://i.ebayimg.com/00/s/MTUzN1gxNjAw/z/RFMAAOSwntxiBu1x/$_57.JPG?set_id=8800005007",
                    ItemId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0003"), // ✅ fixed
                    IsMain = true
                }

            };

        }
    }
}
