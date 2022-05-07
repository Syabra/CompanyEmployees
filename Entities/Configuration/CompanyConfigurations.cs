using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configuration
{
    public class CompanyConfigurations : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasData
                (
                    new Company
                    {
                        Id = new Guid("ed62a714-b958-4e64-a399-c4cb975dcd58"),
                        Name = "Dvoinoi podborodok",
                        Address = "She9 andre9",
                        Country = "Liberty"
                    },

                    new Company
                    {
                        Id = new Guid("7b2e2490-0148-41a3-ba6a-6da08c699d7b"),
                        Name = "ShershaVa9 p9tka",
                        Address = "U kajdogo vtorogo",
                        Country = "Meste4kova9"
                    }
                );
        }
    }
}
