using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using CryptoQuest.System;
using UnityEngine;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Networking.Menu.DimensionBox;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Models
{
    public class WalletEquipmentModel : MonoBehaviour, IWalletEquipmentModel
    {
        [SerializeField] private WalletEquipmentAPI _walletEquipmentAPI;

        public List<INFT> Data { get; private set; }
        public bool IsLoaded { get; private set; }

        private List<Equipment> _convertedEquipmentData;
        public List<Equipment> ConvertedEquipmentData => _convertedEquipmentData;

        public IEnumerator CoGetData()
        {
            Data ??= new List<INFT>();
            Data.Clear();

            _walletEquipmentAPI.LoadEquipmentFromWallet();

            IsLoaded = false;

            yield return new WaitUntil(() => _walletEquipmentAPI.IsFinishFetchData);

            var rawData = _walletEquipmentAPI.RawEquipmentData;
            _convertedEquipmentData = _walletEquipmentAPI.GetDataConvertedFromJson(rawData);

            StartCoroutine(CreateNewEquipment(_convertedEquipmentData));
        }

        private IEnumerator CreateNewEquipment(List<Equipment> equipments)
        {
            var defProvider = ServiceProvider.GetService<IEquipmentDefProvider>();

            foreach (var item in equipments)
            {
                var equip = new EquipmentInfo(item.Id, item.EquipmentId, item.Lv);
                yield return defProvider.Load(equip);

                var obj = new WalletEquipmentData(equip);
                Data.Add(obj);
            }

            IsLoaded = true;
        }

        public void Transfer(int[] values)
        {
            _walletEquipmentAPI.UpdateEquipmentFromWallet(values);
        }
    }
}