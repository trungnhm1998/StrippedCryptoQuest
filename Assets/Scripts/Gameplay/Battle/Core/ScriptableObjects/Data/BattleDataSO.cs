using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data
{
    [CreateAssetMenu(menuName = "Gameplay/Battle/Battle Data")]
    public class BattleDataSO : ScriptableObject
    {
        public CharacterDataSO[] Enemies;
        public bool IsEscapable;
    }
}