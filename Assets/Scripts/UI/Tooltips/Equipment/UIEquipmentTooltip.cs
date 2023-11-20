using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Tooltips.Equipment
{
    public interface ITooltipEquipmentProvider
    {
        public EquipmentInfo Equipment { get; }
    }

    public class UIEquipmentTooltip : UITooltipBase
    {
        [SerializeField] private Image _headerBackground;
        [SerializeField] private Image _rarity;
        [SerializeField] private GameObject _nftTag;
        [SerializeField] private Image _illustration;
        [SerializeField] private LocalizeStringEvent _nameLocalize;

        private EquipmentInfo _equipment;

        protected override bool CanShow()
        {
            var selectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (selectedGameObject == null) return false;
            var provider = selectedGameObject.GetComponent<ITooltipEquipmentProvider>();
            if (provider == null) return false;
            if (provider.Equipment == null || provider.Equipment.IsValid() == false) return false;
            _equipment = provider.Equipment;
            return true;
        }

        protected override void Init()
        {
            var behaviours = GetComponents<TooltipBehaviourBase>();
            foreach (var behaviour in behaviours) behaviour.Setup();
            SetupInfo();
            _illustration.LoadSpriteAndSet(_equipment.Config.Image);
        }

        private void SetupInfo()
        {
            _illustration.enabled = false;
            _headerBackground.color = _equipment.Rarity.Color;
            _rarity.sprite = _equipment.Rarity.Icon;
            _nftTag.SetActive(_equipment.IsNftItem);
            _nameLocalize.StringReference = _equipment.DisplayName;
        }

        private void OnDisable()
        {
            if (_equipment == null ||
                _equipment.Config == null ||
                _equipment.Config.Image.RuntimeKeyIsValid() == false)
                return;
            _equipment.Config.Image.ReleaseAsset();
            _equipment = null;
        }
    }
}