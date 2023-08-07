using System;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.UI.Menu.Panels.Status.Stats
{
    public class UIStats : MonoBehaviour
    {
        public class Equipment
        {
            public Dictionary<AttributeScriptableObject, float> ModifiedAttributes = new();
        }

        [Header("Mock")]
        [SerializeField] private List<AttributeScriptableObject> _modifiedAttributes;
        [SerializeField] private List<float> _modifiedAttributesValue;

        [Space]
        [SerializeField] private List<UIAttribute> _uiAttributes = new List<UIAttribute>();

        private Dictionary<AttributeScriptableObject, UIAttribute> _temp = new();

        private void Awake()
        {
            foreach (var uiAttribute in _uiAttributes)
            {
                _temp.Add(uiAttribute.Attribute, uiAttribute);
            }
        }

        private void OnEnable()
        {
            UIStatusInventoryItemButton.InspectingEquipment += PreviewStats;
        }

        private void OnDisable()
        {
            UIStatusInventoryItemButton.InspectingEquipment -= PreviewStats;
        }

        private void InitMockData()
        {
            var mockEquipment = new Equipment();
            for (var index = 0; index < _modifiedAttributes.Count; index++)
            {
                var attribute = _modifiedAttributes[index];
                mockEquipment.ModifiedAttributes.Add(attribute, _modifiedAttributesValue[index]);
            }

            PreviewStats(mockEquipment);
        }

        public void PreviewStats(Equipment equipment)
        {
            foreach (var modifedAttribute in equipment.ModifiedAttributes)
            {
                if (!_temp.TryGetValue(modifedAttribute.Key, out UIAttribute ui))
                {
                    continue;
                }

                ui.CompareValue(modifedAttribute.Value);
            }
        }
    }
}