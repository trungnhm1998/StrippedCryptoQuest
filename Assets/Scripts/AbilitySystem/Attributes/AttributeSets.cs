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
        public static AttributeScriptableObject Vitality;
        public static AttributeScriptableObject Intelligence;
        public static AttributeScriptableObject CriticalRate;
        public static AttributeScriptableObject FireAttack;
        public static AttributeScriptableObject WaterAttack;
        public static AttributeScriptableObject EarthAttack;
        public static AttributeScriptableObject WindAttack;
        public static AttributeScriptableObject WoodAttack;
        public static AttributeScriptableObject LightAttack;
        public static AttributeScriptableObject DarkAttack;
        public static AttributeScriptableObject FireResist;
        public static AttributeScriptableObject WaterResist;
        public static AttributeScriptableObject EarthResist;
        public static AttributeScriptableObject WindResist;
        public static AttributeScriptableObject WoodResist;
        public static AttributeScriptableObject LightResist;
        public static AttributeScriptableObject DarkResist;


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
        [SerializeField] private AttributeScriptableObject _vitality;
        [SerializeField] private AttributeScriptableObject _intelligence;

        [Header("Elemental attack Attribute")]
        [SerializeField] private AttributeScriptableObject _fireAttack;

        [SerializeField] private AttributeScriptableObject _waterAttack;
        [SerializeField] private AttributeScriptableObject _earthAttack;
        [SerializeField] private AttributeScriptableObject _windAttack;
        [SerializeField] private AttributeScriptableObject _woodAttack;
        [SerializeField] private AttributeScriptableObject _lightAttack;
        [SerializeField] private AttributeScriptableObject _darkAttack;

        [Header("Elemental resist Attribute")]
        [SerializeField] private AttributeScriptableObject _fireResist;

        [SerializeField] private AttributeScriptableObject _waterResist;
        [SerializeField] private AttributeScriptableObject _earthResist;
        [SerializeField] private AttributeScriptableObject _windResist;
        [SerializeField] private AttributeScriptableObject _woodResist;
        [SerializeField] private AttributeScriptableObject _lightResist;
        [SerializeField] private AttributeScriptableObject _darkResist;

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
            Vitality = _vitality;
            Intelligence = _intelligence;
            CriticalRate = _criticalRate;
            FireAttack = _fireAttack;
            WaterAttack = _waterAttack;
            EarthAttack = _earthAttack;
            WindAttack = _windAttack;
            WoodAttack = _woodAttack;
            LightAttack = _lightAttack;
            DarkAttack = _darkAttack;
            FireResist = _fireResist;
            WaterResist = _waterResist;
            EarthResist = _earthResist;
            WindResist = _windResist;
            WoodResist = _woodResist;
            LightResist = _lightResist;
            DarkResist = _darkResist;
        }
    }
}