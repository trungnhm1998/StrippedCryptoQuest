using System.Collections;
using CryptoQuest.UI.Dialogs.BattleDialog;
using CryptoQuest.Character.Behaviours;
using IndiGames.Core.Common;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;
using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Gameplay;

namespace CryptoQuest.Map.CheckPoint
{
    public class SpawnManagerWithCheckPoint : SpawnManager
    {
        private ICheckPointController _checkPointController;
        private UIGenericDialog _dialog;
        [SerializeField] private LocalizedString _revivalMessage;
        [SerializeField] private VoidEventChannelSO _showCheckPointMessageSO;
        [SerializeField] private GameStateSO _gameState;

        protected override void OnAwake()
        {
            _showCheckPointMessageSO.EventRaised += ShowCheckPointMessage;
        }

        private void OnDestroy()
        {
            _showCheckPointMessageSO.EventRaised -= ShowCheckPointMessage;
            GenericDialogController.Instance.Release(_dialog);
        }

        protected override void SpawnPlayer()
        {
            _checkPointController = ServiceProvider.GetService<ICheckPointController>();
            if (_checkPointController.IsBackToCheckPoint)
            {
                _checkPointController.FinishBackToCheckPoint();

                var position = _checkPointController.CheckPointPosition;

                if (position == Vector3.zero)
                {
                    position = transform.GetChild(1).transform.position; // Set default position
                }

                var heroInstance = Instantiate(_heroPrefab, position, Quaternion.identity);
                if (heroInstance.TryGetComponent(out FacingBehaviour facingBehaviourComponent))
                    facingBehaviourComponent.SetFacingDirection(_checkPointController.FacingDirection);

                _gameplayBus.Hero = heroInstance.GetComponent<HeroBehaviour>();
                _gameplayBus.RaiseHeroSpawnedEvent();
                StartCoroutine(CoLoadDialogAndShowMessage());
            }
            else
            {
                base.SpawnPlayer();
            }
        }

        private const float ERROR_PRONE_DELAY = 0.5f;
        private IEnumerator CoLoadDialogAndShowMessage()
        {
            yield return new WaitForSeconds(ERROR_PRONE_DELAY);
            yield return GenericDialogController.Instance.CoInstantiate(dialog => _dialog = dialog);
            _gameState.UpdateGameState(EGameState.Dialogue);
            _dialog.WithMessage(_revivalMessage).RequireInput().WithHideCallback(EnableMapInput).Show();
        }

        private void EnableMapInput()
        {
            _gameState.UpdateGameState(EGameState.Field);
        }

        private void ShowCheckPointMessage()
        {
            StartCoroutine(CoLoadDialogAndShowMessage());
        }
    }
}