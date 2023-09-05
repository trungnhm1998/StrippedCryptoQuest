using System;
using CryptoQuest.Gameplay.PlayerParty;
using UnityEngine;

namespace CryptoQuest.Gameplay.Encounter
{
    [CreateAssetMenu(menuName = "Gameplay/Battle/Battle Data")]
    public class EnemyParty : ScriptableObject
    {
        [field: SerializeField] public int Id { get; private set; }
        [SerializeField] private int[] _enemyIds = Array.Empty<int>();
        public int[] EnemyIds => _enemyIds;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_enemyIds.Length > PartyConstants.MAX_PARTY_SIZE)
                Array.Resize(ref _enemyIds, PartyConstants.MAX_PARTY_SIZE);
        }

        public void Editor_SetId(int id)
        {
            Id = id;
        }

        public void Editor_SetEnemyGroups(int[] enemies)
        {
            _enemyIds = enemies;
        }
#endif
    }
}