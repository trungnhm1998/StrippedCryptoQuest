using System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleVFX
{
    public class BattleVFXBehaviour : MonoBehaviour
    {
        public event Action CompleteVFX;

        protected void FinishVFX()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            CompleteVFX?.Invoke();
            CompleteVFX = null;
        }
    }
}