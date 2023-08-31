using System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(menuName = "Gameplay/Battle/Battle Data")]
    public class EncounterGroups : ScriptableObject
    {
        [Serializable]
        public struct CharacterGroup
        {
            [field: SerializeField] public CharacterDataSO[] Characters { get; private set; }
#if UNITY_EDITOR
            public void Editor_SetCharacters(CharacterDataSO[] characters)
            {
                Characters = characters;
            }
#endif
        }

        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public CharacterGroup[] EnemyGroups { get; private set; }

#if UNITY_EDITOR
        public void Editor_SetId(int id)
        {
            Id = id;
        }

        public void Editor_SetEnemyGroups(CharacterGroup[] group)
        {
            EnemyGroups = group;
        }
#endif
    }
}