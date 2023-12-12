using System.Collections.Generic;
using CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Menus.DimensionalBox.UI.EquipmentsTransfer
{
    public class UIEquipmentList : UIDimensionalBoxList<UIEquipment>
    {
        public List<UIEquipment> SelectedItems
        {
            get
            {
                var equipments = GetComponentsInChildren<UIEquipment>();
                var equipmentsToTransfer = new List<UIEquipment>();
                foreach (var equipment in equipments)
                {
                    if (!equipment.MarkedForTransfer) continue;
                    equipmentsToTransfer.Add(equipment);
                }

                return equipmentsToTransfer;
            }
        }

        public void Initialize(EquipmentResponse[] equipments)
        {
            Clear();
            foreach (var equipment in equipments)
            {
                var uiEquipment = GetItem();
                uiEquipment.Initialize(equipment);
            }
        }

        public void Reset()
        {
            foreach (var uiEquipment in GetComponentsInChildren<UIEquipment>())
            {
                uiEquipment.MarkedForTransfer = false;
            }
        }

        protected override void OnRelease(UIEquipment item)
        {
            base.OnRelease(item);
            item.MarkedForTransfer = false;
        }

        protected override void OnGet(UIEquipment uiItem)
        {
            base.OnGet(uiItem);
            uiItem.MarkedForTransfer = false;
        }
    }
}