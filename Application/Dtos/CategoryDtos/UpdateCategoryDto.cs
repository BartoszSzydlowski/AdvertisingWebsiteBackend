using Application.Mapping;
using AutoMapper;
using Domain.Models;

namespace Application.Dtos.CategoryDtos
{
    public class UpdateCategoryDto : IMap
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateCategoryDto, Category>()
                .ReverseMap();
        }
    }
}