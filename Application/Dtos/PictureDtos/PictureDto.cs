using Application.Mapping;
using AutoMapper;
using Domain.Models;
using System;

namespace Application.Dtos.PictureDtos
{
    public class PictureDto : IMap
    {
        public Guid Id { get; set; }
        public string UniqueName { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
        public int AdvertId { get; set; }
        public string UserId { get; set; }
        //public AdvertDto Advert { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Picture, PictureDto>()
                .ReverseMap();
        }
    }
}