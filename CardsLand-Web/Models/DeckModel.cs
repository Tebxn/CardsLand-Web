using CardsLand_Web.Entities;
using CardsLand_Web.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace CardsLand_Web.Models
{
    public class DeckModel : IDeckModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private String _urlApi;
        private readonly ITools _tools;


        public DeckModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ITools tools)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _HttpContextAccessor = httpContextAccessor;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;
            _tools = tools;
        }

        public async Task<ApiResponse<List<DeckEnt>>> GetAllUserDecks(long userId)
        {
            ApiResponse<List<DeckEnt>> response = new ApiResponse<List<DeckEnt>>();
            try
            {
                string userToken = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                string url = $"{_urlApi}/api/Deck/GetAllUserDecks/{userId}";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<List<DeckEnt>>>(json);
                    return response;
                }

                response.ErrorMessage = "Error al obtener mazos del servidor.";
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
    }
}
