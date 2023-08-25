using CryptoQuest.Gameplay.Battle.Core.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    /// <summary>
    /// Process player party information 
    /// </summary>
    public class PlayerPartyManager : MonoBehaviour
    {
        [SerializeField] private PartySO _party;
        [SerializeField] private BattleTeam _playerTeam;
        [SerializeField] private AbilitySystemBehaviour _mainSystem;

        private void Awake()
        {
            _party.PlayerTeam = _playerTeam;
            _party.MainSystem = _mainSystem;
        }
    }
}