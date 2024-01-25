using CommandTerminal;
using CryptoQuest.System.SaveSystem.Sagas;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.System.Cheat
{
    public class ProfileCheats : MonoBehaviour, ICheatInitializer
    {
        public void InitCheats()
        {
            Terminal.Shell.AddCommand("profile.clear", ClearSavedProfile, 0, -1,
                "clear the user saved profile in save system");
        }

        private void ClearSavedProfile(CommandArg[] obj)
        {
            ActionDispatcher.Dispatch(new ClearProfileAction());
            Debug.Log("<color=green>ProfileCheats.ClearSavedProfile</color> profile cleared");
        }
    }
}