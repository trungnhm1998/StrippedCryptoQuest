using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem
{
    public class TagsDef : ScriptableObject
    {
        public static TagScriptableObject Abnormal { get; private set; }
        public static TagScriptableObject Dead { get; private set; }
        public static TagScriptableObject Guard { get; private set; }
        public static TagScriptableObject CrowdControl { get; private set; }
        public static TagScriptableObject DamageOverTime { get; private set; }
        public static TagScriptableObject Buff { get; private set; }
        public static TagScriptableObject DeBuff { get; private set; }
        public static TagScriptableObject DamageOnce { get; private set; }
        public static TagScriptableObject SealMagic { get; private set; }
        public static TagScriptableObject SealPhysic { get; private set; }
        public static TagScriptableObject DisableActionInfinity { get; private set; }

        [field: SerializeField] public TagScriptableObject AbnormalTag { get; private set; }
        [field: SerializeField] public TagScriptableObject DeadTag { get; private set; }
        [field: SerializeField] public TagScriptableObject GuardTag { get; private set; }
        [field: SerializeField] public TagScriptableObject CrowdControlTag { get; private set; }
        [field: SerializeField] public TagScriptableObject DoTTag { get; private set; }
        [field: SerializeField] public TagScriptableObject DamageOnceTag { get; private set; }
        [field: SerializeField] public TagScriptableObject BuffTag { get; private set; }
        [field: SerializeField] public TagScriptableObject DeBuffTag { get; private set; }
        [field: SerializeField] public TagScriptableObject SealMagicTag { get; private set; }
        [field: SerializeField] public TagScriptableObject SealPhysicTag { get; private set; }
        [field: SerializeField] public TagScriptableObject DisableActionInfinityTag { get; private set; }

        private void OnEnable()
        {
            Dead = DeadTag;
            Guard = GuardTag;
            Abnormal = AbnormalTag;
            CrowdControl = CrowdControlTag;
            DamageOverTime = DoTTag;
            Buff = BuffTag;
            DeBuff = DeBuffTag;
            DamageOnce = DamageOnceTag;
            SealMagic = SealMagicTag;
            SealPhysic = SealPhysicTag;
            DisableActionInfinity = DisableActionInfinityTag;
        }
    }
}