using System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Encounter
{
    public class OverlappingEncounterZone : EncounterZone
    {
        public new static event Action<string> LoadingEncounterArea;
        public new static event Action EnterEncounterZone;
        public new static event Action ExitEncounterZone;
        public static event Action<EncounterInfo> RegisterEncounterInfo;
        public static event Action<EncounterInfo> UnregisterEncounterInfo;
        [SerializeField] private int _priority = 0;
        private EncounterInfo _encounterInfo;

        private void Awake()
        {
            _encounterInfo = new EncounterInfo(_encounterId, _priority);
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;
            RegisterEncounterInfo?.Invoke(_encounterInfo);
            EnterEncounterZone?.Invoke();
        }

        protected override void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;
            UnregisterEncounterInfo?.Invoke(_encounterInfo);
            ExitEncounterZone?.Invoke();
        }

        protected override void LoadEncounterArea()
        {
            LoadingEncounterArea?.Invoke(_encounterId);
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