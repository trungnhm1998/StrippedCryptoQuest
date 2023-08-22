using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle;
using UnityEngine;

namespace CryptoQuest.Data
{
    [CreateAssetMenu(fileName = "BattleFieldsDatabase", menuName = "Data/BattleFieldsDatabase")]
    public class BattleFieldsDatabase : ScriptableObject
    {
        public List<BattleFieldSO> BattleFields;
    }
}