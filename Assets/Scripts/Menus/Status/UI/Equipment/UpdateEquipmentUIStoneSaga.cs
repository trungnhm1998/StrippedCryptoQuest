using CryptoQuest.Item.Equipment;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Sagas.Equipment;
using CryptoQuest.UI.Extensions;
using IndiGames.Core.Events;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    /// <summary>
    /// Update UI when equipment is updated
    /// </summary>
    public class UpdateEquipmentUIStoneSaga : SagaBase<EquipmentUpdated>
    {
        [SerializeField] private UIEquipment _equipmentUI;

        protected override void HandleAction(EquipmentUpdated ctx)
        {
            if (!_equipmentUI.Equipment.IsValid() ||
                ctx.Equipment != _equipmentUI.Equipment) return;

            _equipmentUI.InitStone(_equipmentUI.Equipment);
        }
    }
}