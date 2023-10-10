using System;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.Input;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components
{
    [Obsolete]
    public class BattleController : MonoBehaviour
    {
        [SerializeField] private BattleBus _battleBus;
        [SerializeField] private BattleInput _battleInput;

        [Header("Listen Events")]
        [SerializeField] private VoidEventChannelSO _newTurnEventChannel;
        [SerializeField] private VoidEventChannelSO _doneSetupBattleEventChannel;
        [SerializeField] private VoidEventChannelSO _endStrategyPhaseEventChannel;
        
        private void OnDisable()
        {
            _battleInput.DisableBattleInput();
        }
    }   
}