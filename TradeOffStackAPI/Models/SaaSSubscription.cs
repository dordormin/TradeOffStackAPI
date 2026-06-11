namespace TradeOffStackAPI.Models;
using System.Text.Json.Serialization;

/// <summary>
/// Abonnement à un service SaaS, géré par l'entreprise.
/// Remplace/étend le concept de SoftwareLicense.
/// </summary>
public class SaaSSubscription
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("provider_id")]
    public Guid ProviderId { get; set; }
    
    public SaaSProvider? Provider { get; set; }

    [JsonPropertyName("plan_name")]
    public string PlanName { get; set; } = string.Empty;

    [JsonPropertyName("billing_cycle")]
    public string BillingCycle { get; set; } = "Monthly"; // Monthly, Yearly

    [JsonPropertyName("cost_per_seat")]
    public decimal CostPerSeat { get; set; }

    [JsonPropertyName("total_seats")]
    public int TotalSeats { get; set; }

    [JsonPropertyName("renewal_date")]
    public DateTime? RenewalDate { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = "Active"; // Active, Cancelled, Expired

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public ICollection<SaaSAssignment> Assignments { get; set; } = new List<SaaSAssignment>();
}
