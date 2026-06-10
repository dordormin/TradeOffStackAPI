namespace TradeOffStackAPI.Models;
using System.Text.Json.Serialization;

public enum AssetStatus
{
    None,
    Available,    // Disponible
    Reserved,     // Réservé par un employé
    OutForRepair, // En réparation
    Retired       // Retiré de l'inventaire/Obsolète
}

public enum AssetCategory
{
    None,
    Laptop,
    Monitor,
    Peripheral,
    NetworkDevice
}

public enum DepreciationMethod
{
    None,
    StraightLine,     // Linéaire
    DecliningBalance  // Dégressif
}
/// <summary>
/// Représente un équipement (ordinateur, écran, etc.) dans l'inventaire de l'entreprise.
/// </summary>
public class Equipment
{
    /// <summary>Identifiant unique de l'équipement.</summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    /// <summary>Nom usuel de l'équipement (ex: MacBook Pro 2023).</summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Numéro de série unique du constructeur.</summary>
    [JsonPropertyName("serial_number")]
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary>Statut actuel (Disponible, Réservé, etc.).</summary>
    [JsonPropertyName("status")]
    public AssetStatus Status { get; set; } = AssetStatus.None;

    /// <summary>Catégorie de matériel.</summary>
    [JsonPropertyName("category")] public AssetCategory Category { get; set; } = AssetCategory.None;

    /// <summary>Description détaillée du matériel.</summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>Prix d'achat unitaire.</summary>
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    /// <summary>Nom du fichier image associé.</summary>
    [JsonPropertyName("image")]
    public string Image { get; set; } = string.Empty;

    /// <summary>URL directe vers l'image.</summary>
    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>URL sécurisée vers l'image.</summary>
    [JsonPropertyName("image_url_https")]
    public string ImageUrlHttps { get; set; } = string.Empty;
    
    /// <summary>Date d'acquisition du matériel.</summary>
    [JsonPropertyName("purchase_date")]
    public DateTime? PurchaseDate { get; set; }
    
    /// <summary>Date d'enregistrement dans le système.</summary>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Valeur comptable actuelle (calculée dynamiquement).</summary>
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    [JsonPropertyName("current_book_value")]
    public decimal CurrentBookValue { get; set; }

    /// <summary>Valeur résiduelle estimée (valeur de récupération).</summary>
    [JsonPropertyName("salvage_value")]
    public decimal SalvageValue { get; set; }

    /// <summary>Durée d'amortissement estimée en années.</summary>
    [JsonPropertyName("useful_life_years")]
    public int UsefulLifeYears { get; set; }

    /// <summary>Méthode d'amortissement financière.</summary>
    [JsonPropertyName("depreciation_method")]
    public DepreciationMethod DepreciationMethod { get; set; } = DepreciationMethod.None;

    /// <summary>Date d'expiration de la garantie.</summary>
    [JsonPropertyName("warranty_expiration_date")]
    public DateTime? WarrantyExpirationDate { get; set; }

    /// <summary>Historique des réservations de cet équipement.</summary>
    [JsonIgnore]
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    /// <summary>Historique des demandes de maintenance de cet équipement.</summary>
    [JsonIgnore]
    public ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();

    /// <summary>Licences logicielles installées sur cet équipement.</summary>
    [JsonIgnore]
    public ICollection<EquipmentLicense> EquipmentLicenses { get; set; } = new List<EquipmentLicense>();
}