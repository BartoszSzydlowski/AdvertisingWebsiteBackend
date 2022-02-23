using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PictureRepository : IPictureRepository
    {
        private readonly Context _context;

        public PictureRepository(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Picture>> GetAllAsync()
        {
            return await _context.Pictures.ToListAsync();
        }

        public async Task<Picture> GetByIdAsync(Guid id)
        {
            return await _context.Pictures.SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<Picture> AddAsync(Picture picture)
        {
            var createdEntity = await _context.Pictures.AddAsync(picture);
            picture.UniqueName = $"{picture.Id}{picture.Extension}";
            await _context.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public async Task DeleteAsync(Picture picture)
        {
            _context.Pictures.Remove(picture);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Picture picture)
        {
            _context.Pictures.Update(picture);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }
    }
}