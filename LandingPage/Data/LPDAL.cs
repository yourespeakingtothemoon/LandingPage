using LandingPage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;

namespace LandingPage.Data
{
    public class LPDAL
    {
        private ApplicationDbContext _db;

        public LPDAL(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Post>> GetPosts()
        {
            return await _db.Posts.ToListAsync();
        }

        public async Task<List<Photo>> GetPhotos()
        {
            return await _db.Photos.ToListAsync();
        }

        public async Task<List<Link>> GetLinks()
        {
            return await _db.Links.ToListAsync();
        }

        public async Task<List<User>> GetUsers()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<List<Relationship>> GetFollowers(string id)
        {
            return await _db.Relationships.Where(u => u.Followee == id).ToListAsync();
        }
        
        public async Task<List<Relationship>> GetFollowing(string id)
        {
            return await _db.Relationships.Where(u => u.Follower == id).ToListAsync();
        }
        //REMOVE FOLLOWING
        public async Task Unfollow(string id, string followingId)
        {
            _db.Relationships.Remove(_db.Relationships.Where(u => u.Follower == id && u.Followee == followingId).FirstOrDefault());
            await _db.SaveChangesAsync();
        }

        public async Task<List<Post>> GetPostsByUser(string id)
        {
            return await _db.Posts.Where(p => p.AppUserId == id).ToListAsync();
        }
        
        public async Task<List<Photo>> GetPhotosByUser(string id)
        {
            return await _db.Photos.Where(p => p.AppUserID == id).ToListAsync();
        }
        
        public async Task<List<Link>> GetLinksByUser(string id)
        {
            return await _db.Links.Where(p => p.AppUserId == id).ToListAsync();
        }

     public void editUser(User user)
        {
            _db.Users.Update(user);
            _db.SaveChanges();
        }

        public void deleteUser(User user)
        {
            _db.Users.Remove(user);
            _db.SaveChanges();
        }

        public void deletePost(Post post)
        {
            _db.Posts.Remove(post);
            _db.SaveChanges();
        }

        public void deletePhoto(Photo photo)
        {
            _db.Photos.Remove(photo);
            _db.SaveChanges();
        }

      public void removeLink(int i)
        {

            //List<Link> tempList = _db.Links.ToList();

            _db.Links.Remove(_db.Links.Where(p=>p.Id == i).First());
            _db.SaveChanges();
        }

        public void addPost(Post post)
        {
            _db.Posts.Add(post);
            _db.SaveChanges();
        }

        public void addMessage(Message message)
        {
            _db.Messages.Add(message);
            _db.SaveChanges();
        }

        public void addPhoto(Photo photo)
        {
            _db.Photos.Add(photo);
            _db.SaveChanges();
        }

        public void addLink(Link link)
        {
            _db.Links.Add(link);
            _db.SaveChanges();
        }

      

        public void Follow(User user, User following)
        {
            _db.Relationships.Add(new Relationship ( user.Id, following.Id ));
            _db.SaveChanges();
        }

   

        public List<Message> getInbox(string idReceived) => _db.Messages.Where(i => i.toUserID == idReceived).ToList<Message>();
        public List<Message> getOutbox(string idSent) => _db.Messages.Where(o => o.fromUserID == idSent).ToList<Message>();

        public async Task<User> GetUserById(string id)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

     public async Task<User> GetUserByName(string name)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.NormalizedUserName.ToLower() == name);
        }

        //get all posts based on a list of user IDs
       public async Task<List<Post>> GetFollowingPosts(User usr)
        {
            List<Post> folPosts=new List<Post>();
            usr.UserLoad(this);
          
            if (usr.Posts != null)
            {
                folPosts.AddRange(usr.Posts);
            }
            //loop through every user in the list
            foreach(Relationship f in usr.Following)
            {
				//get the user
				folPosts.AddRange(_db.Posts.Where(p => p.AppUserId == f.Followee));
			}
       
            folPosts.Sort((x, y) => x.DatePosted.CompareTo(y.DatePosted));
			folPosts.Reverse();
			return folPosts;
            

        }
        
        public void UpdateDB()
        {
            _db.SaveChanges();
        }

    }
}
