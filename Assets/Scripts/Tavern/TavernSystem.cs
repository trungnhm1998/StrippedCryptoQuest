using System;
using CryptoQuest.Tavern.ScriptableObjects;
using CryptoQuest.Tavern.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Tavern
{
    public class TavernSystem : MonoBehaviour
    {
        [SerializeField] private ShowTavernEventChannelSO _showTavern;
        [SerializeField] private TavernPresenter _tavernPresenter;
        [SerializeField] private TavernInputManager _tavernInputManager;

        private void OnEnable()
        {
            _showTavern.EventRaised += ShowTavernRequested;
            _tavernPresenter.ExitTavernEvent += ExitTavernRequested;
        }

        private void OnDisable()
        {
            _showTavern.EventRaised -= ShowTavernRequested;
            _tavernPresenter.ExitTavernEvent -= ExitTavernRequested;
        }

        private void ShowTavernRequested()
        {
            _tavernPresenter.gameObject.SetActive(true);
            _tavernInputManager.EnableInput();
        }

        private void ExitTavernRequested()
        {
            _tavernPresenter.gameObject.SetActive(false);
            _tavernInputManager.DisableInput();
        }
    }
}