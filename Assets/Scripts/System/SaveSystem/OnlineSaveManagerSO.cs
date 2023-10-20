using UnityEngine;
using CryptoQuest.Networking;
using System.Threading.Tasks;
using CryptoQuest.Environment;
using CryptoQuest.Networking.RestAPI;
using System.Collections.Generic;

namespace CryptoQuest.System.SaveSystem
{
    public class OnlineSaveManagerSO : SaveManagerSO
    {
        [SerializeField] private EnvironmentSO _environmentSO;
        [SerializeField] private AuthorizationSO _authorizationSO;

        private const string URL_SAVE_GAME = "/crypto/user/game-data";

        public override bool Save(string saveData)
        {
            var t = Task.Run(() => SaveAsync(saveData));
            t.Wait();
            return t.Result;
        }

        public override string Load()
        {
            var t = Task.Run(() => LoadAsync());
            t.Wait();
            return t.Result;
        }

        public async override Task<bool> SaveAsync(string saveData)
        {
            var formData = new WWWForm();
            formData.AddField("game-data", saveData);
            var req = await HttpClient.PostAsync(_environmentSO.BackEndUrl + URL_SAVE_GAME, formData, new Dictionary<string, string>() {
                { "Authorization", "Bearer " + _authorizationSO.AccessToken.Token }
            });
            return req != null && req.responseCode == 200;
        }

        public async override Task<string> LoadAsync()
        {
            var req = await HttpClient.GetAsync(_environmentSO.BackEndUrl + URL_SAVE_GAME, new Dictionary<string, string>() {
                { "Authorization", "Bearer " + _authorizationSO.AccessToken.Token }
            });
            if (req != null && req.responseCode == 200)
            {
                return req.downloadHandler.text;
            }
            return null;
        }
    }
}
