using AuthSystem.Data;
using AuthSystem.DTOs;
using AuthSystem.Mappers;
using AuthSystem.Models;
using AuthSystem.Repository.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Repository.Services
{
    public class CategoryRepository:ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateCategoryAsync(CategoryDTO category)
        {
            var newCategory = category.ToCategoryModel();
            await _context.Categories.AddAsync(newCategory);
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.CategoryModelId == id);
            if (category != null && (category.Products == null || !category.Products.Any()))
            {
                _context.Categories.Remove(category);
                var saved = await _context.SaveChangesAsync();
                return saved > 0;
            }
            return false;
        }
        public async Task<CategoryModel> GetSingleCategoryAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryModelId == id);
        }
        public async Task<bool> UpdateCategoryAsync(UpdateCategoryDTO categoryDTO)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryModelId == categoryDTO.CategoryModelId);
            if(category == null)
            {
                return false;
            }
            category.CategoryName = categoryDTO.CategoryName;
            if(categoryDTO.CategoryName != null)
            {
                category.CategoryImage = categoryDTO.CategoryImage;
            }
            var save = _context.SaveChanges();
            return save > 0;
        }
        public async Task<List<UpdateCategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            var result = categories.ToCategoryList();
            return result;
        }
    }
}