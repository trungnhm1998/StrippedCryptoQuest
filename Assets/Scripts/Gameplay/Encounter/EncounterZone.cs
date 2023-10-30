using System;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using PlasticGui.WorkspaceWindow;
using UnityEngine;

namespace CryptoQuest.Gameplay.Encounter
{
    public class EncounterZone : MonoBehaviour
    {
        public static event Action<string> LoadingEncounterArea;
        public static event Action<EncounterInfo> EnterEncounterZone;
        public static event Action<EncounterInfo> ExitEncounterZone;
        public static event Action<EncounterInfo> RegisterEncounterZone;
        public static event Action<EncounterInfo> UnregisterEncounterZone;

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
            LoadingEncounterArea?.Invoke(_encounterId);
            _encounterInfo = new EncounterInfo(_encounterId, _priority);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;
            RegisterEncounterZone?.Invoke(_encounterInfo);
            EnterEncounterZone?.Invoke(_encounterInfo);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;
            UnregisterEncounterZone?.Invoke(_encounterInfo);
            ExitEncounterZone?.Invoke(_encounterInfo);
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