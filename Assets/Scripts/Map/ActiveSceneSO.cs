using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Map
{
    [CreateAssetMenu(fileName = "Active Scene", menuName = "Crypto Quest/Map/Active Scene")]
    public class ActiveSceneSO : ScriptableObject
    {
        public SceneScriptableObject CurrentActiveMapScene;
    }
}