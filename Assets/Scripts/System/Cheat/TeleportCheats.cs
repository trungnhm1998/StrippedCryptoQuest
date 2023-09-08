using System.Collections.Generic;
using System.IO;
using CommandTerminal;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.System.Cheat
{
    public class TeleportCheats : MonoBehaviour, ICheatInitializer
    {
        [SerializeField] private LoadSceneEventChannelSO _loadSceneEventChannelSO;

        public void InitCheats()
        {
            Terminal.Shell.AddCommand("tp", TriggerTeleport, 1, 1, "Teleport to a location");
        }

        private void TriggerTeleport(CommandArg[] args)
        {
            var address = args[0].String;
            var handle = Addressables.LoadAssetAsync<SceneScriptableObject>(address);
            handle.Completed += operationHandle =>
            {
                if (operationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    var scene = operationHandle.Result;
                    LoadScene(scene);
                }
            };
        }

        private void LoadScene(SceneScriptableObject scene)
        {
            if (scene != null)
                _loadSceneEventChannelSO.RequestLoad(scene);
        }
    }
}