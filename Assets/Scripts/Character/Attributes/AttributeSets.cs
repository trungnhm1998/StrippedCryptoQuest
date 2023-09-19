using UnityEngine;

namespace CryptoQuest.Character.Attributes
{
    public class AttributeSets : ScriptableObject
    {
        public static AttributeScriptableObject MaxHealth;
        public static AttributeScriptableObject Health;
        public static AttributeScriptableObject Attack;
        public static AttributeScriptableObject Strength;
        public static AttributeScriptableObject Defense;
        public static AttributeScriptableObject Agility;

        [SerializeField] private AttributeScriptableObject _maxHealth;
        [SerializeField] private AttributeScriptableObject _health;
        [SerializeField] private AttributeScriptableObject _attack;
        [SerializeField] private AttributeScriptableObject _strength;
        [SerializeField] private AttributeScriptableObject _defense;
        [SerializeField] private AttributeScriptableObject _agility;

        private void OnEnable()
        {
            MaxHealth = _maxHealth;
            Health = _health;
            Attack = _attack;
            Strength = _strength;
            Defense = _defense;
            Agility = _agility;
        }
    }
}