namespace TradeOffStackAPI.Models;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Assignation d'un siège/licence SaaS à un utilisateur (employé).
/// Permet de traquer l'utilisation et le Shadow IT.
/// </summary>
public class SaaSAssignment
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("subscription_id")]
    public Guid SubscriptionId { get; set; }
    
    public SaaSSubscription? Subscription { get; set; }

    [JsonPropertyName("user_id")]
    public Guid UserId { get; set; }

    [NotMapped]
    [JsonPropertyName("user")]
    public User? User { get; set; }

    [JsonPropertyName("assigned_date")]
    public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("last_login_date")]
    public DateTime? LastLoginDate { get; set; }

    [JsonPropertyName("is_active")]
    public bool IsActive { get; set; } = true;
}
