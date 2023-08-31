using UnityEngine;

namespace CryptoQuest.System.Cheat
{
    public class CheatsInitializer : MonoBehaviour
    {
        public void InitCheats()
        {
            Debug.Log("Initialize cheats");
            var cheatInitializers = GetComponentsInChildren<ICheatInitializer>();
            foreach (var cheatInitializer in cheatInitializers)
            {
                cheatInitializer.InitCheats();
            }
        }
    }
}