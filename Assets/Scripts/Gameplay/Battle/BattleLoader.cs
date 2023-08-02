using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class BattleLoader : MonoBehaviour
    {
        [SerializeField] private TriggerBattleEncounterEventSO _triggerBattleEncounterEventSo;
        [SerializeField] private LoadSceneEventChannelSO _loadSceneEventChannelSo;
        [SerializeField] private BattleBus _battleBus;

        private void OnEnable()
        {
            _triggerBattleEncounterEventSo.EncounterBattle += OnEncounterBattle;
        }

        private void OnDisable()
        {
            _triggerBattleEncounterEventSo.EncounterBattle -= OnEncounterBattle;
        }

        private void OnEncounterBattle(BattleInfo battleInfo)
        {
            _battleBus.CurrentBattleInfo = battleInfo;
            _loadSceneEventChannelSo.RequestLoad(battleInfo.BattleSceneSO);
        }
    }
}