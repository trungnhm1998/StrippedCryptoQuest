using CryptoQuest.Tavern.Interfaces;
using CryptoQuest.UI.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Tavern.UI
{
    public class UITavernItem : MonoBehaviour
    {
        public static event UnityAction<UITavernItem> Pressed;

        [SerializeField] private Image _classIcon;
        [SerializeField] private LocalizeStringEvent _localizedName;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private GameObject _pendingTag;
        [SerializeField] private GameObject _inPartyTag;

        [SerializeField] private RectTransform _tooltipPosition;

        public Transform Parent { get; set; }

        private ITooltip _tooltip;

        private bool _isSelected = false;
        private bool _isInParty = false;

        private void Awake()
        {
            _tooltip = TooltipFactory.Instance.GetTooltip(ETooltipType.Equipment);
            _tooltip.WithBoderPointer(true)
                .WithLocalPosition(Vector3.zero)
                .WithScale(new Vector3(.7f, .7f, 0))
                .WithRangePivot(Vector2.zero, Vector2.one);
        }

        public void SetItemInfo(ICharacterData itemInfo)
        {
            _classIcon.sprite = itemInfo.GetClassIcon();
            _localizedName.StringReference = itemInfo.GetLocalizedName();
            _name.text = itemInfo.GetName();
            _level.text = $"Lv{itemInfo.GetLevel()}";
            _isInParty = itemInfo.IsInParty();
            _localizedName.RefreshString();
        }

        public void OnSelectToTransfer()
        {
            if (_isInParty) return;
            Pressed?.Invoke(this);

            _isSelected = !_isSelected;
            EnablePendingTag(_isSelected);
        }

        public void EnablePendingTag(bool enable) => _pendingTag.SetActive(enable);

        public void Transfer(Transform parent)
        {
            gameObject.transform.SetParent(parent);
            Parent = parent;
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