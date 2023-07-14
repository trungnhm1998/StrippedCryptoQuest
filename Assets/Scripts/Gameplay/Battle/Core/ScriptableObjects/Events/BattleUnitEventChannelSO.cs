using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(fileName = "BattleUnitEventChannelSO", menuName = "Gameplay/Battle/Events/Battle Unit Event")]
    public class BattleUnitEventChannelSO : ScriptableObject
    {
        public UnityAction<IBattleUnit> EventRaised;

        public void RaiseEvent(IBattleUnit unit)
        {
            if (EventRaised == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return;
            }

            EventRaised.Invoke(unit);
        }
    }
}