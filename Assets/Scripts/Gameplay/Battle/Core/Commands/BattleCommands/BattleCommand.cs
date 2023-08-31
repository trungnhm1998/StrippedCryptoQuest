using System;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleVFX;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Gameplay.Battle.Core.Commands.BattleCommands
{
    /// <summary>
    /// A BattleCommand can contain command data that has log and vfx infomation
    /// And when BattleCommand execute it'll show log or VFX or both base on implementation 
    /// </summary>
    public abstract class BattleCommand : ICommand
    {
        public Action<LocalizedString> ShowDialog;
        protected BattleActionDataSO _commandData;
        public Action FinishedCommand;

        public BattleCommand(BattleActionDataSO data)
        {
            _commandData = data;
            FinishedCommand += CleanUp;
        }
        
        public abstract void Execute();

        protected virtual void CleanUp()
        {
            Debug.Log($"Finished {this}");
            FinishedCommand = null;
            ShowDialog = null;
        }

        protected virtual void LoadVFX()
        {
            if (!_commandData.VFXPrefab.RuntimeKeyIsValid())
            {
                VFXFinished();
                return;
            }

            // TODO: Might pass target in command directly or using target from battle unit
            var target = _commandData.Target;
            var vfxPos = (target == null) ? Vector3.zero
                : new Vector3(target.transform.position.x, 0, target.transform.position.z);
            _commandData.VFXPrefab.InstantiateAsync(vfxPos, Quaternion.identity).Completed += VFXPrefabAssetLoaded;
        }

        protected virtual void VFXPrefabAssetLoaded(AsyncOperationHandle<GameObject> handle)
        {
            var vfxObject = handle.Result;
            if (!vfxObject.TryGetComponent<BattleVFXBehaviour>(out var vfxBehaviour)) return;
            vfxBehaviour.Init(_commandData);
            vfxBehaviour.CompleteVFX += VFXFinished;
        }

        /// <summary>
        /// Will be called after the VFX is performed
        /// </summary>
        protected virtual void VFXFinished() { }

        protected virtual void ShowLog()
        {
            ShowDialog?.Invoke(_commandData.Log);
        }
    }
}