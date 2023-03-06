using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandingPage.Models
{
    public class Photo
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string url { get; set; }
        [Required]
        
        [ForeignKey("User")]
        public string AppUserID { get; set; }

        public Photo(string url, string userId)
        {
            this.url = url;
            AppUserID = userId;
        }
        public Photo()
        { }
    }
}
