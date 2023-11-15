using IndiGames.Core.SaveSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Language.Settings
{
    public class SerializeLocale : SerializableScriptableObject
    {
        [field: SerializeField] public Locale Locale { get; private set; }
    }
}