using CryptoQuest.UI.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Obj = CryptoQuest.Sagas.Objects;

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
        public int Id { get; private set; }

        private ITooltip _tooltip;

        private bool _isSelected = false;
        private bool _isInParty = false;

        private Obj.Character _cachedInfo;

        private void Awake()
        {
            _tooltip = TooltipFactory.Instance.GetTooltip(ETooltipType.Character);
            _tooltip.WithBorderPointer(true)
                .WithLocalPosition(Vector3.zero)
                .WithScale(new Vector3(.8f, .8f, 0));
        }

        public void SetItemInfo(Obj.Character itemInfo)
        {
            Id = itemInfo.id;
            _name.text = itemInfo.name;
            _level.text = $"Lv{itemInfo.level}";
            _localizedName.RefreshString();

            _cachedInfo = itemInfo;
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
                .WithLevel(_cachedInfo.level)
                .WithContentAwareness(_tooltipPosition);
        }

        public void InspectDetails()
        {
            return; // disable waiting for the new tooltip
            _tooltip.Show();
        }
    }
}