using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Company.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Company.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (await context.Companies.AnyAsync()) return;

            var now = DateTime.UtcNow;

            var contoso = new Domain.Entities.Company
            {
                Id = Guid.NewGuid(),
                Name = "Contoso",
                Website = "www.contoso.com",
                CreatedTime = now
            };

            var fabrikam = new Domain.Entities.Company
            {
                Id = Guid.NewGuid(),
                Name = "Fabrikam",
                Website = "www.fabrikam.com",
                CreatedTime = now
            };

            await context.Companies.AddRangeAsync(contoso, fabrikam);
            await context.SaveChangesAsync();

            var contosoEmployees = Enumerable.Range(1, 10).Select(i => new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = $"ContosoFirst{i}",
                LastName = $"ContosoLast{i}",
                Email = $"contoso{i}@contoso.com",
                HireDate = now.AddDays(-i * 30),
                CompanyId = contoso.Id,
                CreatedTime = now
            }).ToList();

            var fabrikamEmployees = Enumerable.Range(1, 10).Select(i => new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = $"FabrikamFirst{i}",
                LastName = $"FabrikamLast{i}",
                Email = $"fabrikam{i}@fabrikam.com",
                HireDate = now.AddDays(-i * 30),
                CompanyId = fabrikam.Id,
                CreatedTime = now
            }).ToList();

            await context.Employees.AddRangeAsync(contosoEmployees);
            await context.Employees.AddRangeAsync(fabrikamEmployees);
            await context.SaveChangesAsync();

            var contosoProjects = new List<Project>
            {
                new Project { Id = Guid.NewGuid(), Name = "Contoso Project 1", StartDate = now, CompanyId = contoso.Id, CreatedTime = now },
                new Project { Id = Guid.NewGuid(), Name = "Contoso Project 2", StartDate = now, CompanyId = contoso.Id, CreatedTime = now },
                new Project { Id = Guid.NewGuid(), Name = "Contoso Project 3", StartDate = now, CompanyId = contoso.Id, CreatedTime = now }
            };

            var fabrikamProjects = new List<Project>
            {
                new Project { Id = Guid.NewGuid(), Name = "Fabrikam Project 1", StartDate = now, CompanyId = fabrikam.Id, CreatedTime = now },
                new Project { Id = Guid.NewGuid(), Name = "Fabrikam Project 2", StartDate = now, CompanyId = fabrikam.Id, CreatedTime = now },
                new Project { Id = Guid.NewGuid(), Name = "Fabrikam Project 3", StartDate = now, CompanyId = fabrikam.Id, CreatedTime = now }
            };

            await context.Projects.AddRangeAsync(contosoProjects);
            await context.Projects.AddRangeAsync(fabrikamProjects);
            await context.SaveChangesAsync();

            var random = new Random();

            foreach (var employee in contosoEmployees)
            {
                var projects = contosoProjects.OrderBy(x => random.Next()).Take(random.Next(1, 3)).ToList();
                foreach (var project in projects)
                {
                    context.EmployeeProjects.Add(new EmployeeProject
                    {
                        EmployeeId = employee.Id,
                        ProjectId = project.Id,
                        AssignedDate = now
                    });
                }
            }

            foreach (var employee in fabrikamEmployees)
            {
                var projects = fabrikamProjects.OrderBy(x => random.Next()).Take(random.Next(1, 3)).ToList();
                foreach (var project in projects)
                {
                    context.EmployeeProjects.Add(new EmployeeProject
                    {
                        EmployeeId = employee.Id,
                        ProjectId = project.Id,
                        AssignedDate = now
                    });
                }
            }

            await context.SaveChangesAsync();
        }
    }
}