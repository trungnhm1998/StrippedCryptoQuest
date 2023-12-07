using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Extensions;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Status.UI.MagicStone
{
    public class UIEquipmentDetails : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _localizedName;
        [SerializeField] private Image _illustration;

        private IEquipment _equipment;

        public void Init(IEquipment equipment)
        {
            _equipment = equipment;
            SetEquipmentInfo();
        }

        private void SetEquipmentInfo()
        {
            _icon.sprite = _equipment.Type.Icon;
            _localizedName.StringReference = _equipment.Prefab.DisplayName;
            _illustration.LoadSpriteAndSet(_equipment.Prefab.Image);
        }
    }
}