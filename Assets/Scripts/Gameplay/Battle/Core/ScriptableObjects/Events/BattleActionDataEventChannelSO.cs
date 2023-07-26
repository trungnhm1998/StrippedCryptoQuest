using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using UnityEngine;
using UnityEngine.Events;
using CryptoQuest.Events;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events
{
    [CreateAssetMenu(fileName = "BattleActionDataEventChannelSO", menuName = "Gameplay/Battle/Events/Battle Action Data Event")]
    public class BattleActionDataEventChannelSO : ScriptableObject
    {
        public UnityAction<BattleActionDataSO> EventRaised;

        public void RaiseEvent(BattleActionDataSO data)
        {
            this.CallEventSafely<BattleActionDataSO>(EventRaised, data);
        }
    }
}