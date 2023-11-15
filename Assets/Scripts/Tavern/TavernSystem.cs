using CryptoQuest.Input;
using CryptoQuest.Tavern.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Tavern
{
    public class TavernSystem : MonoBehaviour
    {
        [SerializeField] private ShowTavernEventChannelSO _showTavern;
        [SerializeField] private TavernController _tavernController;
        [SerializeField] private MerchantsInputManager _merchantInputManager;
        [SerializeField] private TavernDialogsManager _tavernDialogsManager;

        private void OnEnable()
        {
            _showTavern.EventRaised += ShowTavernRequested;
            _tavernController.ExitTavernEvent += ExitTavernRequested;
            _tavernDialogsManager.TurnOnTavernOptionsEvent += TurnOnTavernOptions;
        }

        private void OnDisable()
        {
            _showTavern.EventRaised -= ShowTavernRequested;
            _tavernController.ExitTavernEvent -= ExitTavernRequested;
            _tavernDialogsManager.TurnOnTavernOptionsEvent -= TurnOnTavernOptions;
        }

        private void ShowTavernRequested()
        {
            _merchantInputManager.EnableInput();
            _tavernDialogsManager.TavernOpened();
        }

        private void ExitTavernRequested()
        {
            _tavernController.gameObject.SetActive(false);
            _tavernDialogsManager.TavernExited();
            _merchantInputManager.DisableInput();
        }

        private void TurnOnTavernOptions() => _tavernController.gameObject.SetActive(true);
    }
}