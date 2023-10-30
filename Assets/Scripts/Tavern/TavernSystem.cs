using System;
using CryptoQuest.Tavern.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Tavern
{
    public class TavernSystem : MonoBehaviour
    {
        [SerializeField] private ShowTavernEventChannelSO _showTavern;
        [SerializeField] private TavernController _tavernController;
        [SerializeField] private TavernInputManager _tavernInputManager;
        [SerializeField] private TavernDialogsManager _tavernDialogsManager;

        private void OnEnable()
        {
            _showTavern.EventRaised += ShowTavernRequested;
            _tavernController.ExitTavernEvent += ExitTavernRequested;
        }

        private void OnDisable()
        {
            _showTavern.EventRaised -= ShowTavernRequested;
            _tavernController.ExitTavernEvent -= ExitTavernRequested;
        }

        private void ShowTavernRequested()
        {
            _tavernController.gameObject.SetActive(true);
            _tavernInputManager.EnableInput();
            _tavernDialogsManager.TavernOpened();
        }

        private void ExitTavernRequested()
        {
            _tavernController.gameObject.SetActive(false);
            _tavernInputManager.DisableInput();
        }
    }
}