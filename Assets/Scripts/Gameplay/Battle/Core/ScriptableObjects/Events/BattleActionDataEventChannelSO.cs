using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using UnityEngine;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events
{
    [CreateAssetMenu(fileName = "BattleActionDataEventChannelSO", menuName = "Gameplay/Battle/Events/Battle Action Data Event")]
    public class BattleActionDataEventChannelSO : GenericEventChannelSO<BattleActionDataSO> { }
}