using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IAdvertRepository
    {
        Task<IEnumerable<Advert>> GetAllAsync();

        //Task<IEnumerable<Advert>> GetAllByUserIdAsync(string userId);

        Task<Advert> GetByIdAsync(int id);

        Task<Advert> AddAsync(Advert advert);

        Task UpdateAsync(Advert advert);

        Task DeleteAsync(Advert advert);

        Task<int> CountAsync(string filterBy);

        Task<int> CountByUserIdAsync(string filterBy, string userId);

        Task<int> CountByCategoryAsync(string filterBy, int? categoryId);

        Task<int> CountByCategoryAndAcceptStatusAsync(string filterBy, int? categoryId, bool? isAccepted);

        Task<int> CountByAcceptStatusAsync(string filterBy, bool isAccepted);

        Task<IEnumerable<Advert>> GetAllPaged(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy);

        Task<IEnumerable<Advert>> GetAllPagedByUserIdAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy, string userId);

        Task<IEnumerable<Advert>> GetAllPagedByCategoryAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy, int? categoryId);

        Task<IEnumerable<Advert>> GetAllPagedByCategoryAndAcceptStatusAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy, int? categoryId, bool? isAccepted);

        Task<IEnumerable<Advert>> GetAllPagedByAcceptStatusAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy, bool isAccepted);
    }
}