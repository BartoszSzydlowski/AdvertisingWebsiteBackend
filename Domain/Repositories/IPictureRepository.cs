using Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IPictureRepository
    {
        Task<IEnumerable<Picture>> GetAllAsync();

        Task<Picture> GetByIdAsync(Guid id);

        Task<Picture> AddAsync(Picture picture);

        Task UpdateAsync(Picture picture);

        Task DeleteAsync(Picture picture);
    }
}