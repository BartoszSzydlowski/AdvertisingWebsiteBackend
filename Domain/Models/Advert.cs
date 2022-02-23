using Domain.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Advert : AuditableEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [Required]
        public Category Category { get; set; }

        [Required]
        public bool IsPromoted { get; set; }

        [Required]
        public bool IsAccepted { get; set; }

        [Required]
        public bool IsExpired { get; set; }

        [Required]
        public string UserId { get; set; }

        public ICollection<Picture> Pictures { get; set; }
    }
}