using LandingPage.Data;
using LandingPage.Models;
using System.Web;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using NuGet.Packaging.Signing;

namespace LandingPage.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		private LPDAL _lpdal;
        private UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
		{
			_logger = logger;
            _lpdal = new LPDAL(context);
		
        }
		
		public IActionResult Index()
		{
			var user = _lpdal.GetUserByName(User.Identity.Name).Result;
			//var posts = _lpdal.GetPosts(user);
			if (user != null)
			{
				if (user.DisplayName == null)
				{
					return RedirectToAction("ProfileEdit", "Home");
				}

				return Redirect("/Home/Homepage");
			}
			return View();
		}

		public IActionResult Homepage()
		{
			var user = _lpdal.GetUserByName(User.Identity.Name).Result;
			//var posts = _lpdal.GetPosts();
			//get a list of user Ids for users followed by user
			//var following = _lpdal.GetFollowing(user.Id);
			//get a list of posts from users followed by user
			//var followingPosts = _lpdal.GetFollowingPosts(user).Result;
			user.UserLoad(_lpdal);

            var usrs = _lpdal.GetUsers().Result;
			var posts = _lpdal.GetPosts().Result;
            //loop to use the UserLoad method on each user in the list
            foreach (var u in usrs)
            {
				u.UserLoad(_lpdal);
            }
			
            return View(_lpdal);


			
		}

	
		public IActionResult About()
        {
            return View();
        }
		
        public IActionResult ProfileEdit()
		{
            if (User.Identity.IsAuthenticated)
			{
				var user = _lpdal.GetUserByName(User.Identity.Name).Result;
				//user.UserLoad(_lpdal);
				var users = _lpdal.GetUsers().Result;
                foreach (var u in users)
                {
                    u.UserLoad(_lpdal);
                }
                return View(user);
				//return View( _lpdal.GetUserByName(User.Identity.Name.ToLower()).Result);
            }
			else
			{
                return Redirect("~/Identity/Account/Login");
            }
                
        }

		public IActionResult ChangePFP(string pfp)
		{
            var user = _lpdal.GetUserByName(User.Identity.Name).Result;
            user.ProfilePicture = pfp;
            _lpdal.editUser(user);
            return Redirect("~/Home/ProfileEdit");
        }
        public IActionResult ChangeCOV(string cov)
        {
            var user = _lpdal.GetUserByName(User.Identity.Name).Result;
            user.CoverPicture = cov;
            _lpdal.editUser(user);
            return Redirect("~/Home/ProfileEdit");
        }

        [HttpPost]
        public IActionResult Edit( string disname, string bio, string location)
        {
            var user = _lpdal.GetUserByName(User.Identity.Name).Result;
           
            user.DisplayName = disname;
            user.Bio = bio;
            user.Location = location;
            _lpdal.editUser(user);
            return Redirect("~/Home/ProfileEdit");
        }

        public IActionResult AddLink(string URL)
		{
			Link l = new Link(URL, _lpdal.GetUserByName(User.Identity.Name.ToLower()).Result.Id);
			_lpdal.addLink(l);
			//_lpdal.GetUserByName(User.Identity.Name.ToLower()).Result.AddLink(l);
			return RedirectToAction("ProfileEdit");
		}
		public IActionResult RemoveLink(int l)
		{
			_lpdal.removeLink(l);
			return RedirectToAction("ProfileEdit");
		}

	
		public IActionResult NewPost(string? content, string? photo, string? title)
		{
			if (content == null && photo == null && title == null)
			{
				return Redirect("~/Home/Homepage");
			}
			else
			{
				Photo phototopost = null;
				var user = _lpdal.GetUserByName(User.Identity.Name).Result;
				if (photo != null)
				{
					phototopost = new Photo(photo, user.Id);
					_lpdal.addPhoto(phototopost);
				}
				Post p = new Post(title, content, phototopost, user.Id);
				_lpdal.addPost(p);
				return Redirect("~/Home/Homepage");
			}
        }
        public IActionResult NewRawPost(string content, string? title)
        {
            var user = _lpdal.GetUserByName(User.Identity.Name).Result;
            Post p = new Post(title, content, user.Id);
			p.IsRaw = true;
            _lpdal.addPost(p);
            return Redirect("~/Home/Homepage");
        
        }

		public IActionResult SendMessage(string message, string fromid, string toid,bool isPublic)
		{
			_lpdal.addMessage(new Message(message, fromid, toid,isPublic));
			return Redirect(Request.Headers["Referer"].ToString());

		}

	

        public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}