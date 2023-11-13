using CommandTerminal;
using CryptoQuest.Core;
using CryptoQuest.Sagas;
using DG.Tweening;
using UnityEngine;

namespace CryptoQuest.System.Cheat
{
    public class SystemCheat : MonoBehaviour, ICheatInitializer
    {
        public void InitCheats()
        {
            Terminal.Shell.AddCommand("speed", TriggerModifySpeedValue, 1, 1, "speed <new_value>, to modify current speed value of game");
            Terminal.Shell.AddCommand("profile.clear", ClearSavedProfile, 0, -1, "clear the user saved profile in save system");
        }

        private void TriggerModifySpeedValue(CommandArg[] obj)
        {
            var newSpeed = obj[0].Float;
            Time.timeScale = newSpeed;
            DOTween.timeScale = newSpeed;

            Debug.Log($"Updated game speed to {newSpeed}");
        }

        private void ClearSavedProfile(CommandArg[] obj)
        {
            ActionDispatcher.Dispatch(new ClearProfileAction());
        }
    }
}