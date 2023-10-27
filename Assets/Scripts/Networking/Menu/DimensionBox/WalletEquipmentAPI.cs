using System;
using System.Collections.Generic;
using CryptoQuest.System;
using Newtonsoft.Json;
using Proyecto26;
using UnityEngine;

namespace CryptoQuest.Networking.Menu.DimensionBox
{
    public class WalletEquipmentTransferData
    {
        [JsonProperty("ids")]
        public int[] Ids;

        public WalletEquipmentTransferData(int[] values)
        {
            Ids = values;
        }
    }

    public class WalletEquipmentAPI : MonoBehaviour
    {
        private IRestClient _restAPINetwork;

        public bool IsFinishFetchData { get; private set; }
        public string RawEquipmentData { get; private set; }

        private void Awake()
        {
            _restAPINetwork = ServiceProvider.GetService<IRestClient>();
        }

        private void OnEnable()
        {
            IsFinishFetchData = false;
        }

        public void LoadEquipmentFromWallet()
        {
            // TODO: REFACTOR NETWORK
            // _restAPINetwork.Get(Constants.LOAD_EQUIPMENT_PATH, OnSuccess, OnFail);
        }

        public void UpdateEquipmentFromWallet(int[] values)
        {
            var data = new WalletEquipmentTransferData(values);

            // TODO: REFACTOR NETWORK
            // _restAPINetwork.Put(Constants.UPDATE_EQUIPMENT_FROM_WALLET_PATH, JsonConvert.SerializeObject(data), OnSuccess, OnFail);
        }

        private void OnSuccess(ResponseHelper res)
        {
            Debug.Log($"DimensionEquipment::LoadData success : {res.Text}");
            RawEquipmentData = res.Text;
            IsFinishFetchData = true;
        }

        private void OnFail(Exception error)
        {
            Debug.Log($"DimensionEquipment::LoadData fail : {error.Message}");
            IsFinishFetchData = true;
        }

        public List<Equipment> GetDataConvertedFromJson(string jsonData)
        {
            var data = JsonConvert.DeserializeObject<WalletEquipmentResponseData>(jsonData);

            return data.Data.Equipments;
        }
    }
}