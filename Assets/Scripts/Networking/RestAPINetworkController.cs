using CryptoQuest.Environment;
using CryptoQuest.Networking;
using CryptoQuest.Networking.RestAPI;
using CryptoQuest.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UniRx;
using CryptoQuest.SNS;

namespace CryptoQuest.Networking
{
    public interface IRestAPINetworkController
    {
        void Post(string url, string data, Action<UnityWebRequest> OnSuccess, Action<Exception> OnFail);
        void Get(string url, Action<UnityWebRequest> OnSuccess, Action<Exception> OnFail);
    }
    public class RestAPINetworkController : MonoBehaviour, IRestAPINetworkController
    {
        [SerializeField] private EnvironmentSO _environmentSO;
        [SerializeField] private AuthorizationSO _authorizationSO;

        private Dictionary<string, string> _header = new();

        private void Awake()
        {
            ServiceProvider.Provide<IRestAPINetworkController>(this);
        }

        private void OnEnable()
        {
            _authorizationSO.OnAccessTokenChanged += InitHeader;
        }

        private void OnDisable()
        {
            _authorizationSO.OnAccessTokenChanged -= InitHeader;
        }

        private void InitHeader(ApiToken token)
        {
            _header.Clear();
            _header.Add("Authorization", "Bearer " + token.Token);
        }    

        public void Post(string url, string data, Action<UnityWebRequest> OnSuccess, Action<Exception> OnFail)
        {
            var fullUrl = _environmentSO.BackEndUrl + url;
            Post(fullUrl, data, _header, OnSuccess, OnFail);
        }

        public void Get(string url, Action<UnityWebRequest> OnSuccess, Action<Exception> OnFail)
        {
            var fullUrl = _environmentSO.BackEndUrl + url;
            Get(fullUrl, _header, OnSuccess, OnFail);
        }

        private void Post(string url, string data, Dictionary<string, string> header, Action<UnityWebRequest> OnSuccess, Action<Exception> OnFail)
        {
            HttpClient.Post(url, data, header).Subscribe(OnSuccess, OnFail);
        }

        private void Get(string url, Dictionary<string, string> header, Action<UnityWebRequest> OnSuccess, Action<Exception> OnFail)
        {
            HttpClient.Get(url, header).Subscribe(OnSuccess, OnFail);
        }    
    }
}
