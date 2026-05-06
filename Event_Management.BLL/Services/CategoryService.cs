using Event_Management.BLL.Interfaces.Repositories;
using Event_Management.BLL.Interfaces.Services;
using Event_Management.CrossCutting.Entities;

namespace Event_Management.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> CreateCategory(Category category)
        {
            // Basic guard: avoid duplicate names
            var existing = await _categoryRepository.GetByNameAsync(category.Name);
            if (existing != null)
                throw new InvalidOperationException("Category with this name already exists");

            await _categoryRepository.AddAsync(category);
            return category;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }
    }
}