using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Menu;
using UnityEngine;

namespace CryptoQuest.Shop.UI.Item
{
    public class PreviewItem : MonoBehaviour, IPreviewItem
    {
        [SerializeField] private RectTransform _tooltipPosition;
        [SerializeField] private Vector2 _tooltipPivot = new Vector2(0.5f, 0.5f);
        private ITooltip _tooltip;
        private float _delayTime = 0;
        private bool _hasPointerBoder = false;

        private void SetupTooltip(ETooltipType type)
        {
            _tooltip = TooltipFactory.Instance.GetTooltip(type);
            _tooltip.WithContentAwareness(null)
                .WithPivot(_tooltipPivot)
                .WithLocalPosition(_tooltipPosition.localPosition)
                .WithScale(_tooltipPosition.localScale)
                .WithBoderPointer(_hasPointerBoder)
                .WithDelayTimeToDisplay(_delayTime);
        }

        public void Preview(EquipmentInfo equipment)
        {
            SetupTooltip(ETooltipType.Equipment);
            _tooltip.WithLevel(equipment.Level)
                .WithDescription(equipment.Data.DisplayName)
                .WithDisplaySprite(equipment.Data.Image)
                .WithRarity(equipment.Rarity)
                .Show();
        }

        public void Preview(ConsumableInfo consumable)
        {
            SetupTooltip(ETooltipType.Consumable);
            _tooltip.WithHeader(consumable.DisplayName)
                .WithDisplaySprite(consumable.Icon)
                .WithDescription(consumable.Description)
                .Show();
        }

        public void Hide()
        {
            _tooltip.Hide();
        }
    }
}