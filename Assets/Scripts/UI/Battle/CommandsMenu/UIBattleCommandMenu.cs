using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using TMPro;

namespace CryptoQuest.UI.Battle
{
    public class UIBattleCommandMenu : MonoBehaviour
    {
        [SerializeField] private BattlePanelController _battlePanelController;

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

        private IBattleUnit _currentUnit;
        private readonly Dictionary<Button, TextMeshProUGUI> _buttonTextMap = new();


        private void OnEnable()
        {
            _heroTurnEventChannel.EventRaised += OnHeroTurn;
        }

        private void OnDisable()
        {
            _heroTurnEventChannel.EventRaised -= OnHeroTurn;
        }

        public void Initialize()
        {
            SelectFirstButton();
            CacheButtonTexts();
        }

        private void OnHeroTurn(IBattleUnit unit)
        {
            Initialize();
            _currentUnit = unit;
            _currentUnitName.text = unit.UnitData.DisplayName;
        }

        private void CacheButtonTexts()
        {
            SetActiveCommandsMenu(true);
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
            _battlePanelController.OnButtonAttackClicked.Invoke(_currentUnit);
            SetActiveCommandsMenu(false);
        }

        public void OnUseSkill()
        {
            // TODO: Implement use Skill flow here
            _battlePanelController.OnButtonSkillClicked.Invoke(_currentUnit);
            SetActiveCommandsMenu(false);
        }

        public void OnUseItem()
        {
            // TODO: Implement use item flow here
            _battlePanelController.OnButtonItemClicked.Invoke(_currentUnit);
            SetActiveCommandsMenu(false);
        }

        public void OnGuard()
        {
            // TODO: Implement guard flow here
            _battlePanelController.OnButtonGuardClicked.Invoke();
            SetActiveCommandsMenu(false);
        }

        public void OnEscape()
        {
            _battlePanelController.OnButtonEscapeClicked.Invoke();
            SetActiveCommandsMenu(false);
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