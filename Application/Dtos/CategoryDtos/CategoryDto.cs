using Application.Mapping;
using AutoMapper;
using Domain.Models;
using System;

namespace Application.Dtos.CategoryDtos
{
    public class CategoryDto : IMap
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.CreationDate,
                    opt => opt.MapFrom(src => src.Created))
                .ReverseMap();
        }
    }
}