﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Common
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}