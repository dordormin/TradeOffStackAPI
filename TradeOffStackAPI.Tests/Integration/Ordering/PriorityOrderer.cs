using Xunit.Abstractions;
using Xunit.Sdk;

namespace TradeOffStackAPI.Tests.Integration.Ordering;

/// <summary>
/// Ordonne les cas de test d'une classe selon [TestPriority]. Sans attribut → priorité 0.
/// Départage par nom de méthode pour un ordre déterministe.
/// </summary>
public sealed class PriorityOrderer : ITestCaseOrderer
{
    public const string TypeName = "TradeOffStackAPI.Tests.Integration.Ordering.PriorityOrderer";
    public const string AssemblyName = "TradeOffStackAPI.Tests";

    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
        where TTestCase : ITestCase
    {
        return testCases
            .Select(tc => (TestCase: tc, Priority: GetPriority(tc)))
            .OrderBy(x => x.Priority)
            .ThenBy(x => x.TestCase.TestMethod.Method.Name, StringComparer.Ordinal)
            .Select(x => x.TestCase);
    }

    private static int GetPriority(ITestCase testCase)
    {
        var attribute = testCase.TestMethod.Method
            .GetCustomAttributes(typeof(TestPriorityAttribute).AssemblyQualifiedName)
            .FirstOrDefault();

        // priority est un argument de constructeur positionnel.
        return attribute?.GetConstructorArguments().FirstOrDefault() is int p ? p : 0;
    }
}
