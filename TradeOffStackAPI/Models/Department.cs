namespace TradeOffStackAPI.Models;
using System.Text.Json.Serialization;

/// <summary>
/// Représente un département ou service de l'entreprise (ex: IT, RH, Finance).
/// </summary>
public class Department
{
    /// <summary>Identifiant unique du département.</summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>Nom du département.</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Description de la mission ou des responsabilités du service.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Date de création du département dans le système.</summary>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Liste des utilisateurs rattachés à ce département.</summary>
    [JsonIgnore]
    public ICollection<User> Users { get; set; } = new List<User>();
}
