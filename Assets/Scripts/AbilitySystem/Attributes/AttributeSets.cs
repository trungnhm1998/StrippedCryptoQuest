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
        public static AttributeScriptableObject Strength;
        public static AttributeScriptableObject Defense;
        public static AttributeScriptableObject Agility;
        public static AttributeScriptableObject CriticalRate;

        [SerializeField] private AttributeScriptableObject _maxHealth;
        [SerializeField] private AttributeScriptableObject _health;
        [SerializeField] private AttributeScriptableObject _mana;
        [SerializeField] private AttributeScriptableObject _maxMana;
        [SerializeField] private AttributeScriptableObject _attack;
        [SerializeField] private AttributeScriptableObject _magicAttack;
        [SerializeField] private AttributeScriptableObject _strength;
        [SerializeField] private AttributeScriptableObject _defense;
        [SerializeField] private AttributeScriptableObject _agility;
        [SerializeField] private AttributeScriptableObject _criticalRate;

        private void OnEnable()
        {
            MaxHealth = _maxHealth;
            Health = _health;
            MaxMana = _maxMana;
            Mana = _mana;
            Attack = _attack;
            MagicAttack = _magicAttack;
            Strength = _strength;
            Defense = _defense;
            Agility = _agility;
            CriticalRate = _criticalRate;
        }
    }
}