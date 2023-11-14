using CryptoQuest.Church.ScriptableObjects;
using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.Church
{
    public class ChurchManager : MonoBehaviour
    {
        [SerializeField] private ShowChurchEventChannelSO _showChurchEvent;
        [SerializeField] private MerchantsInputManager _input;
        [SerializeField] private GameObject _churchPanel;

        private void OnEnable()
        {
            _showChurchEvent.EventRaised += ShowChurch;
        }

        private void OnDisable()
        {
            _showChurchEvent.EventRaised -= ShowChurch;
        }

        private void ShowChurch()
        {
            _input.EnableInput();
            _churchPanel.SetActive(true);
        }

        private void HideChurch()
        {
            _input.DisableInput();
            _churchPanel.SetActive(false);
        }


    }
}
