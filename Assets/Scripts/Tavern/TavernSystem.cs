using CryptoQuest.Gameplay;
using CryptoQuest.Tavern.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Tavern
{
    public class TavernSystem : MonoBehaviour
    {
        [SerializeField] private GameStateSO _gameState;
        [SerializeField] private ShowTavernEventChannelSO _showTavern;
        [SerializeField] private TavernController _tavernController;
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
            _tavernDialogsManager.TavernOpened();
            _gameState.UpdateGameState(EGameState.Merchant);
        }

        private void ExitTavernRequested()
        {
            _tavernController.gameObject.SetActive(false);
            _tavernDialogsManager.TavernExited();
            _gameState.UpdateGameState(EGameState.Field);
        }

        private void TurnOnTavernOptions() => _tavernController.gameObject.SetActive(true);
    }
}