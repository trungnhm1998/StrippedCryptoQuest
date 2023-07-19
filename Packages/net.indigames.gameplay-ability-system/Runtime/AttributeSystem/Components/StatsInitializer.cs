using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.AttributeSystem.Components
{
    public class StatsInitializer : MonoBehaviour
    {
        [SerializeField] private AttributeSystemBehaviour _attributeSystem;
        [SerializeField] private InitializeAttributeDatabase _database;
        public InitializeAttributeDatabase DefaultStats => _database;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_attributeSystem != null) return;
            _attributeSystem = GetComponent<AttributeSystemBehaviour>();
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
            if (stats == null) return;
            _database = stats;
            for (int i = 0; i < stats.AttributesToInitialize.Length; i++)
            {
                _attributeSystem.AddAttributes(stats.AttributesToInitialize[i].Attribute);
            }
            foreach (var initValue in stats.AttributesToInitialize)
            {
                _attributeSystem.SetAttributeBaseValue(initValue.Attribute, initValue.Value);
            }

            _attributeSystem.UpdateAllAttributeCurrentValues();
        }
    }
}