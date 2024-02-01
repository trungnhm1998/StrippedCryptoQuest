using CryptoQuest.BlackSmith.UpgradeStone.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;


namespace CryptoQuest.Menus.Status.UI.MagicStone
{
    public class MagicStoneTooltip : UIUpgradeMagicStoneToolTip
    {
        protected override bool CanShow()
        {
            var selectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (selectedGameObject == null) return false;
            var provider = selectedGameObject.GetComponent<ITooltipStoneProvider>();
            provider ??= selectedGameObject.GetComponentInChildren<ITooltipStoneProvider>();
            if (provider == null) return false;

            if (provider.MagicStone == null || provider.MagicStone.IsValid() == false)
                return false;
            _stone = provider.MagicStone;
            return true;
        }

        public override void SetupInfo()
        {
            CleanUpPool();
            _lvlText.text = $"{_stone.Level}";
            foreach (var passive in _stone.Passives)
            {
                var equipmentUI = _passivePool.Get();
                equipmentUI.Initialize(passive.Description,
                    passive.Context.SkillInfo.SkillParameters.BasePower + "%", Color.white);
            }
        }
    }
}