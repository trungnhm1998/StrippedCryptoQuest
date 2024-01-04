using CryptoQuest.Battle.ScriptableObjects;
using UnityEngine.SceneManagement;

namespace CryptoQuest.Gameplay
{
    public static class ScenarioProvider
    {
        public static bool IsCorrectScenario(EAbilityUsageScenario scenario)
        {
            bool isAllowedInField = scenario.HasFlag(EAbilityUsageScenario.Field);
            if (!SceneManager.GetSceneByName("WorldMap").isLoaded) return isAllowedInField;

            return scenario.HasFlag(EAbilityUsageScenario.World);
        }
    }
}