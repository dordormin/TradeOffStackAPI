namespace TradeOffStackAPI.Models;
using System.Text.Json.Serialization;

public enum AuditAction
{
    Created,
    Updated,
    Deleted
}

/// <summary>
/// Journalise une modification effectuée sur une entité du système.
/// </summary>
public class AuditLog
{
    /// <summary>Identifiant unique de l'entrée d'audit.</summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>Type de l'entité concernée (Equipment, User, etc.).</summary>
    [JsonPropertyName("entity_type")]
    public string EntityType { get; set; } = string.Empty;

    /// <summary>Identifiant unique de l'entité concernée.</summary>
    [JsonPropertyName("entity_id")]
    public Guid EntityId { get; set; }

    /// <summary>Type d'action effectuée (Création, Mise à jour, Suppression).</summary>
    [JsonPropertyName("action")]
    public AuditAction Action { get; set; }

    /// <summary>Valeurs avant la modification (format JSON).</summary>
    [JsonPropertyName("old_values")]
    public string? OldValues { get; set; }

    /// <summary>Valeurs après la modification (format JSON).</summary>
    [JsonPropertyName("new_values")]
    public string? NewValues { get; set; }

    /// <summary>Identifiant de l'utilisateur ayant effectué l'action.</summary>
    [JsonPropertyName("performed_by_id")]
    public Guid? PerformedById { get; set; }

    /// <summary>Date et heure précise de l'action.</summary>
    [JsonPropertyName("performed_at")]
    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;
}
