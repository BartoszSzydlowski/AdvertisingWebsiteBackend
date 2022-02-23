using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AdvertRepository : IAdvertRepository
    {
        private readonly Context _context;

        public AdvertRepository(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Advert>> GetAllAsync()
        {
            return await _context.Adverts
                .Include(x => x.Pictures)
                .Include(x => x.Category)
                .ToListAsync();
        }

        public async Task<Advert> GetByIdAsync(int id)
        {
            return await _context.Adverts.Include(x => x.Category)
                .Include(x => x.Pictures.Where(x => x.AdvertId == id))
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Advert> AddAsync(Advert advert)
        {
            advert.Category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == advert.CategoryId);
            var createdEntity = await _context.Adverts.AddAsync(advert);
            await _context.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public async Task UpdateAsync(Advert advert)
        {
            advert.Category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == advert.CategoryId);
            _context.Adverts.Update(advert);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Advert advert)
        {
            _context.Adverts.Remove(advert);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task<int> CountAsync(string filterBy)
        {
            return await _context.Adverts
                .Where(m => m.Name.ToLower().Contains(filterBy.ToLower()))
                .CountAsync();
        }

        public async Task<int> CountByUserIdAsync(string filterBy, string userId)
        {
            return await _context.Adverts
                .Where(m => m.Name.ToLower().Contains(filterBy.ToLower()))
                .Where(x => x.UserId == userId)
                .CountAsync();
        }

        public async Task<int> CountByCategoryAsync(string filterBy, int? categoryId)
        {
            return await _context.Adverts
                .Where(m => m.Name.ToLower().Contains(filterBy.ToLower()))
                .Where(x => x.CategoryId == categoryId)
                .CountAsync();
        }

        public async Task<int> CountByCategoryAndAcceptStatusAsync(string filterBy, int? categoryId, bool? isAccepted)
        {
            return await _context.Adverts
                .Where(m => m.Name.ToLower().Contains(filterBy.ToLower()))
                .Where(categoryId.HasValue ? x => x.CategoryId == categoryId : _ => true)
                .Where(isAccepted.HasValue ? x => x.IsAccepted == isAccepted : _ => true)
                .CountAsync();
        }

        public async Task<int> CountByAcceptStatusAsync(string filterBy, bool isAccepted)
        {
            return await _context.Adverts
                .Where(m => m.Name.ToLower().Contains(filterBy.ToLower()))
                .Where(x => x.IsAccepted == isAccepted)
                .CountAsync();
        }

        public async Task<IEnumerable<Advert>> GetAllPaged(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy)
        {
            return await _context.Adverts
                .Where(m => m.Name.ToLower().Contains(filterBy.ToLower()))
                .OrderByPropertyName(sortField, ascending)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Pictures)
                .Include(x => x.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Advert>> GetAllPagedByUserIdAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy, string userId)
        {
            return await _context.Adverts
                .Where(m => m.Name.ToLower().Contains(filterBy.ToLower()))
                .Where(x => x.UserId == userId)
                .OrderByPropertyName(sortField, ascending)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Pictures)
                .Include(x => x.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Advert>> GetAllPagedByCategoryAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy, int? categoryId)
        {
            return await _context.Adverts
                    .Where(m => m.Name.ToLower().Contains(filterBy.ToLower()))
                    .Where(categoryId.HasValue ? x => x.CategoryId == categoryId : _ => true)
                    .OrderByPropertyName(sortField, ascending)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Include(x => x.Pictures)
                    .Include(x => x.Category)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Advert>> GetAllPagedByCategoryAndAcceptStatusAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy, int? categoryId, bool? isAccepted)
        {
            return await _context.Adverts
                    .Where(m => m.Name.ToLower().Contains(filterBy.ToLower()))
                    .Where(categoryId.HasValue ? x => x.CategoryId == categoryId : _ => true)
                    .Where(isAccepted.HasValue ? x => x.IsAccepted == isAccepted : _ => true)
                    .OrderByPropertyName(sortField, ascending)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Include(x => x.Pictures)
                    .Include(x => x.Category)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Advert>> GetAllPagedByAcceptStatusAsync(int pageNumber, int pageSize, string sortField, bool ascending, string filterBy, bool isAccepted)
        {
            return await _context.Adverts
                .Where(m => m.Name.ToLower().Contains(filterBy.ToLower()))
                .Where(x => x.IsAccepted == isAccepted)
                .OrderByPropertyName(sortField, ascending)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Pictures)
                .Include(x => x.Category)
                .ToListAsync();
        }
    }
}