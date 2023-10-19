using CryptoQuest.Gameplay;
using CryptoQuest.System;
using CryptoQuest.UI.Dialogs.BattleDialog;
using IndiGames.Core.Events.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Map.CheckPoint
{
    public class SpawnManagerWithCheckPoint : SpawnManager
    {
        private ICheckPointController _checkPointController;
        private UIGenericDialog _dialog;
        [SerializeField] private LocalizedString _revivalMessage;
        [SerializeField] private VoidEventChannelSO _showCheckPointMessageSO;

        protected override void OnAwake()
        {
            GenericDialogController.Instance.Instantiate(OnDialogLoaded, false);
            _showCheckPointMessageSO.EventRaised += ShowCheckPointMessage;
        }

        private void OnDestroy()
        {
            GenericDialogController.Instance.Release(_dialog);
            _showCheckPointMessageSO.EventRaised -= ShowCheckPointMessage;
        }

        protected override void SpawnPlayer()
        {
            _checkPointController = ServiceProvider.GetService<ICheckPointController>();
            if (_checkPointController.IsBackToCheckPoint)
            {
                _checkPointController.FinishBackToCheckPoint();

                var heroInstance = Instantiate(_heroPrefab, _checkPointController.CheckPointPosition, Quaternion.identity);
                heroInstance.SetFacingDirection(_checkPointController.FacingDirection);

                _gameplayBus.Hero = heroInstance;
                _gameplayBus.RaiseHeroSpawnedEvent();
                ShowCheckPointMessage();
            }
            else
            {
                base.SpawnPlayer();
            }    
        }

        private void OnDialogLoaded(UIGenericDialog dialog)
        {
            _dialog = dialog;
        }    

        private void EnableMapInput()
        {
            _inputMediator.EnableMapGameplayInput();
        } 
        
        private void ShowCheckPointMessage()
        {
            _dialog.WithMessage(_revivalMessage).RequireInput().WithHideCallback(EnableMapInput).Show();
        }    
    }
}
