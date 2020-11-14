namespace EasyServices.Data.Seeding.DTO
{
    using System.Text.Json.Serialization;

    public class ImportCitiesDTO
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
