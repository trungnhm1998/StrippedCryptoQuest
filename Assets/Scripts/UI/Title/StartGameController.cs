using System.Collections;
using CryptoQuest.System.SaveSystem;
using CryptoQuest.System.SaveSystem.Savers;
using IndiGames.Core.Events;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.UI.Title
{
    public class StartGameAction : ActionBase { }

    public class StartGameController : SagaBase<StartGameAction>
    {
        [SerializeField] private SaveSystemSO _save;
        [SerializeField] private SceneScriptableObject _defaultStartScene;

        [Header("Raise on")]
        [SerializeField] private LoadSceneEventChannelSO _loadMapChannel;

        protected override void HandleAction(StartGameAction ctx)
        {
            if (_save.SaveData.TryGetValue(SceneSaver.Key, out var sceneGuid))
            {
                StartCoroutine(CoLoadScene(sceneGuid));
                return;
            }

            _loadMapChannel.RequestLoad(_defaultStartScene);
        }

        private IEnumerator CoLoadScene(string sceneGuid)
        {
            var handle = Addressables.LoadAssetAsync<SceneScriptableObject>(sceneGuid);
            yield return handle;
            _loadMapChannel.RequestLoad(handle.Result);
        }
    }
}