namespace TradeOffStackAPI.Models;
using System.Text.Json.Serialization;

/// <summary>
/// Fournisseur d'un service SaaS (ex: Microsoft, Adobe, Figma).
/// </summary>
public class SaaSProvider
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("website_url")]
    public string? WebsiteUrl { get; set; }

    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("contact_email")]
    public string? ContactEmail { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public ICollection<SaaSSubscription> Subscriptions { get; set; } = new List<SaaSSubscription>();
}
