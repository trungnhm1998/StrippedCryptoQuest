using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Extensions;
using CryptoQuest.UI.Tooltips.Equipment;
using UnityEngine;
using UnityEngine.EventSystems;
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

        private void OnEnable()
        {
            InitInfoProvider();
            SetEquipmentInfo();
        }

        private void InitInfoProvider()
        {
            var selectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (selectedGameObject == null) return;
            var provider = selectedGameObject.GetComponent<ITooltipEquipmentProvider>();
            if (provider == null) return;
            if (provider.Equipment == null || provider.Equipment.IsValid() == false) return;
            _equipment = provider.Equipment;
        }

        private void SetEquipmentInfo()
        {
            _icon.sprite = _equipment.Type.Icon;
            _localizedName.StringReference = _equipment.Prefab.DisplayName;
            _illustration.LoadSpriteAndSet(_equipment.Prefab.Image);
        }
    }
}