using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Orpheus.Data.Models.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasData(CreateBrands());
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
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Name = "Harvest Records",
                    LogoUrl = "/images/brands/harvest_records.jpg"
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
                    Id = Guid.Parse("aaaaaaa1-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Name = "Rammstein",
                    LogoUrl = "/images/brands/rammstein.png"
                },
                new Brand
                {
                    Id = Guid.Parse("bbbbbbb2-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Name = "Slayer",
                    LogoUrl = "/images/brands/slayer.png"
                },
                new Brand
                {
                    Id = Guid.Parse("ccccccc3-cccc-cccc-cccc-cccccccccccc"),
                    Name = "Metallica",
                    LogoUrl = "/images/brands/metallica.png"
                }
            };
        }
    }
}
