using CryptoQuest.Beast;
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
        [SerializeField] private Button _button;
        [SerializeField] private RectTransform _tooltipPosition;
        public IBeast Beast { get; private set; }
        public Transform Parent { get; set; }
        public int Id { get; private set; }

        private bool _isSelected = false;
        private bool _isInGame = false;

        public void OnSelectToTransfer()
        {
            if (_isInGame) return;
            Pressed?.Invoke(this);

            _isSelected = !_isSelected;
            EnablePendingTag(_isSelected);
        }

        public void EnablePendingTag(bool isSelected) => _pendingTag.SetActive(isSelected);
        public void EnableButton(bool isEnable) => _button.enabled = isEnable;

        public void Transfer(Transform parent)
        {
            gameObject.transform.SetParent(parent);
            Parent = parent;
        }

        public void OnInspecting(bool isInspecting) { }

        public void SetItemInfo(IBeast beast)
        {
            Beast = beast;
            Id = beast.Id;
            _localizeName.StringReference = beast.LocalizedName;
            _level.text = $"Lv{beast.Level}";
            _localizeName.RefreshString();
        }
    }
}