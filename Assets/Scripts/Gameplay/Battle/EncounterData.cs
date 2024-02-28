using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Encounter;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(menuName = "Gameplay/Battle/Battle Field")]
    public class EncounterData : ScriptableObject
    {
        [Serializable]
        public struct GroupConfig
        {
            public float Probability;
            public Battlefield Battlefield;
        }

        [field: SerializeField] public string ID { get; private set; }

        [SerializeField] private List<GroupConfig> _configs;
        public List<GroupConfig> Configs => _configs;
        [SerializeField] private float _encounterRate = 5f;
        public float EncounterRate => _encounterRate;
        [SerializeField] private bool _isEscapable = true;
        public bool IsEscapable => _isEscapable;
        [field: SerializeField] public AssetReferenceT<Sprite> Background { get; private set; }

#if UNITY_EDITOR
        public void Editor_SetID(string id)
        {
            ID = id;
        }

        public void Editor_SetBackground(AssetReferenceT<Sprite> sprite)
        {
            Background = sprite;
        }

        public void Editor_SetGroup(List<GroupConfig> groupConfigs)
        {
            _configs = groupConfigs;
        }

        public void Editor_SetConfig(List<GroupConfig> config)
        {
            _configs = config;
        }
#endif

        /// <summary>
        /// Based on probability, get a battle data
        /// </summary>
        /// <returns>a Battlefield based on <see cref="GroupConfig.Probability"/></returns>
        public Battlefield GetRandomBattlefield()
        {
            float randomValue = Random.value;
            GroupConfig selectedBattle = _configs[^1];
            for (int i = 0; i < _configs.Count; i++)
            {
                randomValue -= _configs[i].Probability;
                if (randomValue <= 0)
                {
                    selectedBattle = _configs[i];
                    break;
                }
            }

            return selectedBattle.Battlefield;
        }
    }
}