using UnityEngine;
using System;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleVFX
{
    public class BattleVFXBehaviour : MonoBehaviour
    {
        public Action CompleteVFX;

        protected void FinishVFX()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            CompleteVFX?.Invoke();
        }
    }
}