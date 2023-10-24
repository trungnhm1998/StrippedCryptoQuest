using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item.Equipment;
using CryptoQuest.System;
using CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Models;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Proyecto26;
using CryptoQuest.Environment;

namespace CryptoQuest.Networking.Menu.DimensionBox
{
    public class WalletEquipmentAPI : MonoBehaviour, IWalletEquipmentModel
    {
        public List<INFT> Data { get; private set; }
        public bool IsLoaded { get; private set; }

        private IRestClientController _restAPINetworkController;

        private const string LOAD_EQUIPMENT_PATH = "/crypto/equipments?source=1";

        public IEnumerator CoGetData()
        {
            IsLoaded = false;
            yield return null;
            _restAPINetworkController = ServiceProvider.GetService<IRestClientController>();
            _restAPINetworkController.Get(LOAD_EQUIPMENT_PATH, OnLoadDataSuccess, OnLoadDataFail);
        }

        public void Transfer()
        {
            throw new global::System.NotImplementedException();
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

            foreach (var item in equipments)
            {
                var equip = new EquipmentInfo(item.Id.ToString(), item.EquipmentId, item.Lv);
                yield return defProvider.Load(equip);

                var obj = new WalletEquipmentData(equip);
                Data.Add(obj);
            }

            IsLoaded = true;
        }
    }
}
