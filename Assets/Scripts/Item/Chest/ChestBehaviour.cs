using CryptoQuest.Events.UI;
using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using CryptoQuest.Character;
using UnityEngine;

namespace CryptoQuest
{
    public class ChestBehaviour : MonoBehaviour, IInteractable
    {
        [Header("Event")]
        [SerializeField] private OpenChestEventChannelSO _openChestChannel;
        [Header("Raise on")]
        [SerializeField] private ChestDataSO _chestData;

        [SerializeField] private Animator _animator;
        private bool _isOpened = false;
        private static readonly int IsOpening = Animator.StringToHash("IsOpening");

        public void Interact()
        {
            if (!_isOpened)
            {
                _isOpened = true;
                _animator.SetBool(IsOpening, _isOpened);
                _openChestChannel.Open(_chestData);
            }
        }
    }
}