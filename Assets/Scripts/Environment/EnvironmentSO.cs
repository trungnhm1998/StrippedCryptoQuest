using UnityEngine;

namespace CryptoQuest.Environment
{
    public class EnvironmentSO : ScriptableObject
    {
        [field: SerializeField] public string BackEndUrl { get; private set; } = "http://dev.api-game.crypto-quest.org:3000/v1";
        [field: SerializeField] public string DEBUG_TOKEN { get; private set; } = "c1CRi-qi8jfOJHJ5rjH2tO9xjSA_UUORQ1eRBt59BY8.sc6AO3PQnOrQV0hG4SoQ6mTeU8r1n4-WKuCuzrpnmw1";
        [field: SerializeField] public string DEBUG_KEY { get; private set; } = "GQwuFb5HYRrbodgHmlyeJPXYDfRUpxkOZrFlWarb";
    }
}