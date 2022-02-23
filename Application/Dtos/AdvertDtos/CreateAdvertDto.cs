using Application.Mapping;
using AutoMapper;
using Domain.Models;

namespace Application.Dtos.AdvertDtos
{
    public class CreateAdvertDto : IMap
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateAdvertDto, Advert>()
                .ReverseMap();
        }
    }
}