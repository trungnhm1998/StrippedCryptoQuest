using CommandTerminal;
using DG.Tweening;
using UnityEngine;

namespace CryptoQuest.System.Cheat
{
    public class SystemCheat : MonoBehaviour, ICheatInitializer
    {
        public void InitCheats()
        {
            Terminal.Shell.AddCommand("speed", TriggerModifySpeedValue, 1, 1,
                "speed <new_value>, to modify current speed value of game");
        }

        private void TriggerModifySpeedValue(CommandArg[] obj)
        {
            var newSpeed = obj[0].Float;
            Time.timeScale = newSpeed;
            DOTween.timeScale = newSpeed;

            Debug.Log($"Updated game speed to {newSpeed}");
        }
    }
}