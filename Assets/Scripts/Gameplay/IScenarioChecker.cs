using CryptoQuest.Battle.ScriptableObjects;

namespace CryptoQuest.Gameplay
{
    public interface IScenarioChecker
    {
        bool IsCorrectScenario(EAbilityUsageScenario scenario);
    }
}