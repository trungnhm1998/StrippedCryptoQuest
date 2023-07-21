using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using TMPro;

namespace CryptoQuest.UI.Battle
{
    public class UIBattleActionMenu : MonoBehaviour
    {
        [SerializeField] private BattleBus _battleBus;
        [SerializeField] private BattleActionHandler.BattleActionHandler[] _normalAttackChain;

        [Header("Events")]
        [SerializeField] private BattleUnitEventChannelSO _heroTurnEventChannel;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _currentUnitName;

        [SerializeField] private Button _firstButton;

        [SerializeField] private Button[] _allButtons;
        [SerializeField] private TextMeshProUGUI[] _allButtonTexts;

        [Header("Config Button Text Colors")]
        [SerializeField] private Color _activeColor;

        [SerializeField] private Color _inactiveColor;

        private BattleManager _battleManager;
        private IBattleUnit _currentUnit;
        private Dictionary<Button, TextMeshProUGUI> _buttonTextMap = new Dictionary<Button, TextMeshProUGUI>();

        private void Start()
        {
            _battleManager = _battleBus.BattleManager;
            SetupChain(_normalAttackChain);
            CacheButtonTexts();
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
            SetActiveCommandsMenu(true);
            _currentUnit = unit;
            _currentUnitName.text = unit.UnitData.DisplayName;
        }

        private void SetupChain(BattleActionHandler.BattleActionHandler[] chain)
        {
            for (int i = 1; i < chain.Length; i++)
            {
                chain[i - 1].SetNext(chain[i]);
            }
        }

        private void CacheButtonTexts()
        {
            _buttonTextMap.Clear();
            
            for (int i = 0; i < _allButtons.Length; i++)
            {
                _buttonTextMap.Add(_allButtons[i], _allButtonTexts[i]);
            }
        }

        private void SelectFirstButton()
        {
            _firstButton.Select();
        }

        public void OnNormalAttack()
        {
            _normalAttackChain[0].Handle(_currentUnit);
            SetActiveCommandsMenu(false);
        }

        public void OnUseSkill()
        {
            // TODO: Implement use Skill flow here
            SetActiveCommandsMenu(false);
        }

        public void OnUseItem()
        {
            // TODO: Implement use item flow here
            SetActiveCommandsMenu(false);
        }

        public void OnGuard()
        {
            // TODO: Implement guard flow here
            SetActiveCommandsMenu(false);
        }

        public void OnEscape()
        {
            SetActiveCommandsMenu(false);
            _battleManager.OnEscape();
        }

        private void SetActiveCommandsMenu(bool isActive)
        {
            foreach (var pair in _buttonTextMap)
            {
                var button = pair.Key;
                var buttonText = pair.Value;

                button.interactable = isActive;
                buttonText.color = isActive ? _activeColor : _inactiveColor;
            }
        }
    }
}