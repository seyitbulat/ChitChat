using Microsoft.AspNetCore.Mvc;
using SocketChat.MvcUi.GlobalStroge;
using SocketChat.MvcUi.Models;

namespace SocketChat.MvcUi.Controllers
{
    public class HomeController : Controller
    {
        
 
		public IActionResult Index()
        {

            
            return View(ActiveUser.User);
        }

        [HttpPost]
       public IActionResult GetUser(UserModel dto)
        {
          return RedirectToAction("Index","Home");
        }
    }
}
