using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using IndiGames.Core.Events.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class BattleLoader : MonoBehaviour
    {
        [SerializeField] private TriggerBattleEncounterEventSO _triggerBattleEncounterEventSo;
        [SerializeField] private LoadSceneEventChannelSO _loadSceneEventChannelSo;
        [SerializeField] private VoidEventChannelSO _onBattleEndEventChannel;
        [SerializeField] private UnloadSceneEventChannelSO _unloadSceneEvent;
        [SerializeField] private BattleBus _battleBus;
        [SerializeField] private SceneScriptableObject _battleSceneSO;

        private void OnEnable()
        {
            _triggerBattleEncounterEventSo.EncounterBattle += OnEncounterBattle;
            _onBattleEndEventChannel.EventRaised += OnBattleEnd;
        }

        private void OnDisable()
        {
            _triggerBattleEncounterEventSo.EncounterBattle -= OnEncounterBattle;
            _onBattleEndEventChannel.EventRaised -= OnBattleEnd;
        }

        private void OnEncounterBattle(BattleInfo battleInfo)
        {
            _battleBus.CurrentBattleInfo = battleInfo;
            _loadSceneEventChannelSo.RequestLoad(_battleSceneSO);
        }

        private void OnBattleEnd()
        {
            _unloadSceneEvent.RequestUnload(_battleSceneSO);
        }
    }
}