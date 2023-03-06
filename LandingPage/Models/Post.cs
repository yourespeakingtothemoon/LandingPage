using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandingPage.Models
{
    public class Post
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        
        [Required]
        public bool IsRaw { get; set; } = false;
        public Photo? Image { get; set; }
        [Required]
		public DateTime DatePosted { get; set; } = DateTime.Now;

		[Required]
        [ForeignKey("User")]
        public string AppUserId { get; set; }

        public Post(string? title, string? content, Photo? image, string userId)
        {
            Title = title;
            Content = content;
            Image = image;
            AppUserId = userId;
        }
        public Post()
        {
        }

        public Post(string? title, string? content, string userId)
        {
            Title = title;
            Content = content;
            AppUserId = userId;
        }

        public Post(string? title, Photo? image, string userId)
        {
            Title = title;
            Image = image;
            AppUserId = userId;
        }

        public Post(Photo? image, string userId)
        {
            Image = image;
            AppUserId = userId;
        }

       // public List<Comment> Comments { get; set; } = new List<Comment>();
        // public List<Like> Likes { get; set; } = new List<Like>();



    }
}
