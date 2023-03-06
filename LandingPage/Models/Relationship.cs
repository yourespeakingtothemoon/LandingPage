using System.ComponentModel.DataAnnotations;

namespace LandingPage.Models
{
    public class Relationship
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Follower { get; set; }
        [Required]
        public string Followee { get; set; }

        public Relationship(string follower, string followee)
        {
            Follower = follower;
            Followee = followee;
        }
    }
}
