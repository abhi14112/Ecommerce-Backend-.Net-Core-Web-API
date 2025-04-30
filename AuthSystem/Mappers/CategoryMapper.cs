using AuthSystem.DTOs;
using AuthSystem.Models;

namespace AuthSystem.Mappers
{
    public static class CategoryMappingExtension
    {
        public static CategoryModel ToCategoryModel(this CategoryDTO categoryDTO)
        {
            return new CategoryModel
            {
                CategoryName = categoryDTO.CategoryName,
                CategoryImage = categoryDTO.CategoryImage,
            };
        }public static UpdateCategoryDTO ToCategoryDTO
(this CategoryModel category)
        {
            return new UpdateCategoryDTO
            {
                CategoryModelId = category.CategoryModelId,
                CategoryName = category.CategoryName,
                CategoryImage = category.CategoryImage,
            };
        }
        public static List<UpdateCategoryDTO> ToCategoryList(this List<CategoryModel> categories)
        {
            var result = categories.Select(category => category.ToCategoryDTO()).ToList();
            return result;
        }
    }
}
