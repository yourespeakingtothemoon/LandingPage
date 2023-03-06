using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandingPage.Models
{
    public class Link
    {
        public enum LinkType
        {
            Social,
            Personal,
            Professional,
            Content
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string URL { get; set; }

        [ForeignKey("User")]
        public string AppUserId { get; set; }

        [Required]
        public LinkType Type { get; set; } = LinkType.Social;
        public Link()
        {
            
        }
        public Link(string url, string userId)
        {
            URL = url;
            AppUserId = userId;
            //Type = type;
        }
       
    }
}
