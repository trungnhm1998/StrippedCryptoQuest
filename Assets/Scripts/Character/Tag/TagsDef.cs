using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Character.Tag
{
    public class TagsDef : ScriptableObject
    {
        public static TagScriptableObject Abnormal;
        public static TagScriptableObject Dead;
        public static TagScriptableObject Guard;

        [field: SerializeField] public TagScriptableObject AbnormalTag { get; private set; }
        [field: SerializeField] public TagScriptableObject DeadTag { get; private set; }
        [field: SerializeField] public TagScriptableObject GuardTag { get; private set; }

        private void OnEnable()
        {
            Dead = DeadTag;
            Guard = GuardTag;
            Abnormal = AbnormalTag;
        }
    }
}