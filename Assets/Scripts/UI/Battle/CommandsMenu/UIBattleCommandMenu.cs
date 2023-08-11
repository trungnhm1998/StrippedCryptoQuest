using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle.CommandsMenu
{
    public class UIBattleCommandMenu : MonoBehaviour
    {
        [SerializeField] private BattlePanelController _battlePanelController;
        [SerializeField] private ChildButtonsActivator _childButonsActivator;

        [Header("Events")]
        [SerializeField] private BattleUnitEventChannelSO _heroTurnEventChannel;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _currentUnitName;

        [SerializeField] private Button _firstButton;

        private IBattleUnit _currentUnit;

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
            SetActiveCommandsMenu(true);
        }

        private void OnHeroTurn(IBattleUnit unit)
        {
            _currentUnit = unit;
            _currentUnitName.text = unit.UnitInfo.DisplayName;
        }

        private void SelectFirstButton()
        {
            _firstButton.Select();
        }

        public void OnNormalAttack()
        {
            _battlePanelController.OnButtonAttackClicked.Invoke(_currentUnit);
        }

        public void OnUseSkill()
        {
            _battlePanelController.OnButtonSkillClicked.Invoke(_currentUnit);
        }

        public void OnUseItem()
        {
            _battlePanelController.OnButtonItemClicked.Invoke(_currentUnit);
        }

        public void OnGuard()
        {
            _battlePanelController.OnButtonGuardClicked.Invoke();
        }

        public void OnEscape()
        {
            _battlePanelController.OnButtonEscapeClicked.Invoke(_currentUnit);
        }

        public void SetActiveCommandsMenu(bool isActive)
        {
            _childButonsActivator.SetActiveButtons(isActive);
        }
    }
}