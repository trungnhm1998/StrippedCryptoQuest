using UnityEngine;
using IndiGames.Core.SaveSystem;
using CryptoQuest.Networking;
using System.Threading.Tasks;
using CryptoQuest.Environment;
using CryptoQuest.Networking.RestAPI;
using System.Collections.Generic;

namespace CryptoQuest.System.Save
{
    [CreateAssetMenu]
    public class OnlineSaveManagerSO : SaveManagerSO
    {
        [SerializeField] private EnvironmentSO _environmentSO;
        [SerializeField] private AuthorizationSO _authorizationSO;

        private const string URL_SAVE_GAME = "/crypto/user/game-data";

        public async override Task<bool> SaveAsync(SaveData saveData)
        {
            var formData = new WWWForm();
            formData.AddField("game-data", saveData.ToJson());
            var req = await HttpClient.PostAsync(_environmentSO.BackEndUrl + URL_SAVE_GAME, formData, new Dictionary<string, string>() {
                { "Authorization", "Bearer " + _authorizationSO.AccessToken.Token }
            });
            return req != null && req.responseCode == 200;
        }

        public async override Task<SaveData> LoadAsync()
        {
            var req = await HttpClient.GetAsync(_environmentSO.BackEndUrl + URL_SAVE_GAME, new Dictionary<string, string>() {
                { "Authorization", "Bearer " + _authorizationSO.AccessToken.Token }
            });
            if (req != null && req.responseCode == 200)
            {
                var saveData = new SaveData();
                saveData.LoadFromJson(req.downloadHandler.text);
                return saveData;
            }
            return null;
        }
    }
}
