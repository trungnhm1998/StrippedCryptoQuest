using CryptoQuest.Battle.ScriptableObjects;
using CryptoQuest.Map;
using IndiGames.Core.Common;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay
{
    public class ScenarioChecker : MonoBehaviour, IScenarioChecker
    {
        [SerializeField] private SceneScriptableObject _worldMapScene;
        [SerializeField] private SceneManagerSO _sceneManagerSO;

        private void Awake()
        {
            ServiceProvider.Provide<IScenarioChecker>(this);
        }

        public bool IsCorrectScenario(EAbilityUsageScenario scenario)
        {
            bool isAllowedInField = scenario.HasFlag(EAbilityUsageScenario.Field);
            bool isWorldMap = _sceneManagerSO.CurrentScene == _worldMapScene;
            if (!isWorldMap) return isAllowedInField;

            return scenario.HasFlag(EAbilityUsageScenario.World);
        }
    }
}