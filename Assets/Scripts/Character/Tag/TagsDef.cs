using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Character.Tag
{
    public class TagsDef : ScriptableObject
    {
        public static TagScriptableObject Dead;

        [field: SerializeField] public TagScriptableObject DeadTag { get; private set; }

        private void OnEnable()
        {
            Dead = DeadTag;
        }
    }
}