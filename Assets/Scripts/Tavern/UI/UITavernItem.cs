using CryptoQuest.Tavern.Interfaces;
using CryptoQuest.UI.Menu.Panels.Status;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Tavern.UI
{
    public class UITavernItem : MonoBehaviour
    {
        [SerializeField] private Image _classIcon;
        [SerializeField] private LocalizeStringEvent _localizedName;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private GameObject _pendingTag;
        [SerializeField] private GameObject _equippedTag;

        [SerializeField] private RectTransform _tooltipPosition;

        public Transform Parent { get; set; }

        private ITooltip _tooltip;

        private bool _isSelected = false;
        private bool _isEquipped = false;

        private void Awake()
        {
            _tooltip = TooltipFactory.Instance.GetTooltip(ETooltipType.Equipment);
            _tooltip.WithBoderPointer(true)
                .WithLocalPosition(Vector3.zero)
                .WithScale(new Vector3(.7f, .7f, 0))
                .WithRangePivot(Vector2.zero, Vector2.one);
        }

        public void SetItemInfo(IGameCharacterData itemInfo)
        {
            _classIcon.sprite = itemInfo.GetClassIcon();
            _name.text = itemInfo.GetName();
            _level.text = $"Lv{itemInfo.GetLevel()}";
        }
        
        public void OnSelectToTransfer()
        {
            if (_isEquipped) return;
            // SelectItemEvent?.Invoke(this);

            _isSelected = !_isSelected;
            _pendingTag.SetActive(_isSelected);
        }

        public void OnInspecting(bool isInspecting)
        {
            if (isInspecting == false)
            {
                _tooltip.Hide();
                return;
            }

            _tooltip
                .WithLevel(1)
                .WithContentAwareness(_tooltipPosition);
        }
    }
}