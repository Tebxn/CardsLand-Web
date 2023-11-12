using CardsLand_Web.Entities;
using System.Net.Http.Headers;
using System.Net.Http;
using CardsLand_Web.Interfaces;
using CardsLand_Web.Implementations;
using Newtonsoft.Json;
using System.ComponentModel;

namespace CardsLand_Web.Models
{
    public class PokemonTcgModel: IPokemonTcgModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private String _urlApi;

        public PokemonTcgModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _HttpContextAccessor = httpContextAccessor;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;
        

        }

        public async Task<ApiResponse<List<CardEnt>>> GetSpecificCardByName(string pokemonCardName)
        {
            ApiResponse<List<CardEnt>> response = new ApiResponse<List<CardEnt>>();

            try
            {
                string url = $"{_urlApi}/api/PokemonTcg/GetSpecificCardbyName/{pokemonCardName}";

                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<List<CardEnt>>>(json);
                    return response;
                }

                response.ErrorMessage = $"Error al obtener cartas del API. Código: {(int)httpResponse.StatusCode}";
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

