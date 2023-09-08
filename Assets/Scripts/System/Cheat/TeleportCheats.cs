using System;
using System.Collections.Generic;
using System.IO;
using CommandTerminal;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEditor;
using UnityEngine;

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
            var destination = args[0].String;
            var guids = AssetDatabase.FindAssets("t:SceneScriptableObject");
            foreach (var guid in guids)
            {
                var sceneSO = AssetDatabase.LoadAssetAtPath<SceneScriptableObject>(AssetDatabase.GUIDToAssetPath(guid));

                if (SimplifyString(sceneSO.name) == SimplifyString(destination))
                {
                    _loadSceneEventChannelSO.RequestLoad(sceneSO);
                    break;
                }
            }
        }

        private string SimplifyString(string str)
        {
            string returnStr = str
                .ToLower()
                .Replace(" ", "")
                .Replace("_", "")
                .Replace("Scene", "")
                .Replace("scene", "")
                .Replace("-", "")
                .Replace(".", "");
            return returnStr;
        }
    }
}