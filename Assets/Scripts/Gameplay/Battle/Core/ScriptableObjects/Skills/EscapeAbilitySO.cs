using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Map;
using CryptoQuest.System.SceneManagement;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills
{
    [CreateAssetMenu(fileName = "Escape Ability", menuName = "Gameplay/Battle/Abilities/Escape Ability")]
    public class EscapeAbilitySO : SpecialAbilitySO
    {
        [SerializeField] private EscapeRouteMappingSO _escapeRouteMappingSO;
        [SerializeField] private SceneLoaderBus _sceneLoaderBus;
        public UnityAction<MapPathSO> EscapeSucceeded;
        public UnityAction EscapeFailed;

        protected override AbstractAbility CreateAbility()
            => new EscapeAbility(_escapeRouteMappingSO, _sceneLoaderBus);
    }

    public class EscapeAbility : SpecialAbility
    {
        protected new EscapeAbilitySO AbilitySO => (EscapeAbilitySO)_abilitySO;

        private SceneLoaderBus _sceneLoaderBus;
        private EscapeRouteMappingSO _escapeRouteMappingSO;

        public EscapeAbility(EscapeRouteMappingSO escapeRouteMappingSo, SceneLoaderBus sceneLoaderBus)
        {
            _sceneLoaderBus = sceneLoaderBus;
            _escapeRouteMappingSO = escapeRouteMappingSo;
        }

        public override IEnumerator AbilityActivated()
        {
            HandleEscape();
            yield return base.AbilityActivated();
        }

        private void HandleEscape()
        {
            SceneScriptableObject currentActiveMapScene = _sceneLoaderBus.SceneLoader.CurrentLoadedScene;
            Dictionary<SceneScriptableObject, MapPathSO> mapToEscapeRouteDict =
                _escapeRouteMappingSO.MapToEscapePathDictionary;
            if (mapToEscapeRouteDict.TryGetValue(currentActiveMapScene, out var escapeRoute))
            {
                OnEscaping(escapeRoute);
                return;
            }

            AbilitySO.EscapeFailed?.Invoke();
        }

        private void OnEscaping(MapPathSO escapeRoute)
        {
            AbilitySO.EscapeSucceeded?.Invoke(escapeRoute);
        }
    }
}