using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
    
        public async Task AddDepartmentAsync(Department department)
        {
            await _departmentRepository.AddAsync(department);
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync(bool? onlyActiveRecords = true)
        {
           return await _departmentRepository.GetAllAsync(onlyActiveRecords);
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            return await _departmentRepository.GetByIdAsync(id);
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            await _departmentRepository.UpdateAsync(department);
        }
    }
}
