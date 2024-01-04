using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Map
{
    public class SceneManagerSO : ScriptableObject
    {
        [field: SerializeField] public SceneScriptableObject CurrentScene { get; set; }
    }
}