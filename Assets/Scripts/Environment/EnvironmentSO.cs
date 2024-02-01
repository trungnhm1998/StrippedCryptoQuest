using UnityEngine;

namespace CryptoQuest.Environment
{
    public class EnvironmentSO : ScriptableObject
    {
        public string API =>
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            "https://dev-api-game.crypto-quest.org/v1";
#elif STAGING_BUILD
            "https://stg-api-game.crypto-quest.org/v1";
#elif PRODUCTION_BUILD
            "https://dev-api-game.crypto-quest.org/v1";
#else
            "https://dev-api-game.crypto-quest.org/v1";
#endif
    }
}