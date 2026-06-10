namespace TradeOffStackAPI.Services;
using TradeOffStackAPI.Models;
using TradeOffStackAPI.Services.Interfaces;

public class DepreciationService : IDepreciationService
{
    public decimal CalculateCurrentBookValue(Equipment equipment)
    {
        if (equipment.PurchaseDate == null || equipment.Price <= 0 || equipment.UsefulLifeYears <= 0)
        {
            return equipment.Price;
        }

        var ageInYears = (DateTime.UtcNow - equipment.PurchaseDate.Value).TotalDays / 365.25;
        if (ageInYears <= 0) return equipment.Price;

        var bookValue = equipment.Price;

        if (equipment.DepreciationMethod == DepreciationMethod.StraightLine)
        {
            // Amortissement Linéaire
            var depreciableBase = equipment.Price - equipment.SalvageValue;
            var annualDepreciation = depreciableBase / equipment.UsefulLifeYears;
            var totalDepreciation = annualDepreciation * (decimal)ageInYears;
            
            bookValue = equipment.Price - totalDepreciation;
        }
        else if (equipment.DepreciationMethod == DepreciationMethod.DecliningBalance)
        {
            // Amortissement Dégressif (Simplified: Double Declining Balance)
            var rate = 2.0m / equipment.UsefulLifeYears;
            bookValue = equipment.Price * (decimal)Math.Pow((double)(1 - rate), ageInYears);
        }

        // On ne descend jamais en dessous de la valeur de récupération
        return Math.Max(bookValue, equipment.SalvageValue);
    }
}
