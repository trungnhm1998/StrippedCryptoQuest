using UnityEngine;

namespace CryptoQuest.Environment
{
    public class EnvironmentSO : ScriptableObject
    {
        [SerializeField] private string _apiUrl = "http://dev-api-game.crypto-quest.org";
        [SerializeField] private string _apiVersion = "/v1";

        public string API => $"{_apiUrl}{_apiVersion}";
    }
}