using Application.Mapping;
using AutoMapper;
using Domain.Models;
using System;

namespace Application.Dtos.CategoryDtos
{
    public class CreateCategoryDto : IMap
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateCategoryDto, Category>()
                .ReverseMap();
        }
    }
}