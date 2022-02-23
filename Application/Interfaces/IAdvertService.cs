using Application.Dtos.AdvertDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Advert
{
    public interface IAdvertService
    {
        Task<IEnumerable<AdvertDto>> GetAllAsync();

        //Task<IEnumerable<AdvertDto>> GetAllByUserIdAsync(string userId);

        Task<AdvertDto> GetByIdAsync(int id);

        Task<AdvertDto> AddAsync(CreateAdvertDto newAdvert, string userId);

        Task UpdateAsync(UpdateAdvertDto updateAdvert);

        Task DeleteAsync(int id);

        Task<bool> UserOwnsAsync(int postId, string userId);

        Task<int> CountAsync(string filterBy);

        Task<int> CountByUserIdAsync(string filterBy, string userId);

        Task<int> CountByCategoryAsync(string filterBy, int? categoryId);

        Task<int> CountByCategoryAndAcceptStatusAsync(string filterBy, int? categoryId, bool? isAccepted);

        Task<int> CountByAcceptStatusAsync(string filterBy, bool isAccepted);

        Task<IEnumerable<AdvertDto>> GetAllPaged(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy);

        Task<IEnumerable<AdvertDto>> GetAllPagedByUserIdAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy, string userId);

        Task<IEnumerable<AdvertDto>> GetAllPagedByCategoryAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy, int? categoryId);

        Task<IEnumerable<AdvertDto>> GetAllPagedByCategoryAndAcceptStatusAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy, int? categoryId, bool? isAccepted);

        Task<IEnumerable<AdvertDto>> GetAllPagedByAcceptStatusAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy, bool isAccepted);
    }
}