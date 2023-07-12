using UnityEngine;
using CryptoQuest.UI.Battle;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.Gameplay.Battle
{
    public class BattleController : MonoBehaviour
    {
        [SerializeField] private BattleBus _battleBus;

        [Header("UI")]
        [SerializeField] private CharacterList _heroesUI;
        [SerializeField] private CharacterList _monstersUI;
        [SerializeField] private GameObject _batteMenu;

        [Header("Listen Events")]
        [SerializeField] private VoidEventChannelSO _newTurnEventChannel;
        
        private void Start()
        {
            _heroesUI.InitUI(_battleBus.BattleManager.BattleTeam1.BattleUnits);
            _monstersUI.InitUI(_battleBus.BattleManager.BattleTeam2.BattleUnits);
        }

        private void OnEnable()
        {
            _newTurnEventChannel.EventRaised += OnNewTurn;
        }

        private void OnDisable()
        {
            _newTurnEventChannel.EventRaised -= OnNewTurn;
        }

        private void OnNewTurn()
        {
            _batteMenu.SetActive(true);
        }
    }   
}