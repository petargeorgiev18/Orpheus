﻿using System;
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
                    Description = "A leading brand in musical instruments.",
                    LogoUrl = "https://example.com/logo.png"
                },
                new Brand
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Fender",
                    Description = "Fender is an American manufacturer of stringed instruments and amplifiers.",
                    LogoUrl = "https://example.com/fender-logo.png"
                },
                new Brand
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Gibson",
                    Description = "Gibson is an American manufacturer of guitars, other musical instruments, and accessories.",
                    LogoUrl = "https://example.com/gibson-logo.png"
                },
                new Brand
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    Name = "Elektra Records",
                    Description = "Record label for Metallica's albums including 'Master of Puppets'",
                    LogoUrl = "/images/brands/elektra_records.png"
                },
                new Brand
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    Name = "Island Records",
                    Description = "Label for Lady Gaga's 'Born This Way'",
                    LogoUrl = "/images/brands/island_records.png"
                },
                new Brand
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Name = "Harvest Records",
                    Description = "Label for Pink Floyd's 'The Dark Side of the Moon'",
                    LogoUrl = "/images/brands/harvest_records.jpg"
                }
            };
        }
    }
}
