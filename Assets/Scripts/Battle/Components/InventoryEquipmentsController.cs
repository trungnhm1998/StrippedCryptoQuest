using CryptoQuest.Item.Equipment;

namespace CryptoQuest.Battle.Components
{
    public class InventoryEquipmentsController : CharacterComponentBase
    {
        private EquipmentsController _equipmentsController;
        private HeroBehaviour _hero;

        protected override void OnInit()
        {
            _equipmentsController = Character.GetComponent<EquipmentsController>();
            _hero = GetComponent<HeroBehaviour>();

            _equipmentsController.Equipped += SetEquipmentAttachId;
            _equipmentsController.Removed += RemoveEquipmentAttachId;
        }

        protected override void OnReset()
        {
            _equipmentsController.Equipped -= SetEquipmentAttachId;
            _equipmentsController.Removed -= RemoveEquipmentAttachId;
        }

        private void SetEquipmentAttachId(IEquipment equipment)
        {
            equipment.AttachCharacterId = _hero.Spec.Id;
        }

        private void RemoveEquipmentAttachId(IEquipment equipment)
        {
            equipment.AttachCharacterId = -1;
        }
    }
}