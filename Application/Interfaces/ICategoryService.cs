using Application.Dtos.CategoryDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Category
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();

        Task<CategoryDto> GetByIdAsync(int id);

        Task<CategoryDto> AddAsync(CreateCategoryDto newCategory, string userId);

        Task UpdateAsync(UpdateCategoryDto updateCategory);

        Task DeleteAsync(int id);

        Task<bool> UserOwnsAsync(int categoryId, string userId);
    }
}