using CryptoQuest.Item.Equipment;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    public class UIEquipment : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _iconNFT;
        [SerializeField] private LocalizeStringEvent _nameLocalize;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private RectTransform _tooltipPosition;

        [SerializeField] private Color _disabledColor;
        [SerializeField] private Color _enabledColor;
        [SerializeField] private bool _loadImageOnEnable;

        private IEquipment _equipment;
        public IEquipment Equipment => _equipment;

        public void Init(IEquipment equipment)
        {
            if (equipment.IsValid() == false) return;
            _equipment = equipment;
            var data = _equipment.Data;
            _iconNFT.SetActive(_equipment.IsNft);
            _nameText.text = $"{data.ID}-{_equipment.Prefab.ID}";
            _nameText.color = _enabledColor;
            if (!equipment.Prefab.DisplayName.IsEmpty) _nameLocalize.StringReference = equipment.Prefab.DisplayName;
            _icon.sprite = equipment.Type.Icon;
        }

        public void DisableButton()
        {
            _nameText.color = _disabledColor;
        }

        public void Reset() => _equipment = default;
    }
}