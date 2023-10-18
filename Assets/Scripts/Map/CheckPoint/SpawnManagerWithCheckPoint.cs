using CryptoQuest.Gameplay;
using CryptoQuest.System;
using CryptoQuest.UI.Dialogs.BattleDialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Map.CheckPoint
{
    public class SpawnManagerWithCheckPoint : SpawnManager
    {
        private ICheckPointController _checkPointController;
        [SerializeField] private LocalizedString _revivalMessage;
        protected override void SpawnPlayer()
        {
            _checkPointController = ServiceProvider.GetService<ICheckPointController>();
            if (_checkPointController.IsBackToCheckPoint)
            {
                GenericDialogController.Instance.Instantiate(OnDialogLoaded, true);
            }
            else
            {
                base.SpawnPlayer();
            }    
        }

        private void OnDialogLoaded(UIGenericDialog dialog)
        {
            _checkPointController.FinishBackToCheckPoint();

            var heroInstance = Instantiate(_heroPrefab, _checkPointController.CheckPointPosition, Quaternion.identity);
            heroInstance.SetFacingDirection(_checkPointController.FacingDirection);

            _gameplayBus.Hero = heroInstance;
            _gameplayBus.RaiseHeroSpawnedEvent();

            dialog.WithMessage(_revivalMessage).RequireInput().WithHideCallback(EnableMapInput).Show();
        }    

        private void EnableMapInput()
        {
            _inputMediator.EnableMapGameplayInput();
        }    
    }
}
