using CryptoQuest.Gameplay.Battle.Core.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay
{
    [CreateAssetMenu(menuName = "Gameplay/Party SO")]
    public class PartySO : ScriptableObject
    {
        public BattleTeam PlayerTeam;
    }
}