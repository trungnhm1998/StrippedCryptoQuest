using CryptoQuest.Input;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Church
{
    public class ChurchManager : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _showChurchEvent;
        [SerializeField] private ChurchStateController _stateController;
        [SerializeField] private ChurchDialogConroller _dialogController;
        [SerializeField] private MerchantsInputManager _input;
        [SerializeField] private GameObject _churchPanel;

        private void OnEnable()
        {
            _showChurchEvent.EventRaised += ShowChurch;
            _stateController.ExitStateEvent += HideChurch;
        }

        private void OnDisable()
        {
            _showChurchEvent.EventRaised -= ShowChurch;
            _stateController.ExitStateEvent -= HideChurch;
        }

        private void ShowChurch()
        {
            _input.EnableInput();
            _churchPanel.SetActive(true);
            _dialogController.ShowDialog();
        }

        private void HideChurch()
        {
            _input.DisableInput();
            _churchPanel.SetActive(false);
            _dialogController.HideDialog();
        }
    }
}
