namespace TradeOffStackAPI.Models;
using System.Text.Json.Serialization;

/// <summary>
/// Représente une licence logicielle gérée par l'entreprise.
/// </summary>
public class SoftwareLicense
{
    /// <summary>Identifiant unique de la licence.</summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>Nom du logiciel (ex: Microsoft Office 365).</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Clé de licence (peut être vide pour les abonnements basés sur le cloud).</summary>
    [JsonPropertyName("license_key")]
    public string LicenseKey { get; set; } = string.Empty;

    /// <summary>Nombre total de postes/licences achetés.</summary>
    [JsonPropertyName("total_seats")]
    public int TotalSeats { get; set; }

    /// <summary>Date d'expiration de la licence ou de l'abonnement.</summary>
    [JsonPropertyName("expiration_date")]
    public DateTime? ExpirationDate { get; set; }

    /// <summary>Prix d'achat global ou annuel.</summary>
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    /// <summary>Date d'enregistrement dans le système.</summary>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Historique des assignations de cette licence à des équipements.</summary>
    [JsonIgnore]
    public ICollection<EquipmentLicense> EquipmentLicenses { get; set; } = new List<EquipmentLicense>();
}
