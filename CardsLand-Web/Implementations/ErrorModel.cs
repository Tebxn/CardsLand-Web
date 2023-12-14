using CardsLand_Web.Entities;
using System.Net.Http.Headers;
using System.Net.Http;
using CardsLand_Api.Interfaces;
using CardsLand_Web.Interfaces;

namespace CardsLand_Web.Implementations
{
    public class ErrorModel : IError
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private String _urlApi;
        private readonly ITools _tools;


        public ErrorModel(HttpClient httpClient, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _HttpContextAccessor = httpContextAccessor;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;
        }

        public async Task<ApiResponse<ErrorEnt>> AddError(ErrorEnt entity)
        {
            ApiResponse<ErrorEnt> response = new ApiResponse<ErrorEnt>();

            try
            {
                string url = _urlApi + "/api/User/AddError";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PostAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<ErrorEnt>>();
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Server Error.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Server Error: " + ex.Message;
                return response;
            }
        }
    }
}
