using CryptoQuest.Input;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CryptoQuest.TrainingBattle
{
    public class TrainingBattleManager : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _showTrainingBattleEvent;
        [SerializeField] private TrainingBattleStateController _stateController;
        [SerializeField] private TrainingBattleDialogController _dialogController;
        [SerializeField] private MerchantsInputManager _input;
        [SerializeField] private GameObject _battlePanel;
        [SerializeField] private int _partyId;
        
        private void OnEnable()
        {
            _showTrainingBattleEvent.EventRaised += ShowTrainingBattle;
            _stateController.ExitStateEvent += HideTrainingBattle;
        }

        private void OnDisable()
        {
            _showTrainingBattleEvent.EventRaised -= ShowTrainingBattle;
            _stateController.ExitStateEvent -= HideTrainingBattle;
        }

        private void ShowTrainingBattle()
        {
            _input.EnableInput();
            _battlePanel.SetActive(true);
            _dialogController.ShowDialog();
            _stateController.SetPartyId(_partyId);
        }

        private void HideTrainingBattle()
        {
            _input.DisableInput();
            _battlePanel.SetActive(false);
            _dialogController.HideDialog();
        }
    }
}