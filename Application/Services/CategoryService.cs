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
    public class CategoryService: ICategoryService
    {
        private readonly ICategoryRepository _categoryService;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryService = categoryRepository;
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool? onlyActiveRecords = true)
        {
            return await _categoryService.GetAllAsync(onlyActiveRecords);
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _categoryService.GetByIdAsync(id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _categoryService.AddAsync(category);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await _categoryService.UpdateAsync(category);
        }
    }
}
