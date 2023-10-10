using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Character.Tag
{
    public class TagsDef : ScriptableObject
    {
        public static TagScriptableObject Abnormal;
        public static TagScriptableObject Dead;
        public static TagScriptableObject Guard;
        public static TagScriptableObject CrowdControl;
        public static TagScriptableObject DoT;
        public static TagScriptableObject Buff;
        public static TagScriptableObject DeBuff;

        [field: SerializeField] public TagScriptableObject AbnormalTag { get; private set; }
        [field: SerializeField] public TagScriptableObject DeadTag { get; private set; }
        [field: SerializeField] public TagScriptableObject GuardTag { get; private set; }
        [field: SerializeField] public TagScriptableObject CrowdControlTag { get; private set; }
        [field: SerializeField] public TagScriptableObject DoTTag { get; private set; }
        [field: SerializeField] public TagScriptableObject BuffTag { get; private set; }
        [field: SerializeField] public TagScriptableObject DeBuffTag { get; private set; }

        private void OnEnable()
        {
            Dead = DeadTag;
            Guard = GuardTag;
            Abnormal = AbnormalTag;
            CrowdControl = CrowdControlTag;
            DoT = DoTTag;
            Buff = BuffTag;
            DeBuff = DeBuffTag;
        }
    }
}