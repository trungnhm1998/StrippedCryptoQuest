using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item.Equipment;
using CryptoQuest.System;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;

namespace CryptoQuest.Networking.Menu.DimensionBox
{
    public class WalletEquipmentData
    {
        [JsonProperty("ids")]
        public int[] Ids;
        public WalletEquipmentData(int[] value)
        {
            Ids = value;
        }
    }

    public class WalletEquipmentAPI : MonoBehaviour, IWalletEquipmentModel
    {
        public List<INFT> Data { get; private set; }
        public bool IsLoaded { get; private set; }

        private IRestClientController _restAPINetworkController;

        private const string LOAD_EQUIPMENT_PATH = "/crypto/equipments?source=1";
        private const string UPDATE_EQUIPMENT_FROM_WALLET_PATH = "/crypto/equipments/dimention/from";

        private List<Equipment> _equipmentRawData;
        public List<Equipment> EquipmentRawData => _equipmentRawData;

        public IEnumerator CoGetData()
        {
            IsLoaded = false;
            yield return null;
            _restAPINetworkController = ServiceProvider.GetService<IRestClientController>();
            _restAPINetworkController.Get(LOAD_EQUIPMENT_PATH, OnLoadDataSuccess, OnLoadDataFail);
        }

        public void Transfer(int[] values)
        {
            var data = new WalletEquipmentData(values);

            _restAPINetworkController.Put(UPDATE_EQUIPMENT_FROM_WALLET_PATH, JsonConvert.SerializeObject(data), OnLoadDataSuccess, OnLoadDataFail);
        }

        private void OnLoadDataSuccess(ResponseHelper res)
        {
            Debug.Log($"DimensionEquipment::LoadData success : {res.Text}");
            UpdateEquipmentData(res.Text);
        }

        private void OnLoadDataFail(Exception error)
        {
            Debug.Log($"DimensionEquipment::LoadData fail : {error.Message}");
            IsLoaded = true;
        }

        private void UpdateEquipmentData(string jsonData)
        {
            var data = JsonConvert.DeserializeObject<WalletEquipmentResponseData>(jsonData);

            Data ??= new List<INFT>();
            Data.Clear();
            StartCoroutine(LoadAllData(data.Data.Equipments));
        }

        private IEnumerator LoadAllData(List<Equipment> equipments)
        {
            var defProvider = ServiceProvider.GetService<IEquipmentDefProvider>();

            _equipmentRawData = equipments;

            foreach (var item in equipments)
            {
                var equip = new EquipmentInfo(item.Id.ToString(), item.EquipmentId, item.Lv);
                yield return defProvider.Load(equip);

                var obj = new UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Models.WalletEquipmentData(equip);
                Data.Add(obj);
            }

            IsLoaded = true;
        }
    }
}
