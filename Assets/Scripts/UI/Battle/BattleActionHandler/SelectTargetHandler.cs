using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using UnityEngine;

namespace CryptoQuest.UI.Battle.BattleActionHandler
{
    public class SelectTargetHandler : BattleActionHandler
    {
        [SerializeField] private GameObject _battleMenu;
        [SerializeField] private GameObject _selectTargetUI;
        [SerializeField] private UITargetButton[] _targetButtons;

        private IBattleUnit _currentUnit;

        private void OnValidate()
        {
            _targetButtons = gameObject.GetComponentsInChildren<UITargetButton>(true);
        }

        private void Start()
        {
            foreach (var button in _targetButtons)
            {
                button.gameObject.SetActive(false);
            }
        }

        private void SelectFirstButton()
        {
            if (_targetButtons.Length <= 0) return;
            _targetButtons[0].Button.Select();
        }

        public void OnSelectTarget(IBattleUnit unit)
        {
            _currentUnit.SelectSingleTarget(unit.Owner);
            _selectTargetUI.SetActive(false);
            _battleMenu.SetActive(false);
            base.Handle(unit);
        }

        public override object Handle(IBattleUnit currentUnit)
        {
            _currentUnit = currentUnit;
            if (_currentUnit == null) return currentUnit;
            _selectTargetUI.SetActive(true);
            SetupTargetButton(_currentUnit.OpponentTeam);

            SelectFirstButton();
            return null;
        }

        private void SetupTargetButton(BattleTeam team)
        {
            var targetUnits = team.BattleUnits;
            int targetCount = targetUnits.Count;
            for (int i = 0; i < _targetButtons.Length; i++)
            {
                UITargetButton targetButton = _targetButtons[i];
                var isInTargetRange = i < targetCount;
                targetButton.gameObject.SetActive(isInTargetRange);
                if (!isInTargetRange) continue;
                SetupTargetButton(targetButton, targetUnits[i]);
            }
        }

        private void SetupTargetButton(UITargetButton targetButton, IBattleUnit unit)
        {
            targetButton.Button.onClick.AddListener(() => OnSelectTarget(unit));
            targetButton.Text.text = unit.UnitData.DisplayName;
        }
    }
}