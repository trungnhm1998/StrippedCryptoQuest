using CommandTerminal;
using CryptoQuest.Sagas;
using CryptoQuest.System.SaveSystem;
using IndiGames.Core.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.System.Cheat
{
    public class ProfileCheats : MonoBehaviour, ICheatInitializer
    {
        [SerializeField] private SaveSystemSO _saveSystem;
        [SerializeField] private VoidEventChannelSO _forceSaveEvent;

        public void InitCheats()
        {
            Terminal.Shell.AddCommand("profile.clear", ClearSavedProfile, 0, -1,
                "clear the user saved profile in save system");
        }

        private void ClearSavedProfile(CommandArg[] obj)
        {
            _saveSystem.SaveData = new SaveData();

            _forceSaveEvent.RaiseEvent();

            ActionDispatcher.Dispatch(new GoToTitleAction());

            Debug.Log("<color=green>ProfileCheats.ClearSavedProfile</color> profile cleared");
        }
    }
}