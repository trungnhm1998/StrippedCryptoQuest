using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(fileName = "CharacterDataSO", menuName = "Gameplay/Character/Character Data")]
    public class CharacterDataSO : InitializeAttributeDatabase
    {
        public string Name;
        public Sprite BattleIconSprite;
        public AbilityScriptableObject NormalAttack;
        public List<AbilityScriptableObject> GrantedSkills;

        private string _displayName = "";
        public string DisplayName {
            get
            {
                return _displayName == "" ? Name : _displayName;
            }

            set
            {
                _displayName = value;
            }
        }

        public AbilitySystemBehaviour Owner { get; set; }
    }
}
