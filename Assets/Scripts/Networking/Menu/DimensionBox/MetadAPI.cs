using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Networking;
using CryptoQuest.System;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
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
            _restAPINetworkController.Get(URL_LOAD_METAD, OnSuccess, OnFail);
            yield return null;
        }

        public void CoTransferDiamondToMeta(float value)
        {
            CoTransferMetad(URL_TRANSFER_TO_METAD, value);
        }    

        public void CoTransferMetadToDiamond(float value)
        {
            CoTransferMetad(URL_TRANSFER_TO_DIAMOND, value);
        }    

        private void CoTransferMetad(string url, float value)
        {
            var payload = new MetadPayload(value);

            _restAPINetworkController.Post(url, JsonConvert.SerializeObject(payload), OnSuccess, OnFail);
        }    
        private void OnSuccess(UnityWebRequest request)
        {
            Debug.Log($"Dimention::LoadData success : {request.downloadHandler.text}");
            var data = JsonConvert.DeserializeObject<MetadResponseData>(request.downloadHandler.text);
            WebMetad = data.Data.Metad;
            _currenciesController.Wallet.Diamond.SetCurrencyAmount(data.Diamond);
            CurrencyUpdated?.Invoke(IngameMetad, WebMetad);
        }

        private void OnFail(Exception error)
        {
            Debug.Log($"Dimention::LoadData fail : {error.Message}");
        }

        #endregion

        public float GetIngameMetad()
        {
            return IngameMetad;
        }

        public float GetWebMetad()
        {
            return WebMetad;
        }

        public void UpdateCurrency(float inputValue, bool isIngameWallet)
        {
            if (isIngameWallet)
            {
                WebMetad += inputValue;
                _currenciesController.Wallet.Diamond.SetCurrencyAmount(_currenciesController.Wallet.Diamond.Amount - inputValue);
            }
            else
            {
                WebMetad -= inputValue;
                _currenciesController.Wallet.Diamond.SetCurrencyAmount(_currenciesController.Wallet.Diamond.Amount + inputValue);
            }
            CurrencyUpdated?.Invoke(IngameMetad, WebMetad);
        }
    }
}
