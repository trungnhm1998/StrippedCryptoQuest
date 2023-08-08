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
        [SerializeField] private CharacterList _heroesUI;
        [SerializeField] private CharacterList _monstersUI;
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
            _heroesUI.gameObject.SetActive(true);
        }

        private void SetupBattleUIs()
        {
            _heroesUI.InitUI(_battleBus.BattleManager.BattleTeam1.BattleUnits);
            _monstersUI.InitUI(_battleBus.BattleManager.BattleTeam2.BattleUnits);
            _heroesUI.gameObject.SetActive(false);
            _batteMenu.SetActive(false);
        }

        private void OnStategyPhaseEnd()
        {
            _batteMenu.SetActive(false);
        }
    }   
}