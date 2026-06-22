using System;
using System.ComponentModel.DataAnnotations;

namespace CodePulse.API.Models.Domains
{
    public class BlogPost
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string ShortDescription { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public string FeaturedImageUrl { get; set; } = string.Empty;

        [Required]
        public string UrlHandle { get; set; } = string.Empty;

        public DateTime PublishedDate { get; set; }

        [Required]
        public string Author { get; set; } = string.Empty;

        public bool IsVisible { get; set; }
    }
}
