using UnityEngine;

namespace Indigames.AbilitySystem
{
    public class StatsInitializer : MonoBehaviour
    {
        [SerializeField] private AttributeSystem _attributeSystem;
        [SerializeField] private InitializeAttributeDatabase _database;
        public InitializeAttributeDatabase DefaultStats => _database;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_attributeSystem != null) return;
            _attributeSystem = GetComponent<AttributeSystem>();
        }
#endif

        private void Start()
        {
            InitStats();
        }

        public void InitStats()
        {
            InitStats(_database);
        }

        public void InitStats(InitializeAttributeDatabase stats)
        {
            _database = stats;
            AttributeScriptableObject[] attributes = new AttributeScriptableObject[stats.attributesToInitialize.Length];
            for (int i = 0; i < stats.attributesToInitialize.Length; i++)
            {
                attributes[i] = stats.attributesToInitialize[i].attribute;
            }
            _attributeSystem.AddAttributes(attributes);
            foreach (var initValue in stats.attributesToInitialize)
            {
                _attributeSystem.SetAttributeBaseValue(initValue.attribute, initValue.value);
            }

            _attributeSystem.UpdateAllAttributeCurrentValues();
        }
    }
}