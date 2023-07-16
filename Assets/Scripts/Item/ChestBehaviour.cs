using CryptoQuest.Events.UI;
using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using CryptoQuest.Character;
using UnityEngine;

namespace CryptoQuest
{
    public class ChestBehaviour : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueScriptableObject _dialogue;
        [SerializeField] private DialogueEventChannelSO _dialogEventChannel;
        [SerializeField] private SpriteRenderer _currentSprite;
        [SerializeField] private Sprite _closedChest;
        private bool _isOpen = false;

        public void Interact()
        {
            if (!_isOpen)
            {
                _dialogEventChannel.Show(_dialogue);
                _currentSprite.sprite = _closedChest;
                _isOpen = !_isOpen;
            }
        }
    }
}
