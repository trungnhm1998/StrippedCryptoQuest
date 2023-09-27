using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.CommandDetail;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public class EnemyPartyManager : MonoBehaviour
    {
        [field: SerializeField] public EnemiesPresenter EnemiesPresenter { get; private set; }
        [field: SerializeField] public EnemyPartyBehaviour EnemyParty { get; private set; }
        public List<EnemyBehaviour> Enemies => EnemyParty.Enemies;
    }
}