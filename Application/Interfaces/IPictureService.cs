using Application.Dtos.PictureDtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPictureService
    {
        Task<IEnumerable<PictureDto>> GetAllAsync();

        Task<PictureDto> GetByIdAsync(Guid id);

        Task<PictureDto> AddAsync(IFormFile file, int advertId, string userId);

        Task UpdateAsync(PictureDto pictureDto);

        Task DeleteAsync(Guid id);
    }
}