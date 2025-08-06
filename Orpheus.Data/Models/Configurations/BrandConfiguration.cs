using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orpheus.Data.Models;

namespace Orpheus.Data.Models.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            //builder.HasData(CreateBrands());
        }

        public IEnumerable<Brand> CreateBrands()
        {
            return new List<Brand>
            {
                new Brand
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "ESP LTD",
                    LogoUrl = "https://example.com/logo.png"
                },
                new Brand
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Fender",
                    LogoUrl = "https://example.com/fender-logo.png"
                },
                new Brand
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Gibson",
                    LogoUrl = "https://example.com/gibson-logo.png"
                },
                new Brand
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "Dunlop",
                    LogoUrl = "https://example.com/dunlop-logo.png"
                },
                new Brand
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Name = "Boss",
                    LogoUrl = "https://example.com/boss-logo.png"
                },
                new Brand
                {
                    Id = Guid.Parse("3D34833B-4451-423C-83F8-5DB4FE4EA67A"),
                    Name = "Yamaha",
                    LogoUrl = null
                },
                new Brand
                {
                    Id = Guid.Parse("C255FDE3-8805-4813-B815-743048AC1FDF"),
                    Name = "Universal Music Group",
                    LogoUrl = null
                },
                new Brand
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    Name = "Elektra Records",
                    LogoUrl = "/images/brands/elektra_records.png"
                },
                new Brand
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    Name = "Island Records",
                    LogoUrl = "/images/brands/island_records.png"
                },
                new Brand
                {
                    Id = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
                    Name = "Harvest Records",
                    LogoUrl = "/images/brands/harvest_records.jpg"
                },
                new Brand
                {
                    Id = Guid.Parse("AAAAAAA1-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
                    Name = "Rammstein",
                    LogoUrl = "/images/brands/rammstein.png"
                },
                new Brand
                {
                    Id = Guid.Parse("BBBBBBB2-BBBB-BBBB-BBBB-BBBBBBBBBBBB"),
                    Name = "Slayer",
                    LogoUrl = "/images/brands/slayer.png"
                },
                new Brand
                {
                    Id = Guid.Parse("CCCCCCC3-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
                    Name = "Metallica",
                    LogoUrl = "/images/brands/metallica.png"
                },
                new Brand
                {
                    Id = Guid.Parse("4A7EC03D-5A72-4265-8AA6-F893850B8EE9"),
                    Name = "Ibanez",
                    LogoUrl = null
                }
            };
        }
    }
}