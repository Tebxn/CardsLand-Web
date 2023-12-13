using CardsLand_Api.Interfaces;
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
        private readonly IPokemonTcg _pokeTcg;
        private String _urlApi;
        private readonly ITools _tools;


        public DeckModel(HttpClient httpClient, IConfiguration configuration, 
            IHttpContextAccessor httpContextAccessor, ITools tools, IPokemonTcg pokemonTcg)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _HttpContextAccessor = httpContextAccessor;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;
            _tools = tools;
            _pokeTcg = pokemonTcg;
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

        public async Task<ApiResponse<DeckEnt>> GetSpecificDeck(long deckId)
        {
            ApiResponse<DeckEnt> response = new ApiResponse<DeckEnt>();
            try
            {
                string userToken = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                string url = $"{_urlApi}/api/Deck/GetSpecificDeck/{deckId}";

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<DeckEnt>>(json);
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

        public Task<ApiResponse<DeckEnt>> EditDeck(long deckId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<CardEnt>>> GetCardsFromDeck(long deckId)
        {
            ApiResponse<List<CardEnt>> response = new ApiResponse<List<CardEnt>>();
            List<CardEnt> cards = new List<CardEnt>();
            try
            {
                string userToken = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                string url = $"{_urlApi}/api/Deck/GetCardsFromDeck/{deckId}";
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<List<CardEnt>>>(json);

                    foreach (var item in response.Data) 
                    {
                        CardEnt cardEnt = new CardEnt();
                        cardEnt = await _pokeTcg.GetSpecificCardbyId(item.Id);
                        cards.Add(cardEnt);
                    }
                    response.Data = cards;
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
