using CryptoQuest.Events.UI;
using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using CryptoQuest.Character;
using UnityEngine;

namespace CryptoQuest.Gameplay.NPC.Chest
{
    public class ChestBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private ChestEventChannelSO _chestEventChannel;
        [SerializeField] private ChestData _chestData;
        [SerializeField] private Animator _animator;
        private bool _isOpened = false;
        private static readonly int IsOpening = Animator.StringToHash("IsOpening");

        public void Interact()
        {
            if (!_isOpened)
            {
                _isOpened = true;
                _animator.SetBool(IsOpening, _isOpened);
                _chestEventChannel.Open(_chestData);
            }
        }
    }
}