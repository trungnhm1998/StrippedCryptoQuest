using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.System;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

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

        private IRestAPINetworkController _restAPINetworkController;
        private ICurrenciesController _currenciesController;
        private float WebMetad;
        public float IngameMetad => _currenciesController.Wallet.Diamond.Amount;

        private const string URL_LOAD_METAD = "/crypto/dimention/token";
        private const string URL_TRANSFER_TO_METAD = "/crypto/dimention/token/to";
        private const string URL_TRANSFER_TO_DIAMOND = "/crypto/dimention/token/from";

        #region Network

        public IEnumerator CoLoadData()
        {
            _currenciesController = ServiceProvider.GetService<ICurrenciesController>();
            _restAPINetworkController = ServiceProvider.GetService<IRestAPINetworkController>();
            _restAPINetworkController.Get(URL_LOAD_METAD, OnLoadDataSuccess, OnLoadDataFail);
            yield return null;
        }

        public void TransferDiamondToMetad(float value)
        {
            Debug.Log("MetadAPI Execute transfer diamond to metad");
            TransferMetad(URL_TRANSFER_TO_METAD, value);
        }

        public void TransferMetadToDiamond(float value)
        {
            Debug.Log("MetadAPI Execute transfer metad to diamond");
            TransferMetad(URL_TRANSFER_TO_DIAMOND, value);
        }

        private void TransferMetad(string url, float value)
        {
            var payload = new MetadPayload(value);

            _restAPINetworkController.Post(url, JsonConvert.SerializeObject(payload), OnSendWalletSuccess, OnSendWalletFailed);
        }

        private void OnLoadDataSuccess(UnityWebRequest request)
        {
            Debug.Log($"Dimention::LoadData success : {request.downloadHandler.text}");
            UpdateCurrency(request.downloadHandler.text);
        }

        private void OnLoadDataFail(Exception error)
        {
            Debug.Log($"Dimention::LoadData fail : {error.Message}");
        }

        private void OnSendWalletSuccess(UnityWebRequest request)
        {
            Debug.Log($"Dimention::SendWallet success : {request.downloadHandler.text}");
            UpdateCurrency(request.downloadHandler.text);
            OnSendSuccess?.Invoke();
        }

        private void OnSendWalletFailed(Exception exception)
        {
            Debug.Log($"Dimention::LoadData fail : {exception.Message}");
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
