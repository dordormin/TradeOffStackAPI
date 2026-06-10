namespace TradeOffStackAPI.Models;
using System.Text.Json.Serialization;

public enum MaintenanceStatus
{
    Pending,    // En attente
    InProgress, // En cours de traitement
    Completed,  // Réparation terminée
    Cancelled   // Demande annulée
}

public enum MaintenancePriority
{
    Low,
    Medium,
    High,
    Critical
}

/// <summary>
/// Représente une demande de maintenance ou de réparation pour un équipement.
/// </summary>
public class MaintenanceRequest
{
    /// <summary>Identifiant unique de la demande de maintenance.</summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>Identifiant de l'équipement concerné.</summary>
    [JsonPropertyName("equipment_id")]
    public Guid EquipmentId { get; set; }

    /// <summary>Équipement concerné par la maintenance.</summary>
    [JsonPropertyName("equipment")]
    public Equipment? Equipment { get; set; }

    /// <summary>Identifiant de l'utilisateur ayant soumis la demande.</summary>
    [JsonPropertyName("requested_by_id")]
    public Guid RequestedById { get; set; }

    /// <summary>Statut actuel de la demande (En attente, En cours, Terminé, Annulé).</summary>
    [JsonPropertyName("status")]
    public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Pending;

    /// <summary>Niveau de priorité de la demande.</summary>
    [JsonPropertyName("priority")]
    public MaintenancePriority Priority { get; set; } = MaintenancePriority.Medium;

    /// <summary>Description détaillée du problème ou de la maintenance requise.</summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>Date prévue pour l'intervention technique.</summary>
    [JsonPropertyName("scheduled_date")]
    public DateTime? ScheduledDate { get; set; }

    /// <summary>Date effective de fin de maintenance.</summary>
    [JsonPropertyName("completed_date")]
    public DateTime? CompletedDate { get; set; }

    /// <summary>Notes techniques ajoutées par le technicien après intervention.</summary>
    [JsonPropertyName("technician_notes")]
    public string? TechnicianNotes { get; set; }

    /// <summary>Date de création de la demande.</summary>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
