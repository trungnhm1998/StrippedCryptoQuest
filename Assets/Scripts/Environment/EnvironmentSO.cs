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

        public string PKEY =>
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            "AZRlaSzG6T7sROZedUL9VHle0JEim7Gr";
#elif STAGING_BUILD
            "cUXLWm4pMkqbcDU7YtoTlq65CZSn5PHF";
#elif PRODUCTION_BUILD
            "AZRlaSzG6T7sROZedUL9VHle0JEim7Gr";
#else
            "AZRlaSzG6T7sROZedUL9VHle0JEim7Gr";
#endif
    }
}