using CardsLand_Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetProfile(string userToken)
        {
            try
            {
                var apiResponse = await _userModel.GetSpecificUserFromToken(userToken);

                if (apiResponse.Success)
                {
                    var user = apiResponse.Data;
                    if (user != null)
                    {
                        ViewBag.UserToken = userToken;
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
    }
}
