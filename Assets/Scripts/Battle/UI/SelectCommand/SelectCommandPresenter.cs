using System;
using System.Linq;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.Helper;
using CryptoQuest.System;
using CryptoQuest.UI.Common;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Battle.UI.SelectCommand
{
    public class SelectCommandPresenter : MonoBehaviour
    {
        [SerializeField] private UISelectCommand _uiSelectCommand;
        [SerializeField] private Button _skillButton;
        [SerializeField] private Button _itemButton;

        private UIGrayoutTextsBehaviour _skillGrayBehaviour;
        private UIGrayoutTextsBehaviour _itemGrayBehaviour;
        private IInventoryController _inventoryController;

        private void Awake()
        {
            if (_skillGrayBehaviour == null)
            {
                _skillGrayBehaviour = _skillButton.GetComponent<UIGrayoutTextsBehaviour>();
            }

            if (_itemGrayBehaviour == null)
            {
                _itemGrayBehaviour = _itemButton.GetComponent<UIGrayoutTextsBehaviour>();
            }
        }

        /// <summary>
        /// Disable button if hero have no skill or there's no item to select
        /// </summary>
        /// <param name="hero"></param>
        public void CheckActiveButtons(HeroBehaviour hero)
        {
            SetSkillButtonActive(hero);
            SetItemButtonActive();
        }

        private void SetSkillButtonActive(HeroBehaviour hero)
        {
            if (!hero.TryGetComponent(out HeroSkills heroSkills))
                return;

            var validBattleSkills = heroSkills.Skills.Select(s => IsBattleSkill(s));

            var isButtonSelectable = validBattleSkills.Count() > 0;

            _skillGrayBehaviour.SetGrayoutTexts(isButtonSelectable);
            _skillButton.onClick = new Button.ButtonClickedEvent();
            if (isButtonSelectable)
                _skillButton.onClick.AddListener(_uiSelectCommand.OnSkillPressed);
        }

        private void SetItemButtonActive()
        {
            _inventoryController ??= ServiceProvider.GetService<IInventoryController>();
            var inventory = _inventoryController.Inventory;

            var isButtonSelectable = inventory.GetItemsInBattle().Count() > 0;
            _itemGrayBehaviour.SetGrayoutTexts(isButtonSelectable);
            _itemButton.onClick = new Button.ButtonClickedEvent();
            if (isButtonSelectable)
                _itemButton.onClick.AddListener(_uiSelectCommand.OnItemPressed);
        }

        private bool IsBattleSkill(CastSkillAbility skill)
            => skill.SkillInfo.UsageScenarioSO.HasFlag(EAbilityUsageScenario.Battle);
    }
}