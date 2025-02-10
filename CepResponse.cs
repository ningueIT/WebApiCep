using System.Text.Json.Serialization;

public class CepResponse
{
    [JsonPropertyName("cep")]
    public string ZipCode { get; set; }

    [JsonPropertyName("logradouro")]
    public string Street { get; set; }

    [JsonPropertyName("complemento")]
    public string Complement { get; set; }

    [JsonPropertyName("bairro")]
    public string Neighborhood { get; set; }

    [JsonPropertyName("localidade")]
    public string City { get; set; }

    [JsonPropertyName("uf")]
    public string Uf { get; set; }

    public string State { get; set; }
}