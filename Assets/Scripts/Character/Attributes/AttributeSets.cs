using UnityEngine;

namespace CryptoQuest.Character.Attributes
{
    public class AttributeSets : ScriptableObject
    {
        public static AttributeScriptableObject Health;
        public static AttributeScriptableObject Attack;

        [SerializeField] private AttributeScriptableObject _health;
        [SerializeField] private AttributeScriptableObject _attack;

        private void OnEnable()
        {
            Health = _health;
            Attack = _attack;
        }
    }
}