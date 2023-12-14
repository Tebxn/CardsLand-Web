using CardsLand_Api.Interfaces;
using CardsLand_Web.Entities;
using CardsLand_Web.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using static Dapper.SqlMapper;

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
                        cardEnt = await _pokeTcg.GetSpecificCardbyId(item.Card_Id);
                        cardEnt.Card_Quantity = item.Card_Quantity;
                        cards.Add(cardEnt);
                    }
                    response.Data = cards;
                    response.Success = true;
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

        public async Task<ApiResponse<DeckEnt>> CreateDeck(DeckEnt entity)
        {
            ApiResponse<DeckEnt> response = new ApiResponse<DeckEnt>();

            try
            {
                entity.Deck_User_Id = long.Parse(_HttpContextAccessor.HttpContext.Session.GetString("UserId"));

                string url = _urlApi + "/api/Deck/CreateDeck";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PostAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<DeckEnt>>();
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Error al crear mazo. Verifique los datos.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error inesperado al crear mazo: " + ex.Message;
                return response;
            }
        }

        public async Task<ApiResponse<CardDeckEnt>> AddCardToDeck(CardDeckEnt entity)
        {
            ApiResponse<CardDeckEnt> response = new ApiResponse<CardDeckEnt>();
            try
            {
                string url = _urlApi + "/api/Deck/AddCardToDeck";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PostAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<CardDeckEnt>>();
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Error al agregar carta. Verifique los datos.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error inesperado al crear mazo: " + ex.Message;
                return response;
            }
        }

        public async Task<ApiResponse<DeckEnt>> EditDeckValues(DeckEnt entity)
        {
            ApiResponse<DeckEnt> response = new ApiResponse<DeckEnt>();
            try
            {
                string url = _urlApi + "/api/Deck/EditDeckValues";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PutAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<DeckEnt>>();
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Error al editar mazo. Verifique los datos.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al editar mazo: " + ex.Message;
                return response;
            }
        }

        public async Task<ApiResponse<DeckEnt>> DeleteDeck(long deckId)
        {
            ApiResponse<DeckEnt> response = new ApiResponse<DeckEnt>();
            try
            {
                string url = $"{_urlApi}/api/Deck/DeleteDeck/{deckId}";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var httpResponse = await _httpClient.DeleteAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<DeckEnt>>();
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Error al eliminar mazo. Verifique los datos.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al eliminar mazo: " + ex.Message;
                return response;
            }
        }

        public async Task<ApiResponse<DeckEnt>> DeleteCardFromDeck(long deckId, string cardId)
        {
            ApiResponse<DeckEnt> response = new ApiResponse<DeckEnt>();
            try
            {
                string url = $"{_urlApi}/api/Deck/DeleteCardFromDeck?deckId={deckId}&cardId={cardId}";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var httpResponse = await _httpClient.DeleteAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<DeckEnt>>();
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Error al eliminar carta del mazo. Verifique los datos.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al eliminar carta del mazo: " + ex.Message;
                return response;
            }
        }
    }
}
