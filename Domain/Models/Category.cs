using Domain.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Category : AuditableEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public ICollection<Advert> Adverts { get; set; }
    }
}