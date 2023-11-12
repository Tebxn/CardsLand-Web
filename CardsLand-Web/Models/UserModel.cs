using CardsLand_Web.Entities;
using CardsLand_Web.Interfaces;
using System.Net.Http.Headers;

namespace CardsLand_Web.Models
{
    public class UserModel : IUserModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private string _urlApi;
        private readonly ITools _tools;


        public UserModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ITools tools)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _HttpContextAccessor = httpContextAccessor;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;
            _tools = tools;

        }

        public async Task<ApiResponse<UserEnt>> Login(UserEnt entity)
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();

            try
            {
                string url = _urlApi + "/api/Authentication/Login";
                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PostAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<UserEnt>>();
                }
                else
                {
                    response.ErrorMessage = "Error al iniciar sesión. Verifica tus credenciales.";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error inesperado al iniciar sesión: " + ex.Message;
            }

            return response;
        }

        public async Task<ApiResponse<UserEnt>> RegisterAccount(UserEnt entity) //no sirve
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();

            try
            {
                string url = _urlApi + "/api/Authentication/RegisterAccount";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PostAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<UserEnt>>();
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Error Registrar Usuario. Verifique los datos.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error inesperado al registrar usuario: " + ex.Message;
                return response;
            }
        }

    }
}
