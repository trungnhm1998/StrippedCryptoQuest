using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleVFX
{
    public class DemoBattleVFXBehaviour : BattleVFXBehaviour
    {
        [SerializeField] private float _waitTime = 1f;

        private void Start()
        {
            Debug.Log($"Showing VFX {gameObject} in {_waitTime}s");
            Invoke(nameof(FinishVFX), _waitTime);
        }
    }
}