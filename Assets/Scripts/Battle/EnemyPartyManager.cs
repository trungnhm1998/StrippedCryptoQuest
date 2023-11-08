using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public class EnemyPartyManager : MonoBehaviour
    {
        [field: SerializeField] public EnemyPartyBehaviour EnemyParty { get; private set; }
        public List<EnemyBehaviour> Enemies => EnemyParty.Enemies;
        public List<EnemyGroup> EnemyGroups => EnemyParty.EnemyGroups;
    }
}