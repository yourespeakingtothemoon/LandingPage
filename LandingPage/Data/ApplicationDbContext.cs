using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LandingPage.Models;
using Microsoft.AspNetCore.Identity;

namespace LandingPage.Data
{
	public class ApplicationDbContext : IdentityDbContext<User>
	{
        public DbSet<Post> Posts { get; set; }
        public DbSet<Photo> Photos { get; set; }
		public override DbSet<User> Users { get; set; } 
        public DbSet<Link> Links { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbSet<Relationship> Relationships{ get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
	}
}