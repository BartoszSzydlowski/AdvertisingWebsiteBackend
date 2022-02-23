using Domain.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Picture : AuditableEntity
    {
        [Key]
        public new Guid Id { get; set; }

        [Required]
        public string UniqueName { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public string Extension { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int AdvertId { get; set; }

        public Advert Advert { get; set; }
    }
}