using System.Threading.Tasks;

namespace CryptoQuest.System.SaveSystem
{
    public class OnlineSaveManagerSO : SaveManagerSO
    {
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
            // TODO: REFACTOR NETWORK
            // var formData = new WWWForm();
            // formData.AddField("game-data", saveData);
            // var req = await HttpClient.PostAsync(_environmentSO.BackEndUrl + URL_SAVE_GAME, formData,
            //     new Dictionary<string, string>()
            //     {
            //         { "Authorization", "Bearer " + _authorizationSO.AccessToken.Token }
            //     });
            // return req != null && req.responseCode == 200;
            return true;
        }

        public async override Task<string> LoadAsync()
        {
            // TODO: REFACTOR NETWORK
            // var req = await HttpClient.GetAsync(_environmentSO.BackEndUrl + URL_SAVE_GAME,
            //     new Dictionary<string, string>()
            //     {
            //         { "Authorization", "Bearer " + _authorizationSO.AccessToken.Token }
            //     });
            // if (req != null && req.responseCode == 200)
            // {
            //     return req.downloadHandler.text;
            // }

            return null;
        }
    }
}