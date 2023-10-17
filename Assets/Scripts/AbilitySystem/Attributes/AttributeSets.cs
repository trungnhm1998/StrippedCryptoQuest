using UnityEngine;

namespace CryptoQuest.AbilitySystem.Attributes
{
    public class AttributeSets : ScriptableObject
    {
        public static AttributeScriptableObject MaxHealth;
        public static AttributeScriptableObject Health;
        public static AttributeScriptableObject MaxMana;
        public static AttributeScriptableObject Mana;
        public static AttributeScriptableObject Attack;
        public static AttributeScriptableObject MagicAttack;
        public static AttributeScriptableObject MagicValue;
        public static AttributeScriptableObject Strength;
        public static AttributeScriptableObject Defense;
        public static AttributeScriptableObject Agility;

        [SerializeField] private AttributeScriptableObject _maxHealth;
        [SerializeField] private AttributeScriptableObject _health;
        [SerializeField] private AttributeScriptableObject _mana;
        [SerializeField] private AttributeScriptableObject _maxMana;
        [SerializeField] private AttributeScriptableObject _attack;
        [SerializeField] private AttributeScriptableObject _magicAttack;
        [SerializeField] private AttributeScriptableObject _magicValue;
        [SerializeField] private AttributeScriptableObject _strength;
        [SerializeField] private AttributeScriptableObject _defense;
        [SerializeField] private AttributeScriptableObject _agility;

        private void OnEnable()
        {
            MaxHealth = _maxHealth;
            Health = _health;
            MaxMana = _maxMana;
            Mana = _mana;
            Attack = _attack;
            MagicAttack = _magicAttack;
            MagicValue = _magicValue;
            Strength = _strength;
            Defense = _defense;
            Agility = _agility;
        }
    }
}