using System;
using CryptoQuest.Core;
using CryptoQuest.System.Sceneload;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Gameplay.Encounter
{
    public class EncounterZone : MonoBehaviour
    {
        public static event Action<string> LoadingEncounterArea;
        public static event Action<string> EnterEncounterZone;
        public static event Action<string> ExitEncounterZone;


        [Header("Area Configuration")] [SerializeField, ReadOnly]
        protected string _playerTag = "Player";

        [SerializeField] protected string _encounterId;

        private TinyMessageSubscriptionToken _token;

        /// <summary>
        /// When enter a map that contains encounter zone, we will try to load every encounter zone in that map
        /// </summary>
        protected virtual void LoadEncounterArea()
        {
            LoadingEncounterArea?.Invoke(_encounterId);
        }

        private void OnEnable()
        {
            _token = ActionDispatcher.Bind<PostSceneLoadedAction>(HandleAction);
        }

        private void HandleAction(PostSceneLoadedAction _)
            => LoadEncounterArea();

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_token);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;
            EnterEncounterZone?.Invoke(_encounterId);
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;
            ExitEncounterZone?.Invoke(_encounterId);
        }

        /// <summary>
        /// These will be called when SuperTiled2Unity has finished importing the component.
        /// </summary>
        /// <param name="encounterId"></param>
        public void EncounterId(string encounterId)
        {
            _encounterId = encounterId;
        }
    }
}