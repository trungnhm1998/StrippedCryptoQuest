using CryptoQuest.Item.Equipment;
using CryptoQuest.Menus.Status.UI.Equipment;
using UnityEngine;

namespace CryptoQuest.Menus.Status.UI.MagicStone
{
    public class UIEquipmentDetails : MonoBehaviour
    {
        [SerializeField] private UIAttachList _attachList;
        [SerializeField] private UIEquipment _uiEquipment;

        private IEquipment _equipment;
        public IEquipment Equipment => _equipment;

        public void Init(IEquipment equipment)
        {
            _equipment = equipment;
            SetEquipmentInfo();
            SetAttachingStones();
        }

        private void SetEquipmentInfo()
        {
            _uiEquipment.Init(_equipment);
            _attachList.Init(_equipment.Data.StoneSlots);
        }

        private void SetAttachingStones() => _uiEquipment.InitStone(_equipment);
    }
}