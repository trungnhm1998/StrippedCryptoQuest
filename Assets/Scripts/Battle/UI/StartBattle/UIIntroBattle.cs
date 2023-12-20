using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace CryptoQuest.Battle.UI.StartBattle
{
    public class UIIntroBattle : MonoBehaviour
    {
        [field: SerializeField, FormerlySerializedAs("_battleAppearPrompt")]
        public LocalizedString IntroMessage { get; set; }

        [field: SerializeField, FormerlySerializedAs("_duration")]
        public float Duration { get; private set; } = 3f;
    }
}