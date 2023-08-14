using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleVFX;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using CryptoQuest.GameHandler;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Battle.Core.Components.ActionVFX
{
    public class LogAfterVFXHandler : GameHandler<object>
    {
        public VoidEventChannelSO ShowBattleLogSuccessEventChannel;
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

            var target = _actionData.Target;
            var vfxPos = (target == null)
                ? Vector3.zero
                : new Vector3(target.transform.position.x, 0, target.transform.position.z);
            _actionData.VFXPrefab.InstantiateAsync(vfxPos, Quaternion.identity).Completed += VFXPrefabAssetLoaded;
        }

        private void VFXPrefabAssetLoaded(AsyncOperationHandle<GameObject> handle)
        {
            var vfxObject = handle.Result;
            if (!vfxObject.TryGetComponent<BattleVFXBehaviour>(out var vfxBehaviour)) return;
            vfxBehaviour.Init(_actionData);
            vfxBehaviour.CompleteVFX += OnEffectComplete;
        }

        protected void OnEffectComplete()
        {
            ShowLog();
        }

        protected void ShowLog()
        {
            ShowBattleDialog?.Invoke(_actionData.Log);
        }

        public override void Handle()
        {
            ShowBattleLogSuccessEventChannel.EventRaised += OnShowBattleLogSuccess;
            LoadEffect();
        }

        private void OnShowBattleLogSuccess()
        {
            NextHandler?.Handle();
            ShowBattleDialog = null;
            ShowBattleLogSuccessEventChannel.EventRaised -= OnShowBattleLogSuccess;
        }
    }
}