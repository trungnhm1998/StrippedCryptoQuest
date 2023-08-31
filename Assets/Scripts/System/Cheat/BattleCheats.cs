using UnityEngine;

namespace CryptoQuest.System.Cheat
{
    public class BattleCheats : MonoBehaviour, ICheatInitializer
    {
        public void InitCheats()
        {
            Debug.Log($"BattleCheats.InitCheats()");
        }
    }
}