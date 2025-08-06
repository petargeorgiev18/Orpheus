using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orpheus.Data.Models;

namespace Orpheus.Data.Models.Configurations
{
    public class ItemImagesConfiguration : IEntityTypeConfiguration<ItemImage>
    {
        public void Configure(EntityTypeBuilder<ItemImage> builder)
        {
            //builder.HasData(GetItems());
        }

        public IEnumerable<ItemImage> GetItems()
        {
            return new List<ItemImage>
            {
                new ItemImage
                {
                    Id = Guid.Parse("6129E2C3-E051-4B60-990D-016CA1AD0BD2"),
                    Url = "https://muzikercdn.com/uploads/products/3732/373292/thumb_large_d_gallery_base_af02fa57.jpg",
                    ItemId = Guid.Parse("3B2EEDB0-E426-45AF-8F49-3169299A1F39"),
                    IsMain = true
                },
                new ItemImage
                {
                    Id = Guid.Parse("2E4952CF-05C2-42A8-B9DE-0F09CC321B37"),
                    Url = "https://muzikercdn.com/uploads/products/19123/1912392/main_cf0f3076.jpg",
                    ItemId = Guid.Parse("3E3C22C3-FD05-4D2C-932A-1841CA70A8C8"),
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
                    Id = Guid.Parse("B74D660B-CF1F-44D7-8F93-1417A335071F"),
                    Url = "https://muzikercdn.com/uploads/product_gallery/3876/387698/main_d2251751.jpg",
                    ItemId = Guid.Parse("1F261899-F152-4B34-BD1B-51D21B077F57"),
                    IsMain = false
                },
                new ItemImage
                {
                    Id = Guid.Parse("A42898D0-4799-4D3E-B78F-1D398F2C488B"),
                    Url = "https://muzikercdn.com/uploads/products/2286/228604/thumb_large_d_gallery_base_96a1dc9a.png",
                    ItemId = Guid.Parse("68F29CE0-6435-4CEE-BC5A-521C166F71E3"),
                    IsMain = true
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
                    Id = Guid.Parse("84EA9BDE-785F-4D47-8874-2381123343FA"),
                    Url = "https://muzikercdn.com/uploads/product_gallery/19432/1943249/main_a423b730.jpg",
                    ItemId = Guid.Parse("C4690AE8-BBDF-4AA0-8140-FBFF27687865"),
                    IsMain = false
                },
                new ItemImage
                {
                    Id = Guid.Parse("2B8F09DB-AD4C-4D15-B555-2A0124E4E0A9"),
                    Url = "https://muzikercdn.com/uploads/product_gallery/19922/1992243/thumb_large_d_gallery_base_7094b0d5.jpg",
                    ItemId = Guid.Parse("3B2EEDB0-E426-45AF-8F49-3169299A1F39"),
                    IsMain = false
                },
                new ItemImage
                {
                    Id = Guid.Parse("BB8EEF12-C02F-4C70-A779-37ACD3AA6EF4"),
                    Url = "https://muzikercdn.com/uploads/products/610/61003/main_1d3ca44c.jpg",
                    ItemId = Guid.Parse("D67363D7-622E-4659-8D64-7FC0F7B02FCC"),
                    IsMain = true
                },
                new ItemImage
                {
                    Id = Guid.Parse("F173CDFE-A0B2-48B9-997A-4285B15F12B6"),
                    Url = "https://muzikercdn.com/uploads/products/20698/2069856/thumb_large_d_gallery_base_3d1f0fb7.jpg",
                    ItemId = Guid.Parse("6B2343B9-BE7B-47DF-B0B5-FB8BD41798CF"),
                    IsMain = true
                },
                new ItemImage
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Url = "https://m.media-amazon.com/images/I/61Rx712UAbL._UF894,1000_QL80_.jpg",
                    ItemId = Guid.Parse("CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
                    IsMain = true
                },
                new ItemImage
                {
                    Id = Guid.Parse("4446400B-6B89-4356-A93E-4A851EC198AE"),
                    Url = "https://muzikercdn.com/uploads/products/12334/1233487/thumb_large_d_gallery_base_9b60b202.jpg",
                    ItemId = Guid.Parse("9953F144-647E-45A0-AEE8-A394674B5B06"),
                    IsMain = true
                },
                new ItemImage
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    Url = "https://muzikercdn.com/uploads/products/20832/2083281/main_600bf18c.jpg",
                    ItemId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    IsMain = true
                },
                new ItemImage
                {
                    Id = Guid.Parse("750CBEAB-A9C6-4E80-8F07-7D44041E5543"),
                    Url = "https://muzikercdn.com/uploads/products/11857/1185775/thumb_large_d_gallery_febb8645.jpg",
                    ItemId = Guid.Parse("45B325C8-E2B3-4571-9223-F7FB06B8FD3A"),
                    IsMain = true
                },
                new ItemImage
                {
                    Id = Guid.Parse("D0A6186F-ACED-4A4B-BDA1-7F95FEA6B036"),
                    Url = "https://muzikercdn.com/uploads/products/3876/387696/main_044ecb08.jpg",
                    ItemId = Guid.Parse("1F261899-F152-4B34-BD1B-51D21B077F57"),
                    IsMain = true
                },
                new ItemImage
                {
                    Id = Guid.Parse("B4255949-C04B-4620-B7C7-8201736E4526"),
                    Url = "https://muzikercdn.com/uploads/products/4301/430113/thumb_large_d_gallery_base_bbf86abb.jpg",
                    ItemId = Guid.Parse("66F2BA23-232E-4573-86FF-EF04D7572F2C"),
                    IsMain = true
                },
                new ItemImage
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    Url = "https://muzikercdn.com/uploads/products/4270/427019/thumb_large_d_gallery_base_e0067575.jpg",
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
                    Id = Guid.Parse("98BDDE14-41A1-45E3-B47F-A994B09A8E47"),
                    Url = "https://muzikercdn.com/uploads/product_gallery/2286/228605/main_24ef7713.png",
                    ItemId = Guid.Parse("68F29CE0-6435-4CEE-BC5A-521C166F71E3"),
                    IsMain = false
                },
                new ItemImage
                {
                    Id = Guid.Parse("CCCCCCC1-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
                    Url = "https://www.emp.ie/dw/image/v2/BBQV_PRD/on/demandware.static/-/Sites-master-emp/default/dwa9ad3e79/images/1/0/5/8/105863a.jpg?sfrm=png",
                    ItemId = Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDD0001"),
                    IsMain = true
                },
                new ItemImage
                {
                    Id = Guid.Parse("CCCCCCC2-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
                    Url = "https://www.emp.ie/dw/image/v2/BBQV_PRD/on/demandware.static/-/Sites-master-emp/default/dwe630cd96/images/1/0/5/8/105863b.jpg?sfrm=png",
                    ItemId = Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDD0001"),
                    IsMain = false
                },
                new ItemImage
                {
                    Id = Guid.Parse("CCCCCCC3-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
                    Url = "https://muzikercdn.com/uploads/products/3972/397246/thumb_large_d_gallery_base_dc6883f7.jpg",
                    ItemId = Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDD0002"),
                    IsMain = true
                },
                new ItemImage
                {
                    Id = Guid.Parse("CCCCCCC4-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
                    Url = "https://muzikercdn.com/uploads/product_gallery/3972/397247/main_692dc8f6.jpg",
                    ItemId = Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDD0002"),
                    IsMain = false
                },
                new ItemImage
                {
                    Id = Guid.Parse("CCCCCCC5-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
                    Url = "https://i.ebayimg.com/00/s/MTUzN1gxNjAw/z/RFMAAOSwntxiBu1x/$_57.JPG?set_id=8800005007",
                    ItemId = Guid.Parse("DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDD0003"),
                    IsMain = true
                }
            };
        }
    }
}