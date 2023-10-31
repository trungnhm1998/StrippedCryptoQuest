using System;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;

namespace CryptoQuest.Gameplay.Encounter
{
    public class EncounterZone : MonoBehaviour
    {
        public static event Action<string> LoadingEncounterArea;
        public static event Action EnterEncounterZone;
        public static event Action ExitEncounterZone;
        public static event Action<EncounterInfo> RegisterEncounterInfo;
        public static event Action<EncounterInfo> UnregisterEncounterInfo;

        [Header("Area Configuration")] [SerializeField, ReadOnly]
        private string _playerTag = "Player";

        [SerializeField] private string _encounterId;
        [SerializeField] private int _priority = 0;
        private EncounterInfo _encounterInfo;

        /// <summary>
        /// When enter a map that contains encounter zone, we will try to load every encounter zone in that map
        /// </summary>
        private void Awake()
        {
            _encounterInfo = new EncounterInfo(_encounterId, _priority);
        }

        private void LoadEncounterArea()
        {
            LoadingEncounterArea?.Invoke(_encounterId);
        }

        private void OnEnable()
        {
            SceneLoaderDispatch.SceneLoaded += LoadEncounterArea;
        }

        private void OnDisable()
        {
            SceneLoaderDispatch.SceneLoaded -= LoadEncounterArea;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;
            RegisterEncounterInfo?.Invoke(_encounterInfo);
            EnterEncounterZone?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;
            UnregisterEncounterInfo?.Invoke(_encounterInfo);
            ExitEncounterZone?.Invoke();
        }

        /// <summary>
        /// These will be called when SuperTiled2Unity has finished importing the component.
        /// </summary>
        /// <param name="encounterId"></param>
        public void EncounterId(string encounterId)
        {
            _encounterId = encounterId;
        }

        public void Priority(int priority)
        {
            _priority = priority;
        }
    }

    [Serializable]
    public class EncounterInfo
    {
        public string EncounterId;
        public int Priority;

        public EncounterInfo(string encounterId, int priority)
        {
            EncounterId = encounterId;
            Priority = priority;
        }
    }
}