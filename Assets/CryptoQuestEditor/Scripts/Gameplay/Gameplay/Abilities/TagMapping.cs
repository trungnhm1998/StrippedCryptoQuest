using System;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Gameplay.Abilities
{
    [CreateAssetMenu(menuName = "Crypto Quest/Ability System/Abilities/Tag Mapping",
        fileName = "TagMapping")]
    public class TagMapping : ScriptableObject
    {
        public List<TagBuffMap> TagBuffMaps = new();
    }

    [Serializable]
    public class TagBuffMap
    {
        public string Id;
        public TagScriptableObject BuffTag;
        public TagScriptableObject DebuffTag;
    }
}