using CardsLand_Web.Entities;
using System.Net.Http.Headers;
using System.Net.Http;
using CardsLand_Web.Interfaces;
using CardsLand_Web.Implementations;
using Newtonsoft.Json;
using System.ComponentModel;
using Azure;
using System.Net;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Base;
using PokemonTcgSdk.Standard.Infrastructure.HttpClients.Cards;

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
        public async Task<ApiResponse<ApiResourceList<Card>>> GetSpecificCardByName(string pokemonCardName)
        {
            ApiResponse<ApiResourceList<Card>> response = new ApiResponse<ApiResourceList<Card>>();
            try
            {
                string url = $"{_urlApi}/api/PokemonTcg/GetSpecificCardbyName/{pokemonCardName}";
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<ApiResourceList<Card>>>(json);
                    return response;
                }

                response.ErrorMessage = "Error al obtener cartas del servidor.";
                response.Code = (int)httpResponse.StatusCode;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error con el servidor: " + ex.Message;
                response.Code = 500;
                return response;
            }
        }



    }



}

