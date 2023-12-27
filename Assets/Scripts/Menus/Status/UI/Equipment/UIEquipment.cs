using CryptoQuest.Item.Equipment;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.UI.Extensions;
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
        [SerializeField] private UIEquipmentStone[] _stoneUIs;

        [SerializeField] private Color _disabledColor;
        [SerializeField] private Color _enabledColor;

        [SerializeField] private MagicStoneInventory _stoneInventory; // Refactor if there is a performance issue

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
            if (!equipment.Prefab.DisplayName.IsEmpty) 
                _nameLocalize.StringReference = equipment.Prefab.DisplayName;
            _icon.sprite = equipment.Type.Icon;
        }

        public void InitStone(IEquipment equipment)
        {
            var maxSlots = equipment.Data.StoneSlots;
            for (int i = 0; i < maxSlots; i++)
            {
                _stoneUIs[i].gameObject.SetActive(true);
                _stoneUIs[i].ResetUI();
            }

            var attachStones = equipment.Data.AttachStones;
            for (int inventoryIdx = 0; inventoryIdx < _stoneInventory.MagicStones.Count; inventoryIdx++)
            {
                for (int attachIdx = 0; attachIdx < attachStones.Count; attachIdx++)
                {
                    if (_stoneInventory.MagicStones[inventoryIdx].ID != attachStones[attachIdx]) continue;
                    _stoneUIs[attachIdx].StoneIcon.LoadSpriteAndSet(
                        _stoneInventory.MagicStones[inventoryIdx].Definition.Image);
                }
            }
        }

        public void DisableButton()
        {
            _nameText.color = _disabledColor;
        }

        public void Reset() => _equipment = default;

        private void OnDisable()
        {
            foreach (var slot in _stoneUIs)
                slot.gameObject.SetActive(false);
        }
    }
}