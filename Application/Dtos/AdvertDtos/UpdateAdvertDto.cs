using Application.Mapping;
using AutoMapper;
using Domain.Models;
using System;

namespace Application.Dtos.AdvertDtos
{
    public class UpdateAdvertDto : IMap
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public bool IsPromoted { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsExpired { get; set; }
        public DateTime CreationDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateAdvertDto, Advert>()
                .ReverseMap();
        }
    }
}