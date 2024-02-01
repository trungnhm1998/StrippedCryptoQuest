using UnityEngine;

namespace CryptoQuest.System.Cheat
{
    public class CheatsInitializer : MonoBehaviour
    {
        public void InitCheats()
        {
#if DEVELOPMENT_BUILD || UNITY_EDITOR || ENABLE_CHEAT
            Debug.Log("Initialize cheats");
            var cheatInitializers = GetComponentsInChildren<ICheatInitializer>();
            foreach (var cheatInitializer in cheatInitializers)
            {
                cheatInitializer.InitCheats();
            }
#endif
        }
    }
}