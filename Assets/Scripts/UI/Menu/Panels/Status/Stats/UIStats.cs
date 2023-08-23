using System;
using System.Collections.Generic;
using CryptoQuest.UI.Menu.Panels.Status.Equipment;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Status.Stats
{
    public class UIStats : MonoBehaviour
    {
        public class Equipment
        {
            public Dictionary<AttributeScriptableObject, float> ModifiedAttributes = new();
        }

        [Header("Attribute keys")]
        [SerializeField] private List<AttributeScriptableObject> _modifiedAttributes;

        [Header("UI references")]
        [SerializeField] private List<UIAttribute> _uiAttributes = new List<UIAttribute>();
        [SerializeField] private TMP_Text _currentHp;
        [SerializeField] private TMP_Text _maxHp;
        [SerializeField] private Image _HpBar;
        [SerializeField] private TMP_Text _currentMp;
        [SerializeField] private TMP_Text _maxMp;
        [SerializeField] private Image _MpBar;
        [SerializeField] private TMP_Text _currentExp;
        [SerializeField] private TMP_Text _maxExp;
        [SerializeField] private Image _ExpBar;

        private Dictionary<AttributeScriptableObject, UIAttribute> _targetAttributes = new();

        private void Awake()
        {
            foreach (var uiAttribute in _uiAttributes)
            {
                _targetAttributes.Add(uiAttribute.Attribute, uiAttribute);
            }
        }

        private void OnEnable()
        {
            UIEquipmentItem.InspectingEquipment += PreviewStats;
        }

        private void OnDisable()
        {
            UIEquipmentItem.InspectingEquipment -= PreviewStats;
        }

        private void PreviewStats(Equipment equipment)
        {
            foreach (var modifedAttribute in equipment.ModifiedAttributes)
            {
                if (!_targetAttributes.TryGetValue(modifedAttribute.Key, out UIAttribute ui))
                {
                    continue;
                }

                ui.CompareValue(modifedAttribute.Value);
            }
        }

        public void SetAttributes(AttributeSystemBehaviour characterASB)
        {
            foreach (var initAttribute in _modifiedAttributes)
            {
                characterASB.TryGetAttributeValue(initAttribute, out var attribute);

                if (!_targetAttributes.TryGetValue(initAttribute, out UIAttribute ui))
                    continue;

                ui.Init(attribute.CurrentValue);
            }
        }
    }
}