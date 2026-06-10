namespace TradeOffStackAPI.Models;
using System.Text.Json.Serialization;

public enum UserRole
{
    Admin,
    Manager,
    Employee,
    Tester
}

/// <summary>
/// Représente un utilisateur (employé, gestionnaire ou administrateur) du système.
/// </summary>
public class User
{
    /// <summary>Identifiant unique de l'utilisateur.</summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>Prénom de l'utilisateur.</summary>
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Nom de famille de l'utilisateur.</summary>
    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>Adresse email professionnelle (identifiant de connexion).</summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>Numéro de téléphone de contact.</summary>
    [JsonPropertyName("phone_number")]
    public string? PhoneNumber { get; set; }
    
    /// <summary>Nom du fichier image de profil (ex: 'mon_avatar.jpg').</summary>
    [JsonPropertyName("profile_image")]
    public string? ProfileImage { get; set; }

    /// <summary>URL complète de l'image de profil (construite par le serveur).</summary>
    [JsonPropertyName("profile_image_url")]
    public string? ProfileImageUrl { get; set; }

    /// <summary>Rôle et niveau d'accès (Admin, Manager, Employee).</summary>
    [JsonPropertyName("role")]
    public UserRole Role { get; set; } = UserRole.Employee;

    // Hash BCrypt du mot de passe. JsonIgnore : ne doit JAMAIS sortir de l'API.
    /// <summary>Empreinte sécurisée du mot de passe (interne).</summary>
    [JsonIgnore]
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>Identifiant du département de rattachement.</summary>
    [JsonPropertyName("department_id")]
    public Guid? DepartmentId { get; set; }

    /// <summary>Département de rattachement.</summary>
    [JsonPropertyName("department")]
    public Department? Department { get; set; }

    /// <summary>Indique si le compte est actif.</summary>
    [JsonPropertyName("is_active")]
    public bool IsActive { get; set; } = true;

    /// <summary>Date de création du compte.</summary>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Liste des réservations effectuées par cet utilisateur.</summary>
    [JsonIgnore]
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    /// <summary>Liste des demandes de maintenance soumises par cet utilisateur.</summary>
    [JsonIgnore]
    public ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();
}