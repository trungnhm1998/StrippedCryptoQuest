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

        private EquipmentInfo _equipment = new();
        public EquipmentInfo Equipment => _equipment;

        public void Init(EquipmentInfo equipment)
        {
            if (equipment.IsValid() == false) return;
            _equipment = equipment;
            var data = _equipment.Data;
            _nameText.text = $"{data.ID}-{data.PrefabId}";
            if (!equipment.DisplayName.IsEmpty) _nameLocalize.StringReference = equipment.DisplayName;
            _nameText.color = _enabledColor;
            _icon.sprite = equipment.EquipmentType.Icon;
            _iconNFT.SetActive(_equipment.IsNftItem);
        }

        public void DisableButton()
        {
            _nameText.color = _disabledColor;
        }

        public void Reset() => _equipment = new EquipmentInfo();
    }
}