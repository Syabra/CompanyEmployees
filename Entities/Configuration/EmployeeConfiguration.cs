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
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasData
                (
                    new Employee
                    {
                        Id = new Guid("e375886b-07ce-4771-86b5-3dc3a62b3e23"),
                        Name = "Jack Jons",
                        Age = 47,
                        Position = "Manager",
                        CompanyId = new Guid("ed62a714-b958-4e64-a399-c4cb975dcd58")
                    },

                    new Employee
                    {
                        Id = new Guid("b69883db-64ee-467b-8e07-1ffd6c1545be"),
                        Name = "Dora Nemo",
                        Age = 31,
                        Position = "HR",
                        CompanyId = new Guid("ed62a714-b958-4e64-a399-c4cb975dcd58")
                    },

                    new Employee
                    {
                        Id = new Guid("1196e3e2-feff-40d1-b88b-4074f3c90d1f"),
                        Name = "Skoobi Doo",
                        Age = 18,
                        Position = "Barista",
                        CompanyId = new Guid("ed62a714-b958-4e64-a399-c4cb975dcd58")
                    },

                    new Employee
                    {
                        Id = new Guid("a4ac1859-e888-4bc7-a94b-40814190b679"),
                        Name = "Bazon Higgs",
                        Age = 59,
                        Position = "World Developer",
                        CompanyId = new Guid("7b2e2490-0148-41a3-ba6a-6da08c699d7b")
                    }
                );
        }
    }
}
