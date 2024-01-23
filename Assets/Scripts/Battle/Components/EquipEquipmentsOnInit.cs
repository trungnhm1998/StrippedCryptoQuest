using CryptoQuest.Inventory.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    /// <summary>
    /// Make sure the order of this component is correct
    /// </summary>
    public class EquipEquipmentsOnInit : CharacterComponentBase
    {
        [SerializeField] private EquipmentInventory _equipmentInventory;

        private IHeroSpecProvider _heroSpecProvider;
        private EquipmentsController _equipmentsController;

        protected override void OnInit()
        {
            _heroSpecProvider ??= GetComponent<IHeroSpecProvider>();
            _equipmentsController ??= GetComponent<EquipmentsController>();
            // foreach (var equipment in _equipmentInventory.Equipments)
            // {
            //     if (equipment.AttachCharacterId == _heroSpecProvider.Spec.Id)
            //     {
            //         _equipmentsController.Equip(equipment, );
            //     }
            // }
        }
    }
}