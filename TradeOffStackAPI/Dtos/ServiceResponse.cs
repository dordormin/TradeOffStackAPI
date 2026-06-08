using System.Text.Json.Serialization;

namespace TradeOffStackAPI.Dtos;

/// <summary>
/// Classe de base pour les réponses de service.
/// </summary>
public class ServiceResponse
{
    [JsonIgnore]
    public bool Success { get; protected set; } = true;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }

    // Méthodes Factory pour la version non-générique
    public static ServiceResponse Ok(string? message = null)
    {
        return new ServiceResponse { Message = message };
    }

    public static ServiceResponse Fail(string message)
    {
        return new ServiceResponse { Success = false, Message = message };
    }
}

/// <summary>
/// Enveloppe standard générique qui hérite de la classe de base et ajoute les données.
/// </summary>
/// <typeparam name="T">Le type de données retourné en cas de succès.</typeparam>
public class ServiceResponse<T> : ServiceResponse
{
    public T? Data { get; set; }

    // Méthodes Factory pour la version générique
    public static ServiceResponse<T> Ok(T data, string? message = null)
    {
        return new ServiceResponse<T> { Data = data, Message = message };
    }

    // On peut "cacher" la méthode de base pour qu'elle retourne le bon type
    public new static ServiceResponse<T> Fail(string message)
    {
        return new ServiceResponse<T> { Success = false, Message = message };
    }
}