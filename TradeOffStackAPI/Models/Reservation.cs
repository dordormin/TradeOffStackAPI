namespace TradeOffStackAPI.Models;
using System.Text.Json.Serialization;

public enum ReservationStatus
{
    Pending,   // En attente d'approbation
    Active,    // Équipement en possession de l'utilisateur
    Returned,  // Équipement retourné
    Cancelled  // Réservation annulée
}

/// <summary>
/// Représente une réservation d'équipement effectuée par un utilisateur.
/// </summary>
public class Reservation
{
    /// <summary>Identifiant unique de la réservation.</summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>Identifiant de l'équipement réservé.</summary>
    [JsonPropertyName("equipment_id")]
    public Guid EquipmentId { get; set; }

    /// <summary>Équipement réservé.</summary>
    [JsonPropertyName("equipment")]
    public Equipment? Equipment { get; set; }

    /// <summary>Identifiant de l'utilisateur ayant effectué la réservation.</summary>
    [JsonPropertyName("user_id")]
    public Guid UserId { get; set; }

    /// <summary>Utilisateur ayant effectué la réservation.</summary>
    [JsonPropertyName("user")]
    public User? User { get; set; }

    /// <summary>Statut actuel de la réservation (En attente, Active, Retourné, Annulé).</summary>
    [JsonPropertyName("status")]
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

    /// <summary>Date et heure de début prévue de la réservation.</summary>
    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }

    /// <summary>Date et heure de fin prévue de la réservation.</summary>
    [JsonPropertyName("end_date")]
    public DateTime? EndDate { get; set; }

    /// <summary>Date et heure effective de retour de l'équipement.</summary>
    [JsonPropertyName("return_date")]
    public DateTime? ReturnDate { get; set; }

    /// <summary>Commentaires ou notes supplémentaires.</summary>
    [JsonPropertyName("notes")]
    public string? Notes { get; set; }

    /// <summary>Date de création de la demande de réservation.</summary>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
