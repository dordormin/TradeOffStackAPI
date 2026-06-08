namespace TradeOffStackAPI.Tests.Integration.Ordering;

/// <summary>
/// Donne un ordre d'exécution à une méthode de test (priorité croissante).
/// Permet d'exécuter les POST (création) AVANT les GET/PUT/DELETE.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class TestPriorityAttribute(int priority) : Attribute
{
    public int Priority { get; } = priority;
}
