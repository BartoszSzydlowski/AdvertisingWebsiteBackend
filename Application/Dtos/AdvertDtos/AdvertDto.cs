using Application.Dtos.CategoryDtos;
using Application.Dtos.PictureDtos;
using Application.Mapping;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;

namespace Application.Dtos.AdvertDtos
{
    public class AdvertDto : IMap
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public CategoryDto Category { get; set; }
        public List<PictureDto> Pictures { get; set; }
        public bool IsPromoted { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsExpired { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Advert, AdvertDto>()
                .ForMember(dest => dest.CreationDate,
                    opt => opt.MapFrom(src => src.Created))
                .ReverseMap();
        }
    }
}