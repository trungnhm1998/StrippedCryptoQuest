using System;
using System.Collections;
using CryptoQuest.API;
using CryptoQuest.Inventory.ScriptableObjects;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Networking;
using CryptoQuest.Sagas.Equipment;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using UniRx;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    [Serializable]
    public class EquipmentsLoader : Loader
    {
        [SerializeField] private EquipmentInventory _equipmentInventory;
        [SerializeField] private EquipmentsMinStatsSO _equipmentsMinStatsSO;

        public override IEnumerator LoadAsync()
        {
            _equipmentsMinStatsSO.EquipmentsMinStats.Clear();
            
            var restClient = ServiceProvider.GetService<IRestClient>();
            var op = restClient
                .WithParam("source", $"{((int)EEquipmentStatus.InGame).ToString()}")
                .Get<EquipmentsResponse>(EquipmentAPI.EQUIPMENTS)
                .ToYieldInstruction();
            yield return op;

            var converter = ServiceProvider.GetService<IEquipmentResponseConverter>();
            _equipmentInventory.Equipments.Clear();
            var equipmentResponses = op.Result.data.equipments;
            foreach (var equipmentResponse in equipmentResponses)
            {
                var equipment = converter.Convert(equipmentResponse);
                _equipmentInventory.Equipments.Add(equipment);
            }
        }
    }
}