using CryptoQuest.Events;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class EncounterZone : MonoBehaviour
    {
        [SerializeField] private StringEventChannelSO _battleEncounterConfigureEvent;
        [SerializeField] private StringEventChannelSO _encounterRequestEvent;

        [Header("Listen to Events")]
        [SerializeField] private FloatEventChannelSO _encounterRateConfigResponseEvent;

        [Header("Area Configuration")]
        [SerializeField, ReadOnly] private string _playerTag = "Player";

        [SerializeField, ReadOnly] private string _encounterId;
        [SerializeField] private float _customRatio = 1.7f;
        private BattleFieldSO _battleField;
        private Vector2 _playerPosition;
        private float _encounterRate = 0;
        private float _countdown;

        private void OnEnable()
        {
            _encounterRateConfigResponseEvent.EventRaised += ConfigEncounterRate;
        }

        private void OnDisable()
        {
            _encounterRateConfigResponseEvent.EventRaised -= ConfigEncounterRate;
        }

        private void Awake()
        {
        }

        private void ConfigEncounterRate(float encounterRate)
        {
            _encounterRate = encounterRate;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;
            _battleEncounterConfigureEvent.RaiseEvent(_encounterId);
            InitCountDown();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;
            if ((Vector2)other.transform.position == _playerPosition) return;

            _playerPosition = other.transform.position;
            _countdown -= Time.deltaTime / _customRatio;
            if (_countdown < 0f)
            {
                OnCountdownEnd();
                InitCountDown();
            }
        }

        private void OnCountdownEnd()
        {
            OnTriggerBattleEncounter();
        }


        private void InitCountDown()
        {
            _countdown = _encounterRate;
        }


        private void OnTriggerBattleEncounter()
        {
            _encounterRequestEvent.RaiseEvent(_encounterId);
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