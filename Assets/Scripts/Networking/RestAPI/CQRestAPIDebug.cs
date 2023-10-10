using CryptoQuest.Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static CryptoQuest.SNS.FirebaseAuthScript;
using UniRx;
using System;
using Newtonsoft.Json;

namespace CryptoQuest.Networking.RestAPI
{
    public class CQRestAPIDebug : MonoBehaviour
    {

        [SerializeField] private EnvironmentSO _environmentSO;
        [SerializeField] private AuthorizationSO _authorizationSO;

#if UNITY_EDITOR
        public class DebugLoginPayLoad
        {
            public string token;
            public DebugLoginPayLoad(string token)
            {
                this.token = token;
            }
        }

        private void Start()
        {
            DebugLogin();
        }

        private void DebugLogin()
        {
            string url = _environmentSO.BackEndUrl + "/crypto/debug/login";

            var payload = new DebugLoginPayLoad(_environmentSO.DEBUG_TOKEN);

            Dictionary<string, string> headers = new();
            headers.Add("DEBUG_KEY", _environmentSO.DEBUG_KEY);

            HttpClient.Post(url, JsonUtility.ToJson(payload), headers).Subscribe(Result, OnFailed);
        }

        private void OnFailed(Exception error)
        {
            Debug.Log("Login fail {" + error.Message + "}");
        }

        private void Result(UnityWebRequest webRequest)
        {
            Debug.Log(webRequest.downloadHandler.text);
            _authorizationSO.Init(webRequest.downloadHandler.text);
        }
#endif

    }
}
