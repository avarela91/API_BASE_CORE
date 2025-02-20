using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class DepartmentRepository:GenericRepository<Department>,IDepartmentRepository
    {
        public DepartmentRepository(IConfiguration configuration):base(configuration) 
        {
        }
    }
}
