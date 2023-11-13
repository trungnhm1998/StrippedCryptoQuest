using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Status.UI
{
    public class UIEquipmentTooltip : UITooltip
    {
        [SerializeField] private TMP_Text _effectDescription;
        [SerializeField] private Image _rarity;
        [SerializeField] private TMP_Text _level;

        public override UITooltip WithLevel(int equipmentLevel)
        {
            _level.text = $"Lv. { equipmentLevel}";
            return this;
        }

        public override UITooltip WithRarity(RaritySO rarity)
        {
            _rarity.sprite = rarity.Icon;
            return this;
        }

        public UITooltip WithEquipment(EquipmentInfo equipment)
        {
            return this;
        }
    }
}