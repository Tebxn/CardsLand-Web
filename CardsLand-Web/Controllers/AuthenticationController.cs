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
                HttpContext.Session.SetString("UserName", resp.Data.User_Name);
                HttpContext.Session.SetString("UserName", resp.Data.User_LastName);
                HttpContext.Session.SetString("UserCompanyId", resp.Data.User_Company_Id.ToString());
              

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.MensajePantalla = resp.ErrorMessage;
                return View();
            }
        }
        //[HttpGet] Se comenta metodo porque GetAllUsersRoles no existe aun
        //public async Task<IActionResult> RegisterAccount()
        //{
        //    try
        //    {
        //        var roleDropdownData = await _userModel.GetAllUsersRoles();
        //        ViewBag.ListRoles = roleDropdownData.Data.Select(role => new SelectListItem
        //        {
        //            Value = role.User_Type_Id.ToString(),
        //            Text = role.User_Type_Name
        //        });
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.MensajePantalla = "Error al cargar los datos";
        //        return View();
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> RegisterAccount(UserEnt entity)
        {
            var apiResponse = await _userModel.RegisterAccount(entity);

            if (apiResponse.Success)
            {
                return RedirectToAction("Employees", "User");
            }
            else
            {
                ViewBag.MensajePantalla = "No se realizaron cambios";
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
