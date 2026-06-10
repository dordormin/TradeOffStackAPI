namespace TradeOffStackAPI.Models;
using System.Text.Json.Serialization;

/// <summary>
/// Table de liaison représentant l'assignation d'une licence logicielle à un équipement.
/// </summary>
public class EquipmentLicense
{
    /// <summary>Identifiant de l'équipement.</summary>
    [JsonPropertyName("equipment_id")]
    public Guid EquipmentId { get; set; }

    /// <summary>L'équipement associé.</summary>
    [JsonIgnore]
    public Equipment? Equipment { get; set; }

    /// <summary>Identifiant de la licence logicielle.</summary>
    [JsonPropertyName("software_license_id")]
    public Guid SoftwareLicenseId { get; set; }

    /// <summary>La licence logicielle associée.</summary>
    [JsonIgnore]
    public SoftwareLicense? SoftwareLicense { get; set; }

    /// <summary>Date à laquelle la licence a été installée/assignée à l'équipement.</summary>
    [JsonPropertyName("assigned_at")]
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}
