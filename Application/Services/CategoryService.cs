using Application.Dtos.CategoryDtos;
using Application.Interfaces.Category;
using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(items);
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            return _mapper.Map<CategoryDto>(item);
        }

        public async Task<CategoryDto> AddAsync(CreateCategoryDto newCategory, string userId)
        {
            var category = _mapper.Map<Category>(newCategory);
            category.UserId = userId;
            var item = await _repo.AddAsync(category);
            return _mapper.Map<CategoryDto>(item);
        }

        public async Task UpdateAsync(UpdateCategoryDto updateCategoryDto)
        {
            var existingItem = await _repo.GetByIdAsync(updateCategoryDto.Id);
            var item = _mapper.Map(updateCategoryDto, existingItem);
            await _repo.UpdateAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            await _repo.DeleteAsync(item);
        }

        public async Task<bool> UserOwnsAsync(int categoryId, string userId)
        {
            var category = await _repo.GetByIdAsync(categoryId);

            if (category == null)
            {
                return false;
            }

            if (category.UserId != userId)
            {
                return false;
            }

            return true;
        }
    }
}