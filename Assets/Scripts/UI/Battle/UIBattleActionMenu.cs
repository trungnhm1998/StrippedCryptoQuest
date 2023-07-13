using UnityEngine;
using UnityEngine.UI;
using CryptoQuest.Gameplay.Battle;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.Core.Events.ScriptableObjects;
using CryptoQuest.GameHandler;

namespace CryptoQuest.UI.Battle
{
    public class UIBattleActionMenu : MonoBehaviour
    {
        [SerializeField] private BattleBus _battleBus;
        [SerializeField] private BattleActionHandler[] _normalAttackChain;

        [Header("Events")]
        [SerializeField] private BattleUnitEventChannelSO _heroTurnEventChannel;

        [Header("UI")]
        [SerializeField] private Text _currentUnitName;
        [SerializeField] private Button _firstButton;

        private BattleManager _battleManager;
        private IBattleUnit _currentUnit;

        private void Start()
        {
            _battleManager = _battleBus.BattleManager;
            SetupChain(_normalAttackChain);
        }

        private void OnEnable()
        {
            _heroTurnEventChannel.EventRaised += OnHeroTurn;
        }

        private void OnDisable()
        {
            _heroTurnEventChannel.EventRaised -= OnHeroTurn;
        }

        private void OnHeroTurn(IBattleUnit unit)
        {
            SelectFirstButton();
            _currentUnit = unit;
            _currentUnitName.text = unit.UnitData.DisplayName;
        }

        private void SetupChain(BattleActionHandler[] chain)
        {
            for (int i = 1; i < chain.Length; i++) 
            {
                chain[i - 1].SetNext(chain[i]);
            }
        }

        private void SelectFirstButton()
        {
            _firstButton.Select(); 
        }

        public void OnNormalAttack()
        {
            _normalAttackChain[0].Handle(_currentUnit);
        }

        public void OnUseSkill()
        {
            // TODO: Implement use Skill flow here
        }
        
        public void OnUseItem()
        {
            // TODO: Implement use item flow here
        }
        
        public void OnGuard()
        {
            // TODO: Implement guard flow here
        }
        
        public void OnEscape()
        {
            _battleManager.OnEscape();
        }
    }
}