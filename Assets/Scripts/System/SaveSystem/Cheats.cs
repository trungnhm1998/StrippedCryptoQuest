using CommandTerminal;
using CryptoQuest.Core;
using CryptoQuest.SaveSystem.Sagas;
using CryptoQuest.System.Cheat;
using UnityEngine;

namespace CryptoQuest.SaveSystem
{
    public class Cheats : MonoBehaviour, ICheatInitializer
    {
        public void InitCheats()
        {
            Terminal.Shell.AddCommand("profile.clear", ClearSavedProfile, 0, -1,
                "clear the user saved profile in save system");
        }

        private void ClearSavedProfile(CommandArg[] obj)
        {
            ActionDispatcher.Dispatch(new ClearProfileAction());
        }
    }
}