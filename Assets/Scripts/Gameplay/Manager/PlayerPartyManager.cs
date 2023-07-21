using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CryptoQuest.Gameplay.Battle.Core.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.Gameplay.Manager
{
    /// <summary>
    /// Process player party information 
    /// </summary>
    public class PlayerPartyManager : MonoBehaviour
    {
        [SerializeField] private GameplayBus _gameplayBus;
        [SerializeField] private BattleTeam _playerTeam;
        [SerializeField] private AbilitySystemBehaviour _mainSystem;

        private void Awake()
        {
            _gameplayBus.PlayerTeam = _playerTeam;
            _gameplayBus.MainSystem = _mainSystem;
        }
    }
}
