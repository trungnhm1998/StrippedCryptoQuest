using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.PlayerParty;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Gameplay.Encounter
{
    [CreateAssetMenu(menuName = "Gameplay/Battle/Battle Data")]
    public class Battlefield : ScriptableObject
    {
        [field: SerializeField] public int Id { get; private set; }
        [SerializeField] private bool _canRetreat = true;
        public bool CanRetreat => _canRetreat;

        [SerializeField] private EnemyGroupId[] _enemyGroups = Array.Empty<EnemyGroupId>();
        public EnemyGroupId[] EnemyGroups => _enemyGroups;

        [SerializeField, ReadOnly] private int[] _enemyIds = Array.Empty<int>();
        public int[] EnemyIds
        {
            get
            {
                if (_enemyIds.Length <= 0)
                    _enemyIds = _enemyGroups.SelectMany(g => g.EnemyIds).ToArray();
                return _enemyIds;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (EnemyIds.Length > PartyConstants.MAX_PARTY_SIZE)
            {
                Debug.LogWarning($"Enemies lenght {EnemyIds.Length}"
                    + $"is higher that max party size {PartyConstants.MAX_PARTY_SIZE}");
            }
        }

        public void Editor_SetId(int id)
        {
            Id = id;
        }

        public void Editor_SetEnemyGroups(List<int[]> enemyGroups)
        {
            _enemyGroups = new EnemyGroupId[enemyGroups.Count];
            for (int i = 0; i < _enemyGroups.Length; i++)
            {
                var enemyGroup = new EnemyGroupId()
                {
                    EnemyIds = enemyGroups[i]
                };
                _enemyGroups[i] = enemyGroup;
            }
        }

        public void Editor_SetRetreat(bool value)
        {
            _canRetreat = value;
        }
#endif
    }

    [Serializable]
    public struct EnemyGroupId
    {
        public int[] EnemyIds;
    }
}