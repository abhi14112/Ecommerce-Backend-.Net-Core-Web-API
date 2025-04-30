using AuthSystem.DTOs;
using AuthSystem.Models;

namespace AuthSystem.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<bool> CreateCategoryAsync(CategoryDTO category);
        Task<List<UpdateCategoryDTO>> GetAllCategoriesAsync();
        Task<bool> DeleteCategory(int id);
        Task<bool> UpdateCategoryAsync(UpdateCategoryDTO categoryDTO);
        Task<CategoryModel> GetSingleCategoryAsync(int id);
    }
}
