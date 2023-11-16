using CardsLand_Web.Entities;
using CardsLand_Web.Interfaces;
using CardsLand_Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CardsLand_Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IUserModel _userModel;


        public AuthenticationController(ILogger<AuthenticationController> logger, IUserModel userModel)
        {
            _logger = logger;
            _userModel = userModel;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserEnt entity)
        {

            var resp = await _userModel.Login(entity);


            if (resp.Success)
            {
                HttpContext.Session.SetString("UserId", resp.Data.User_Id.ToString());
                HttpContext.Session.SetString("UserNickname", resp.Data.User_Nickname);
                HttpContext.Session.SetString("UserToken", resp.Data.UserToken);


                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.MensajePantalla = resp.ErrorMessage;
                return View();
            }
        }

        [HttpGet]
        public IActionResult EndSession()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult RegisterAccount()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RegisterAccount(UserEnt entity)
        {
            try
            {
                var apiResponse = await _userModel.RegisterAccount(entity);

                if (apiResponse.Success)
                {
                    return RedirectToAction("Login", "Authentication");
                }
                else
                {
                    ViewBag.MensajePantalla = "No se pudo registrar la cuenta";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.MensajePantalla = "Error al cargar los datos";
                return View();
            }

        }
        public IActionResult AuthRecoverPW()
        {
            return View();
        }
        public IActionResult Auth404()
        {
            return View();
        }
        public IActionResult Auth500()
        {
            return View();
        }
    }
}
