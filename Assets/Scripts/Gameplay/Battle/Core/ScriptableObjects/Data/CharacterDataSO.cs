using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(fileName = "CharacterDataSO", menuName = "Gameplay/Character/Character Data")]
    public class CharacterDataSO : InitializeAttributeDatabase
    {
        public string Name;
        public Sprite Sprite;
        public List<AbilityScriptableObject> GrantedSkills = new();
    }
}
