using Application.Dtos.PictureDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PictureService : IPictureService
    {
        private readonly IPictureRepository _repo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHost;

        public PictureService(IPictureRepository repo, IMapper mapper, IWebHostEnvironment webHost)
        {
            _mapper = mapper;
            _repo = repo;
            _webHost = webHost;
        }

        public async Task<IEnumerable<PictureDto>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<PictureDto>>(items);
        }

        public async Task<PictureDto> GetByIdAsync(Guid id)
        {
            var item = await _repo.GetByIdAsync(id);
            return _mapper.Map<PictureDto>(item);
        }

        public async Task<PictureDto> AddAsync(IFormFile file, int advertId, string userId)
        {
            var wwwroot = _webHost.WebRootPath + "/";
            var directory = "pictures/";
            if (!Directory.Exists(wwwroot + directory))
            {
                Directory.CreateDirectory(wwwroot + directory);
            }

            var picture = new Picture()
            {
                Path = wwwroot + file.FileName,
                Extension = Path.GetExtension(file.FileName),
                AdvertId = advertId,
                UserId = userId
            };

            var result = await _repo.AddAsync(picture);

            var createdPicture = await _repo.GetByIdAsync(picture.Id);
            var path = Path.Combine(wwwroot, directory, createdPicture.UniqueName);
            createdPicture.Path = Path.Combine(directory, createdPicture.UniqueName);
            await _repo.UpdateAsync(createdPicture);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return _mapper.Map<PictureDto>(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = await _repo.GetByIdAsync(id);
            await _repo.DeleteAsync(item);
        }

        public async Task UpdateAsync(PictureDto pictureDto)
        {
            var existingItem = await _repo.GetByIdAsync(pictureDto.Id);
            var item = _mapper.Map(pictureDto, existingItem);
            await _repo.UpdateAsync(item);
        }
    }
}