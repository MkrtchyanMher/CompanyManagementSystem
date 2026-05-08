using Company.Application.Common;
using Company.Application.DTO.Company;
using Company.Application.DTO.Employee;
using Company.Application.DTO.Project;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Company.Application.Interfaces
{
    public interface ICompanyService : IBaseService<Application.DTO.Company.Company, CreateCompany, UpdateCompany>
    {
    }
}
