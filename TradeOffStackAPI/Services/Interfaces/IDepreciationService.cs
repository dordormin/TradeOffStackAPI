namespace TradeOffStackAPI.Services.Interfaces;
using TradeOffStackAPI.Models;

public interface IDepreciationService
{
    /// <summary>
    /// Calcule la valeur comptable actuelle (valeur nette) d'un équipement.
    /// </summary>
    decimal CalculateCurrentBookValue(Equipment equipment);
}
