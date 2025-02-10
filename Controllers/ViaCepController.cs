using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Collections.Generic;

[ApiController]
[Route("api/cep")]
public class ViaCepController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private static readonly Dictionary<string, string> Estados = new()
    { { "AC", "Acre" }, { "AL", "Alagoas" }, { "AP", "Amap�" }, { "AM", "Amazonas" }, { "BA", "Bahia" }, { "CE", "Cear�" }, { "DF", "Distrito Federal" }, { "ES", "Esp�rito Santo" }, { "GO", "Goi�s" }, { "MA", "Maranh�o" }, { "MT", "Mato Grosso" }, { "MS", "Mato Grosso do Sul" }, { "MG", "Minas Gerais" }, { "PA", "Par�" }, { "PB", "Para�ba" }, { "PR", "Paran�" }, { "PE", "Pernambuco" }, { "PI", "Piau�" }, { "RJ", "Rio de Janeiro" }, { "RN", "Rio Grande do Norte" }, { "RS", "Rio Grande do Sul" }, { "RO", "Rond�nia" }, { "RR", "Roraima" }, { "SC", "Santa Catarina" }, { "SP", "S�o Paulo" }, { "SE", "Sergipe" }, { "TO", "Tocantins" } };

    public ViaCepController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("{cep}")]
    public async Task<IActionResult> GetCepInfo(string cep)
    {
        if (string.IsNullOrWhiteSpace(cep) || cep.Length != 8)
        {
            return BadRequest("CEP inv�lido. Certifique-se de inserir um CEP com 8 d�gitos.");
        }

        string url = $"https://viacep.com.br/ws/{cep}/json/";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return NotFound("CEP n�o encontrado.");
        }

        var json = await response.Content.ReadAsStringAsync();
        var cepInfo = JsonSerializer.Deserialize<CepResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (cepInfo == null || cepInfo.Uf == null)
        {
            return NotFound("CEP n�o encontrado.");
        }

        if (cepInfo.Uf == "AC")
        {
            return StatusCode(403, "n�o � poss�vel consultar para estado acre.");
        }

        cepInfo.State = Estados.ContainsKey(cepInfo.Uf) ? Estados[cepInfo.Uf] : null;

        return Ok(cepInfo);
    }
}