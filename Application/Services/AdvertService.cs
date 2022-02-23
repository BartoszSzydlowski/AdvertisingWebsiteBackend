using Application.Dtos.AdvertDtos;
using Application.Interfaces.Advert;
using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AdvertService : IAdvertService
    {
        private readonly IAdvertRepository _repo;
        private readonly IMapper _mapper;

        public AdvertService(IAdvertRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AdvertDto>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<AdvertDto>>(items);
        }

        public async Task<AdvertDto> GetByIdAsync(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            return _mapper.Map<AdvertDto>(item);
        }

        public async Task<AdvertDto> AddAsync(CreateAdvertDto newAdvert, string userId)
        {
            var advert = _mapper.Map<Advert>(newAdvert);
            advert.UserId = userId;
            var item = await _repo.AddAsync(advert);
            return _mapper.Map<AdvertDto>(item);
        }

        public async Task UpdateAsync(UpdateAdvertDto updateAdvert)
        {
            var existingAdvert = await _repo.GetByIdAsync(updateAdvert.Id);
            var item = _mapper.Map(updateAdvert, existingAdvert);
            await _repo.UpdateAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            await _repo.DeleteAsync(item);
        }

        public async Task<bool> UserOwnsAsync(int postId, string userId)
        {
            var advert = await _repo.GetByIdAsync(postId);

            if (advert == null)
            {
                return false;
            }

            if (advert.UserId != userId)
            {
                return false;
            }

            return true;
        }

        public async Task<int> CountAsync(string filterBy)
        {
            return await _repo.CountAsync(filterBy);
        }

        public async Task<int> CountByUserIdAsync(string filterBy, string userId)
        {
            return await _repo.CountByUserIdAsync(filterBy, userId);
        }

        public async Task<int> CountByCategoryAsync(string filterBy, int? categoryId)
        {
            return await _repo.CountByCategoryAsync(filterBy, categoryId);
        }

        public async Task<int> CountByCategoryAndAcceptStatusAsync(string filterBy, int? categoryId, bool? isAccepted)
        {
            return await _repo.CountByCategoryAndAcceptStatusAsync(filterBy, categoryId, isAccepted);
        }

        public async Task<int> CountByAcceptStatusAsync(string filterBy, bool isAccepted)
        {
            return await _repo.CountByAcceptStatusAsync(filterBy, isAccepted);
        }

        public async Task<IEnumerable<AdvertDto>> GetAllPaged(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy)
        {
            var items = await _repo.GetAllPaged(pageNumber, pageSize, sortField, ascending, filterBy);
            return _mapper.Map<IEnumerable<AdvertDto>>(items);
        }

        public async Task<IEnumerable<AdvertDto>> GetAllPagedByUserIdAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy, string userId)
        {
            var items = await _repo.GetAllPagedByUserIdAsync(pageNumber, pageSize, sortField, ascending, filterBy, userId);
            return _mapper.Map<IEnumerable<AdvertDto>>(items);
        }

        public async Task<IEnumerable<AdvertDto>> GetAllPagedByCategoryAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy, int? categoryId)
        {
            var items = await _repo.GetAllPagedByCategoryAsync(pageNumber, pageSize, sortField, ascending, filterBy, categoryId);
            return _mapper.Map<IEnumerable<AdvertDto>>(items);
        }

        public async Task<IEnumerable<AdvertDto>> GetAllPagedByCategoryAndAcceptStatusAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy, int? categoryId, bool? isAccepted)
        {
            var items = await _repo.GetAllPagedByCategoryAndAcceptStatusAsync(pageNumber, pageSize, sortField, ascending, filterBy, categoryId, isAccepted);
            return _mapper.Map<IEnumerable<AdvertDto>>(items);
        }

        public async Task<IEnumerable<AdvertDto>> GetAllPagedByAcceptStatusAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy, bool isAccepted)
        {
            var items = await _repo.GetAllPagedByAcceptStatusAsync(pageNumber, pageSize, sortField, ascending, filterBy, isAccepted);
            return _mapper.Map<IEnumerable<AdvertDto>>(items);
        }
    }
}