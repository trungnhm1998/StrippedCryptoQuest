using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.AttributeSystem.Components
{
    public class StatsInitializer : MonoBehaviour, IStatInitializer
    {
        [SerializeField] private AttributeSystemBehaviour _attributeSystem;
        [SerializeField] private AttributeWithValue[] _database;
        [SerializeField] private bool _initOnStart = false;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_attributeSystem != null) return;
            _attributeSystem = GetComponent<AttributeSystemBehaviour>();
        }
#endif

        private void Start()
        {
            if (_initOnStart) InitStats();
        }

        public void InitStats()
        {
            for (int i = 0; i < _database.Length; i++)
            {
                var initValue = _database[i];
                _attributeSystem.AddAttribute(initValue.Attribute);
                _attributeSystem.SetAttributeBaseValue(initValue.Attribute, initValue.Value);
            }

            _attributeSystem.UpdateAttributeValues();
        }
    }
}