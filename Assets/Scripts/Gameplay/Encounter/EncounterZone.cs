using System;
using CryptoQuest.Character.MonoBehaviours;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;

namespace CryptoQuest.Gameplay.Encounter
{
    public class EncounterZone : MonoBehaviour
    {
        public static event Action<string> LoadingEncounterArea;
        public static event Action<HeroBehaviour, string> EnterEncounterZone;
        public static event Action<HeroBehaviour, string> ExitEncounterZone;

        [Header("Area Configuration")]
        [SerializeField, ReadOnly] private string _playerTag = "Player";

        [SerializeField] private string _encounterId;

        /// <summary>
        /// When enter a map that contains encounter zone, we will try to load every encounter zone in that map
        /// </summary>
        private void Awake()
        {
            LoadingEncounterArea?.Invoke(_encounterId);
        }
        
        public void LoadEncounterArea()
        {
            LoadingEncounterArea?.Invoke(_encounterId);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;
            EnterEncounterZone?.Invoke(other.GetComponent<HeroBehaviour>(), _encounterId);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;
            ExitEncounterZone?.Invoke(other.GetComponent<HeroBehaviour>(), _encounterId);
        }

        /// <summary>
        /// This will be called when SuperTiled2Unity has finished importing the component.
        /// </summary>
        /// <param name="encounterId"></param>
        public void EncounterId(string encounterId)
        {
            _encounterId = encounterId;
        }
    }
}