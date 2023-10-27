using System;
using CryptoQuest.Tavern.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Tavern
{
    public class TavernSystem : MonoBehaviour
    {
        [SerializeField] private ShowTavernEventChannelSO _showTavern;
        [SerializeField] private GameObject _tavernOverviewPresenter;

        private void OnEnable()
        {
            _showTavern.EventRaised += ShowTavernRequested;
        }

        private void OnDisable()
        {
            _showTavern.EventRaised -= ShowTavernRequested;
        }

        private void ShowTavernRequested()
        {
            _tavernOverviewPresenter.SetActive(true);
        }
    }
}