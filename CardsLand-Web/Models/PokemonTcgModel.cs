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
        public async Task<ApiResourceList<Card>> GetSpecificCardByName(string pokemonCardName)
        {
            ApiResponse<ApiResourceList<Card>> apiResponse = new ApiResponse<ApiResourceList<Card>>();
            

            try
            {
                string url = $"{_urlApi}/api/PokemonTcg/GetSpecificCardbyName/?pokemonCardName={pokemonCardName}";

                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    apiResponse = JsonConvert.DeserializeObject<ApiResponse<ApiResourceList<Card>>>(json);

                    // Verificar si la respuesta contiene datos y la propiedad "results" no es nula.
                    if (apiResponse?.Data?.Results != null)
                    {
                        // Mapear los datos al tipo de tu modelo local (CardEnt).
                        response.Data = apiResponse.Data.Results.Select(card => new CardEnt
                        {
                            // Mapear las propiedades según sea necesario.
                            // Ejemplo:
                            CardId = card.Id,
                            CardName = card.Name,
                            // ... mapear otras propiedades ...
                        }).ToList();
                    }

                    // Propagar otras propiedades de ApiResponse (Success, ErrorMessage, Code).
                    response.Success = apiResponse.Success;
                    response.ErrorMessage = apiResponse.ErrorMessage;
                    response.Code = apiResponse.Code;

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

