using LandingPage.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LandingPage.Models
{
    public class User : IdentityUser
    {
		//private LPDAL _db = new LPDAL(new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>()));
		//[Key]
        //[Required]
        //public String Id { get; set; }
        //[Required]
        //[StringLength(50)]
        //disallow spaces
        //[Required]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Use letters and numbers only please")]
        [Remote(action: "VerifyUserName", controller: "Users")]
        public override string UserName { get; set; }
        //[Required]
        //public string Email { get; set; }
       // [Required]
        public string? Passwd { get; set; }
        
        public User(string name, string email, string password)
        {
            UserName = name;
            Email = email;
            Passwd = password;
        }
		public virtual ICollection<Post>? Posts { get; set; } = new List<Post>();

		public virtual ICollection<Photo>? Photos { get; set; } = new List<Photo>();

		//public virtual ICollection<User>? Following { get; set; } = new List<User>();

		public  ICollection<Relationship>? Following { get; set; } = new List<Relationship>();
		public  ICollection<Relationship>? Followers { get; set; } = new List<Relationship>();
        
		public virtual ICollection<Link>? Links { get; set; } = new List<Link>();

        public User() {

            



        }
        public void UserLoad(LPDAL db)
        {
			Links = db.GetLinksByUser(this.Id).Result;
			Posts = db.GetPostsByUser(this.Id).Result;
			Following = db.GetFollowing(this.Id).Result;
			Followers = db.GetFollowers(this.Id).Result;
			Photos = db.GetPhotosByUser(this.Id).Result;
		}

        public string? DisplayName { get; set; }
        public string? Bio { get; set; }
        public string ProfilePicture { get; set; } = "/img/profile/def1.png";
        public string CoverPicture { get; set; } = "/img/profile/cover/def1.png";
        public string? Location { get; set; }



		public void createProfile(string? displayName, string? bio, string? location, string profilePicture, string coverPicture)
        {
            DisplayName = displayName;
            Bio = bio;
            Location = location;
            ProfilePicture = profilePicture;
            CoverPicture = coverPicture;
        }



		


		
        public bool IsFollowing(User user)
        {
            //find the relationship where the follower is the current user and the followee is the user passed in
            var relationships = Following.Where(u => u.Followee == user.Id);
            if (relationships.Count() != 0)
            {
                return true;
            }
            return false;
            

            //return Followers.Contains(user);
        }

        public bool IsFollower(User user)
        {
			//find the relationship where the follower is the current user and the followee is the user passed in
			var relationships = Followers.Where(u => u.Follower == user.Id);
			if (relationships.Count() != 0)
			{
				return true;
			}
            return false;
		}

		public bool IsSelf(User user)
		{
			return this.Id == user.Id;
		}

		
        
		public void AddPost(Post post)
        {
            Posts.Add(post);
        }

        public void AddPhoto(Photo photo)
        {
            Photos.Add(photo);
        }

        public void AddLink(Link link)
        {
            Links.Add(link);
        }
   

    }
}
