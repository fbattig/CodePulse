using System;
using System.ComponentModel.DataAnnotations;

namespace CodePulse.API.Models.Domains
{
    public class Category
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string UrlHandle { get; set; } = string.Empty;
    }
}
