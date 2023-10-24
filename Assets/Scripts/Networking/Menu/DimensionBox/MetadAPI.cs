using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.System;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using Newtonsoft.Json;
using Proyecto26;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Networking.Menu.DimensionBox
{
    public class MetadPayload
    {
        [JsonProperty("amount")]
        public float Amount;
        public MetadPayload(float value)
        {
            Amount = value;
        }
    }

    public class MetadAPI : MonoBehaviour, IMetadModel
    {
        public event UnityAction<float, float> CurrencyUpdated;
        public event UnityAction OnSendSuccess;
        public event UnityAction OnSendFailed;

        private IRestClientController _restAPINetworkController;
        private ICurrenciesController _currenciesController;
        private float WebMetad;
        public float IngameMetad => _currenciesController.Wallet.Diamond.Amount;

        #region Network

        public IEnumerator CoLoadData()
        {
            _currenciesController = ServiceProvider.GetService<ICurrenciesController>();
            _restAPINetworkController = ServiceProvider.GetService<IRestClientController>();
            _restAPINetworkController.Get(Constants.LOAD_METAD_PATH, OnLoadDataSuccess, OnLoadDataFail);
            yield return null;
        }

        public void TransferDiamondToMetad(float value)
        {
            Debug.Log("MetadAPI Execute transfer diamond to metad");
            TransferMetad(Constants.TRANSFER_TO_METAD_PATH, value);
        }

        public void TransferMetadToDiamond(float value)
        {
            Debug.Log("MetadAPI Execute transfer metad to diamond");
            TransferMetad(Constants.TRANSFER_TO_DIAMOND_PATH, value);
        }

        private void TransferMetad(string path, float value)
        {
            var payload = new MetadPayload(value);

            _restAPINetworkController.Post(path, JsonConvert.SerializeObject(payload), OnSendWalletSuccess, OnSendWalletFailed);
        }

        private void OnLoadDataSuccess(ResponseHelper res)
        {
            Debug.Log($"Dimension::LoadData success : {res.Text}");
            UpdateCurrency(res.Text);
        }

        private void OnLoadDataFail(Exception error)
        {
            Debug.Log($"Dimension::LoadData fail : {error.Message}");
        }

        private void OnSendWalletSuccess(ResponseHelper res)
        {
            Debug.Log($"Dimension::SendWallet success : {res.Text}");
            UpdateCurrency(res.Text);
            OnSendSuccess?.Invoke();
        }

        private void OnSendWalletFailed(Exception exception)
        {
            Debug.Log($"Dimension::LoadData fail : {exception.Message}");
            OnSendFailed?.Invoke();
        }

        #endregion

        private void UpdateCurrency(string dataJson)
        {
            var data = JsonConvert.DeserializeObject<MetadResponseData>(dataJson);
            WebMetad = data.Data.Metad;
            _currenciesController.Wallet.Diamond.SetCurrencyAmount(data.Diamond);
            CurrencyUpdated?.Invoke(IngameMetad, WebMetad);
        }

        public float GetIngameMetad()
        {
            return IngameMetad;
        }

        public float GetWebMetad()
        {
            return WebMetad;
        }
    }
}
