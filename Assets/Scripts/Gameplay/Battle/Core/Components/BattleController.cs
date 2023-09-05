using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.Input;
using CryptoQuest.UI.Battle;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components
{
    public class BattleController : MonoBehaviour
    {
        [SerializeField] private BattleInputSO _battleInput;
        [SerializeField] private BattleBus _battleBus;

        [Header("UI")]
        [SerializeField] private GameObject _batteMenu;

        [Header("Listen Events")]
        [SerializeField] private VoidEventChannelSO _newTurnEventChannel;
        [SerializeField] private VoidEventChannelSO _doneSetupBattleEventChannel;
        [SerializeField] private VoidEventChannelSO _endStrategyPhaseEventChannel;
        
        private void OnEnable()
        {
            _newTurnEventChannel.EventRaised += OnNewTurn;
            _doneSetupBattleEventChannel.EventRaised += SetupBattleUIs;
            _endStrategyPhaseEventChannel.EventRaised += OnStategyPhaseEnd;
        }

        private void OnDisable()
        {
            _newTurnEventChannel.EventRaised -= OnNewTurn;
            _doneSetupBattleEventChannel.EventRaised -= SetupBattleUIs;
            _endStrategyPhaseEventChannel.EventRaised -= OnStategyPhaseEnd;
            _battleInput.DisableBattleInput();
        }

        private void OnNewTurn()
        {
            _batteMenu.SetActive(true);
        }

        private void SetupBattleUIs()
        {
            _batteMenu.SetActive(false);
        }

        private void OnStategyPhaseEnd()
        {
            _batteMenu.SetActive(false);
        }
    }   
}