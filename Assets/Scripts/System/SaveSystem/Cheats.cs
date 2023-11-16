using CommandTerminal;
using CryptoQuest.System.Cheat;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.SaveSystem
{
    public class Cheats : MonoBehaviour, ICheatInitializer
    {
        [SerializeField] private SceneScriptableObject _titleScene;
        [SerializeField] private LoadSceneEventChannelSO _loadTitleEventChannel;
        [SerializeField] private SaveSystemSO _saveSystem;

        public void InitCheats()
        {
            Terminal.Shell.AddCommand("profile.clear", ClearSavedProfile, 0, -1,
                "clear the user saved profile in save system");
        }

        private void ClearSavedProfile(CommandArg[] obj)
        {
            _saveSystem.SaveData.Objects = new();
            _saveSystem.Save(); // this will override the localsave
            _loadTitleEventChannel.RequestLoad(_titleScene);
        }
    }
}