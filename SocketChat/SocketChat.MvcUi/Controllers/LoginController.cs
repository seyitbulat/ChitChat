using Microsoft.AspNetCore.Mvc;
using SocketChat.MvcUi.GlobalStroge;
using SocketChat.MvcUi.Models;
using System.Diagnostics;

namespace SocketChat.MvcUi.Controllers
{
    
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/Login/Login")]
        public async Task<IActionResult> Login(UserModel dto)
        {
            if(dto == null)
            {
                return Json(new {IsSuccess = false, Message="Hata" });
            }
			if (!(ActiveUsers.Users.Any(x => x.UserName.Contains(dto.UserName))))
			{
				ActiveUsers.Users.Add(dto);
				ActiveUser.User = dto;

			}
			return Json(new { IsSuccess = true, Message = "Basarili" });
        }
    }
}
