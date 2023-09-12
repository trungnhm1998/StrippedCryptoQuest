using System;
using CryptoQuest.Character;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.NPC.Chest
{
    public class ChestBehaviour : MonoBehaviour, IInteractable
    {
        public static event Action<ChestBehaviour> LoadingChest;
        public static event Action<ChestBehaviour> Opening;
        public Action Opened; // Event will only called when the chest already opened
        [SerializeField, ReadOnly] private string _guid; // TODO: HideInInspector
        public string GUID => _guid;
        [SerializeField] private VoidEventChannelSO _sceneLoadedEvent;
        [SerializeField] private Animator _animator;
        [SerializeField] private int _treasureId = -1;
        public int Treasure => _treasureId;
        private bool _isOpened;
        private static readonly int IsOpening = Animator.StringToHash("IsOpening");
        private static readonly int OpenedState = Animator.StringToHash("Opened");

        private void Awake()
        {
            _sceneLoadedEvent.EventRaised += LoadChestState;
            Opened += SetChestToOpened;
        }

        private void OnDestroy()
        {
            _sceneLoadedEvent.EventRaised -= LoadChestState;
            Opened -= SetChestToOpened;
        }

        private void LoadChestState()
        {
            LoadingChest?.Invoke(this);
        }

        private void SetChestToOpened()
        {
            _isOpened = true;
            _animator.Play(OpenedState);
        }

        /// <summary>
        /// Ignore client side, will play animation again and again
        /// </summary>
        public void Interact()
        {
            if (_isOpened) return;
            _animator.SetBool(IsOpening, true);
            Opening?.Invoke(this);
        }

        public void TreasureId(int id)
        {
            _treasureId = id;
            if (string.IsNullOrEmpty(_guid))
                _guid = Guid.NewGuid().ToString();
        }
    }
}