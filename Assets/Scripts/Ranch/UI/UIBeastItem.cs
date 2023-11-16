using CryptoQuest.UI.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Ranch.UI
{
    public class UIBeastItem : MonoBehaviour
    {
        public static event UnityAction<UIBeastItem> Pressed;

        [SerializeField] private Image _beatIcon;
        [SerializeField] private LocalizeStringEvent _localizeName;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private GameObject _pendingTag;
        [SerializeField] private GameObject _inGameTag;

        [SerializeField] private RectTransform _tooltipPosition;

        public Transform Parent { get; set; }
        public int Id { get; private set; }

        private ITooltip _tooltip;

        private bool _isSelected = false;
        private bool _isInGame = false;

        private void Awake()
        {
            _tooltip = TooltipFactory.Instance.GetTooltip(ETooltipType.Beast);
            _tooltip.WithBorderPointer(true)
                .WithLocalPosition(Vector3.zero)
                .WithScale(new Vector3(.8f, .8f, 0));
        }

        public void OnSelectToTransfer()
        {
            if (_isInGame) return;
            Pressed?.Invoke(this);

            _isSelected = !_isSelected;
            EnablePendingTag(_isSelected);
        }

        public void EnablePendingTag(bool isSelected) => _pendingTag.SetActive(isSelected);

        public void Transfer(Transform parent)
        {
            gameObject.transform.SetParent(parent);
            Parent = parent;
        }

        public void OnInspecting(bool isInspecting)
        {
            if (!isInspecting)
            {
                _tooltip.Hide();
                return;
            }

            _tooltip
                // .WithSomethingHereForBeat
                .WithContentAwareness(_tooltipPosition);
        }

        public void InspectDetail()
        {
            return;
            _tooltip.Show();
        }
    }
}