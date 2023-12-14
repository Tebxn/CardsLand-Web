using CardsLand_Web.Entities;
using CardsLand_Web.Implementations;
using CardsLand_Web.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CardsLand_Web.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserModel _userModel;

        private readonly IHttpContextAccessor _HttpContextAccessor;


        public UserController(IUserModel userModel, IHttpContextAccessor httpContextAccessor)
        {
            _userModel = userModel;
            _HttpContextAccessor = httpContextAccessor;

        }


        [HttpGet]
        [SecurityFilter]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var apiResponse = await _userModel.GetSpecificUserFromToken();

                if (apiResponse.Success)
                {
                    var user = apiResponse.Data;
                    if (user != null)
                    {
                        return View(user);
                    }
                    else
                    {
                        ViewBag.MensajePantalla = "No se pudo desplegar el perfil";
                        return View();
                    }
                }
                else
                {
                    ViewBag.MensajePantalla = "No se logro conexion con el servidor";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.MensajePantalla = "Error al cargar los datos";
                return View();
            }
        }


        [HttpGet]
        [SecurityFilter]
        [SecurityFilterIsAdmin]

        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var apiResponse = await _userModel.GetAllUsers();
                var listUsers = apiResponse.Data.ToList();
                return View(listUsers);
            }
            catch (Exception)
            {
                List<UserEnt> errors = new List<UserEnt>();
                return View(errors);
            }
        }

        [HttpGet]
        //[SecurityFilter]
        //[SecurityFilterIsAdmin]
        public async Task<IActionResult> GetSpecificUser(long userId)
        {
            try
            {
                var apiResponse = await _userModel.GetSpecificUser(userId);
                if (apiResponse.Success)
                {
                    var user = apiResponse.Data;
                    if (user != null)
                    {
                        return RedirectToAction("EditSpecificUser", new { UserId = userId });
                    }
                    else
                    {
                        return View("Error");
                    }
                }
                else
                {
                    return View("Error"); 
                }
            }
            catch (Exception ex)
            {
                
                return View("Error"); 
            }
        }

        [HttpGet]
        [SecurityFilter]
        [SecurityFilterIsAdmin]
        public async Task<IActionResult> EditSpecificUser(long userId)
        {
            try
            {
                var apiResponse = await _userModel.GetSpecificUser(userId);
                if (apiResponse.Success)
                {
                    var user = apiResponse.Data;
                    if (user != null)
                    {

                        return View("EditSpecificUser", user);
                    }
                    else
                    {
                        ViewBag.MensajePantalla = apiResponse.ErrorMessage;
                        return View("GetAllUsers");
                    }
                }
                else
                {
                    ViewBag.MensajePantalla = apiResponse.ErrorMessage;
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpGet]
        [SecurityFilter]
        public async Task<IActionResult> EditSpecificProfile(long userId)
        {
            try
            {
                var apiResponse = await _userModel.GetSpecificUser(userId);
                if (apiResponse.Success)
                {
                    var user = apiResponse.Data;
                    if (user != null)
                    {

                        return View("EditSpecificUser", user);
                    }
                    else
                    {
                        ViewBag.MensajePantalla = apiResponse.ErrorMessage;
                        return View("GetProfile");
                    }
                }
                else
                {
                    ViewBag.MensajePantalla = apiResponse.ErrorMessage;
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditSpecificUser(UserEnt user)
        {
            var apiResponse = await _userModel.EditSpecificUser(user);

            if (apiResponse.Success)
            {
                // Obtener el valor de la variable de sesión UserIsAdmin
                bool isAdmin = HttpContext.Session.GetString("UserIsAdmin") == "True";

                // Redirigir a la vista correspondiente
                return RedirectToAction(isAdmin ? "GetAllUsers" : "GetProfile", "User");
            }
            else
            {
                ViewBag.MensajePantalla = apiResponse.ErrorMessage;
                return View();
            }
        }

        [HttpGet]
        [SecurityFilter]
        [SecurityFilterIsAdmin]
        public async Task<IActionResult> UpdateUserState(long userId)
        {
            try
            {
                
                var apiResponse = await _userModel.GetSpecificUser(userId);

                if (apiResponse.Success)
                {
                    var user = apiResponse.Data;

                    
                    var updateResponse = await _userModel.UpdateUserState(user);

                    if (updateResponse.Success)
                    {
                        return RedirectToAction("GetAllUsers", "User");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = updateResponse.ErrorMessage;
                        return  RedirectToAction("GetAllUsers", "User");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = apiResponse.ErrorMessage;
                    return  RedirectToAction("GetAllUsers", "User");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al cambiar el estado del usuario: " + ex.Message;
                return View("Error");
            }
        }

    }
}
