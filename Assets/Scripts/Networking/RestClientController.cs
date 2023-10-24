using CryptoQuest.Environment;
using CryptoQuest.Networking.RestAPI;
using CryptoQuest.SNS;
using CryptoQuest.System;
using Proyecto26;
using System;
using UnityEngine;

namespace CryptoQuest.Networking
{
    public interface IRestClientController
    {
        void Get(string path, Action<ResponseHelper> OnSuccess, Action<Exception> OnFail);

        void Post(string path, string data, Action<ResponseHelper> OnSuccess, Action<Exception> OnFail);

        void Put(string path, string data, Action<ResponseHelper> OnSuccess, Action<Exception> OnFail);
    }

    public class RestClientController : MonoBehaviour, IRestClientController
    {
        [SerializeField] private EnvironmentSO _environmentSO;
        [SerializeField] private AuthorizationSO _authorizationSO;

        private string _host = "";

        private void Awake()
        {
            ServiceProvider.Provide<IRestClientController>(this);
            _host = _environmentSO.BackEndUrl;
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
            RestClient.DefaultRequestHeaders["Authorization"] = $"Bearer {token.Token}";
        }

        public void Get(string path, Action<ResponseHelper> OnSuccess, Action<Exception> OnFail)
        {
            RestClient
                    .Get($"{_host}{path}")
                    .Then(res => OnSuccess(res))
                    .Catch(OnFail);
        }

        public void Post(string path, string data, Action<ResponseHelper> OnSuccess, Action<Exception> OnFail)
        {
            RestClient
                    .Post($"{_host}{path}", data)
                    .Then(res => OnSuccess(res))
                    .Catch(OnFail);
        }

        public void Put(string path, string data, Action<ResponseHelper> OnSuccess, Action<Exception> OnFail)
        {
            RestClient
                    .Put($"{_host}{path}", data)
                    .Then(res => OnSuccess(res))
                    .Catch(OnFail);
        }
    }
}
