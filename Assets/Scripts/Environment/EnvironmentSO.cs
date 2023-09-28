using UnityEngine;

namespace CryptoQuest.Environment
{
    public class EnvironmentSO : ScriptableObject
    {
        [field: SerializeField] public string BackEndUrl { get; private set; } = "http://dev.api-game.crypto-quest.org:3000/v1";
    }
}