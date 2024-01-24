using CryptoQuest.Item.Equipment;
using CryptoQuest.System.SaveSystem.Savers;
using IndiGames.Core.Events;

namespace CryptoQuest.Battle.Components
{
    public class EquipmentCharacterMapController : CharacterComponentBase
    {
        private EquipmentsController _equipmentsController;
        private IHeroSpecProvider _heroSpecProvider;

        private void OnEnable()
        {
            _equipmentsController ??= Character.GetComponent<EquipmentsController>();
            _heroSpecProvider ??= Character.GetComponent<IHeroSpecProvider>();
            _equipmentsController.Equipped += UpdateCharacterId;
            _equipmentsController.Removed += RemoveCharacterId;
        }

        private void OnDisable()
        {
            _equipmentsController.Equipped -= UpdateCharacterId;
            _equipmentsController.Removed -= RemoveCharacterId;
        }

        private void UpdateCharacterId(IEquipment item)
        {
            item.AttachCharacterId = _heroSpecProvider.Spec.Id;
            OnSave();
        }

        private void RemoveCharacterId(IEquipment item)
        {
            item.AttachCharacterId = -1;
            OnSave();
        }

        private static void OnSave()
        {
            ActionDispatcher.Dispatch(new SaveEquipmentAction());
        }
    }
}