using UnityEngine;

namespace CryptoQuest.System.SceneManagement
{
    [CreateAssetMenu(fileName = "SceneLoaderBus", menuName = "Gameplay/SceneLoaderBus", order = 1)]
    public class SceneLoaderBus : ScriptableObject
    {
        public SceneLoaderHandler SceneLoader;
    }
}