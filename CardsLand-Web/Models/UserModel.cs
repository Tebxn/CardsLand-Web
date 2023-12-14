using CardsLand_Web.Entities;
using CardsLand_Web.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using System.Data;
using NuGet.Common;
using Azure;
using static Dapper.SqlMapper;
using System.Web;

namespace CardsLand_Web.Models
{
    public class UserModel : IUserModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private String _urlApi;
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
                _tools.AddError(ex.Message);
                response.ErrorMessage = "Error inesperado al iniciar sesión: " + ex.Message;
            }

            return response;
        }

        public async Task<ApiResponse<UserEnt>> RegisterAccount(UserEnt entity)
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();

            try
            {
                string url = _urlApi + "/api/Authentication/RegisterAccount";
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

        public async Task<ApiResponse<UserEnt>> GetSpecificUserFromToken()
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();
            try
            {
                // Encripta el userToken antes de pasarlo en la URL
                //string encryptedUserToken = _tools.Encrypt(userToken);
                string userToken = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");

                string url = _urlApi + "/api/User/GetSpecificUserFromToken?q=" + userToken;

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<UserEnt>>(json);
                    return response;
                }
                else
                {
                    response.ErrorMessage = "No se pudo concretar la solicitud";
                    response.Code = (int)httpResponse.StatusCode;
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Unexpected Error: " + ex.Message;
                response.Code = 500;
                return response;
            }
        }

        public async Task<ApiResponse<List<UserEnt>>> GetAllUsers()
        {
            ApiResponse<List<UserEnt>> response = new ApiResponse<List<UserEnt>>();
            try
            {
                string url = _urlApi + "/api/User/GetAllUsers";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<List<UserEnt>>>(json);
                    return response;
                }

                response.ErrorMessage = "Error al obtener usuarios del API.";
                response.Code = (int)httpResponse.StatusCode;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Unexpected Error: " + ex.Message;
                response.Code = 500;
                return response;
            }
        }

        public async Task<ApiResponse<UserEnt>> GetSpecificUser(long userId)
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();
            try
            {
                // Encripta el userId antes de pasarlo en la URL
                string encryptedUserId = _tools.Encrypt(userId.ToString());
                string encodedHashedId = HttpUtility.UrlEncode(encryptedUserId);


                string url = $"{_urlApi}/api/User/GetSpecificUser/{encodedHashedId}";

                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<UserEnt>>(json);
                    return response;
                }

                response.ErrorMessage = "Error al obtener el usuario del API.";
                response.Code = (int)httpResponse.StatusCode;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Unexpected Error: " + ex.Message;
                response.Code = 500;
                return response;
            }
        }

        public async Task<ApiResponse<UserEnt>> EditSpecificUser(UserEnt entity)
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();

            try
            {
                string url = $"{_urlApi}/api/User/EditSpecificUser";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                JsonContent obj = JsonContent.Create(entity);

                var httpResponse = await _httpClient.PutAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response.Data = await httpResponse.Content.ReadFromJsonAsync<UserEnt>();
                }
                else if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    response.ErrorMessage = "User not found";
                    response.Code = 404;
                }
                else
                {
                    response.ErrorMessage = "Unexpected Error: " + httpResponse.ReasonPhrase;
                    response.Code = (int)httpResponse.StatusCode;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Unexpected Error: " + ex.Message;
                response.Code = 500;
            }

            return response;

        }

        public async Task<ApiResponse<UserEnt>> PwdRecovery(UserEnt entity)
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();

            try
            {
                string url = _urlApi + "/api/Authentication/PwdRecovery";
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
                    response.ErrorMessage = "Error al restablecer contraseña.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error inesperado al restablecer contraseña: " + ex.Message;
                return response;
            }

        }

        public async Task<ApiResponse<UserEnt>> UpdateNewPassword(UserEnt entity)
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();

            try
            {
                string url = _urlApi + "/api/Authentication/UpdateNewPassword";
                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PutAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<UserEnt>>();
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Error al actualizar la contraseña.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error inesperado al actualizar la contraseña: " + ex.Message;
                return response;
            }

        }

        public async Task<ApiResponse<UserEnt>> UpdateUserState(UserEnt entity)
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();

            try
            {
                string url = _urlApi + "/api/User/UpdateUserState";
                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PutAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response.Data = await httpResponse.Content.ReadFromJsonAsync<UserEnt>();
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Error al conectar con el servidor. Contacte con soporte.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error inesperado al actualizar usuario: " + ex.Message;
                return response;
            }
        }

        public async Task<ApiResponse<UserEnt>> ActivateAccount(UserEnt entity)
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();
            try
            {
                string url = _urlApi + "/api/Authentication/ActivateAccount";
                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PutAsync(url, obj);
                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<UserEnt>>();
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Error al autorizar acceso.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error inesperado al autorizar acceso: " + ex.Message;
                return response;
            }
        }
    }
}

