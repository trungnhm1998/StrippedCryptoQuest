using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleVFX;
using UnityEngine;
using CryptoQuest.GameHandler;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Battle.Core.Components.ActionVFXHandler
{
    public class LogAfterVFXHandler : GameHandler<object>
    {
        public Action<LocalizedString> ShowBattleDialog;
        private BattleActionDataSO _actionData;

        public LogAfterVFXHandler(BattleActionDataSO actionData)
        {
            _actionData = actionData;
        }

        public virtual void LoadEffect()
        {
            if (!_actionData.VFXPrefab.RuntimeKeyIsValid())
            {
                OnEffectComplete();
                return;
            }
            _actionData.VFXPrefab.InstantiateAsync(Vector3.zero, Quaternion.identity).Completed += VFXPrefabAssetLoaded;
        }

        private void VFXPrefabAssetLoaded(AsyncOperationHandle<GameObject> gameObjectAsyncOps)
        {
            var vfxObject = gameObjectAsyncOps.Result;
            if (!vfxObject.TryGetComponent<BattleVFXBehaviour>(out var vfxBehaviour)) return;
            vfxBehaviour.CompleteVFX += OnEffectComplete;
        }

        protected void OnEffectComplete()
        {
            ShowLog();
            NextHandler?.Handle();
        }

        protected void ShowLog()
        {
            ShowBattleDialog?.Invoke(_actionData.Log);
        }

        public override void Handle()
        {
            LoadEffect();
        }
    }
}