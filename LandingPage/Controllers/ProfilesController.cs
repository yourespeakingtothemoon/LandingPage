using LandingPage.Data;
using LandingPage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LandingPage.Controllers
{
	public class ProfilesController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		private LPDAL _lpdal;
		private UserManager<User> _userManager;

		public ProfilesController(ILogger<HomeController> logger, ApplicationDbContext context)
		{
			_logger = logger;
			_lpdal = new LPDAL(context);

		}
		public IActionResult Index()
		{
			return View();
		}
		
		public IActionResult Search(string filter)
		{
			var currentUser = _lpdal.GetUserByName(User.Identity.Name).Result;
			currentUser.UserLoad(_lpdal);
			List<User> users =_lpdal.GetUsers().Result;
			if (filter != null)
			{
				users = users.Where(u => u.DisplayName.ToLower().Contains(filter.ToLower())||u.Email.ToLower().Contains(filter.ToLower())).ToList();
				
			}
			ViewBag.filter = filter;
			return View(users);
		}
		
		public IActionResult SearchRoute(string filter)
		{
			return Redirect("~/search/" + filter);
		}

		public IActionResult Follow(string id)
        {
            var user = _lpdal.GetUserByName(User.Identity.Name).Result;
            var follow = _lpdal.GetUserById(id).Result;
			_lpdal.Follow(user, follow);
            //user.Follow(follow);
           // _lpdal.UpdateDB();
            
            return Redirect(Request.Headers["Referer"].ToString());

        }
        public IActionResult Unfollow(string id)
        {
            var user = _lpdal.GetUserByName(User.Identity.Name).Result;
			
			
            _lpdal.Unfollow(user.Id,id);
           // _lpdal.UpdateDB();
           
            return Redirect(Request.Headers["Referer"].ToString());
        }

		public IActionResult Card(string id)
		{
			var user = _lpdal.GetUserById(id).Result;
			user.UserLoad(_lpdal);
			return View(user);
		}


		public IActionResult Profile(String id)
		{
			var currentUser = _lpdal.GetUserByName(User.Identity.Name).Result;
			currentUser.UserLoad(_lpdal);
			var user = _lpdal.GetUserById(id).Result;
			ViewBag.MessageList = _lpdal.getInbox(user.Id);
			
			user.UserLoad(_lpdal);
            return View(user);
		}
	}
}
