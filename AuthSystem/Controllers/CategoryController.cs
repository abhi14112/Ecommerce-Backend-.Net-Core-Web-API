using AuthSystem.DTOs;
using AuthSystem.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _category;
        public CategoryController(ICategoryRepository category)
        {
            _category = category;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDTO category)
        {
            var result = await _category.CreateCategoryAsync(category);
            if(result)
            {
                return Ok("Category Created");
            }
            return BadRequest("Failed to create Category");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _category.DeleteCategory(id);
            if (result)
            {
                return Ok("Category Deleted Successfully");
            }
            return BadRequest("Failed to delete Category");
        }

        [HttpPut]
        public async Task<IActionResult>UpdateCategory(UpdateCategoryDTO categoryDTO)
        {
            var result = await _category.UpdateCategoryAsync(categoryDTO);
            if (result)
            {
                return Ok("Category Updated");
            }
            return BadRequest("Failed to update category");
        }
        [HttpGet("all")]
        public async Task<IActionResult> AllCategories()
        {
            var result = await _category.GetAllCategoriesAsync();
            if(result != null)
                return Ok(result);
            return BadRequest("Failed to get Categories");
        }
    }
}
