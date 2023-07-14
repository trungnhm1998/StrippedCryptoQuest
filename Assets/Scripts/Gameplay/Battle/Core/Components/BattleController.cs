using UnityEngine;
using CryptoQuest.UI.Battle;
using IndiGames.Core.Events.ScriptableObjects;
using CryptoQuest.Input;

namespace CryptoQuest.Gameplay.Battle
{
    public class BattleController : MonoBehaviour
    {
        [SerializeField] private BattleBus _battleBus;
        [SerializeField] private InputMediatorSO _inputMediator;

        [Header("UI")]
        [SerializeField] private CharacterList _heroesUI;
        [SerializeField] private CharacterList _monstersUI;
        [SerializeField] private GameObject _batteMenu;

        [Header("Listen Events")]
        [SerializeField] private VoidEventChannelSO _newTurnEventChannel;
        [SerializeField] private VoidEventChannelSO _battleStartChannelEvent;
        
        private void OnEnable()
        {
            _newTurnEventChannel.EventRaised += OnNewTurn;
            _battleStartChannelEvent.EventRaised += OnBattleStart;
        }

        private void OnDisable()
        {
            _newTurnEventChannel.EventRaised -= OnNewTurn;
            _battleStartChannelEvent.EventRaised -= OnBattleStart;
            _inputMediator.EnableMapGameplayInput();
        }

        private void OnNewTurn()
        {
            _batteMenu.SetActive(true);
        }

        private void OnBattleStart()
        {
            _heroesUI.InitUI(_battleBus.BattleManager.BattleTeam1.BattleUnits);
            _monstersUI.InitUI(_battleBus.BattleManager.BattleTeam2.BattleUnits);
            _inputMediator.EnableMenuInput();
        }
    }   
}