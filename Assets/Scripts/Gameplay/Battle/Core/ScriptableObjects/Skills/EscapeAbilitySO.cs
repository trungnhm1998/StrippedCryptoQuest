using System.Collections;
using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Map;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills
{
    [CreateAssetMenu (fileName = "Escape Ability", menuName = "Gameplay/Battle/Abilities/Escape Ability")]
    public class EscapeAbilitySO : SpecialAbilitySO
    {
        public PathStorageSO PathStorage;
        public ActiveSceneSO ActiveSceneSO;
        public SceneScriptableObject EscapeDestinationSceneSO;
        public EscapeRouteMappingSO EscapeRouteMappingSO;
        public LoadSceneEventChannelSO LoadSceneEventChannelSO;

        protected override AbstractAbility CreateAbility() => new EscapeAbility();
    }

    public class EscapeAbility : SpecialAbility
    {
        protected new EscapeAbilitySO AbilitySO => (EscapeAbilitySO)_abilitySO;

        public override IEnumerator AbilityActivated()
        {
            HandleEscape();
            yield return base.AbilityActivated();
        }


        private void HandleEscape()
        {
            SceneScriptableObject currentActiveMapScene = AbilitySO.ActiveSceneSO.CurrentActiveMapScene;
            foreach (var escapeRoute in AbilitySO.EscapeRouteMappingSO.EscapeRouteMappings)
            {
                if (escapeRoute.EscapableMaps.Contains(currentActiveMapScene))
                {
                    TriggerEscape(escapeRoute.EscapeRoute);
                    return;
                }
            }

            Debug.Log("Map is unescapable");
        }

        private void TriggerEscape(MapPathSO escapeRoute)
        {
            if (SceneManager.GetSceneByName("WorldMap").isLoaded)
                return;
            AbilitySO.PathStorage.LastTakenPath = escapeRoute;
            AbilitySO.LoadSceneEventChannelSO.RequestLoad(AbilitySO.EscapeDestinationSceneSO);
        }
    }
}