using UnityEngine;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using System;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleVFX
{
    public class BattleVFXBehaviour : MonoBehaviour
    {
        public Action CompleteVFX;

        protected BattleActionDataSO _actionData;

        public virtual void Init(BattleActionDataSO data)
        {
            _actionData = data;
        }

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