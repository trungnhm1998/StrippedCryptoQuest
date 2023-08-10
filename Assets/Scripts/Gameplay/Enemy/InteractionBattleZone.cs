using System.Collections;
using System.Collections.Generic;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Enemy
{
    public class InteractionBattleZone : MonoBehaviour
    {
        [Header("Listen Event")]
        [SerializeField] private VoidEventChannelSO _raiseEventBattle;

        [Header("Area Configuration")]
        [SerializeField, ReadOnly] private string _playerTag = "Player";

        private bool _isBattleEvent = false;
        private void OnEnable()
        {
            _raiseEventBattle.EventRaised += EnableBattleEvent;
        }

        private void OnDisable()
        {
            _raiseEventBattle.EventRaised -= EnableBattleEvent;
        }

        private void EnableBattleEvent()
        {
            if (!_isBattleEvent) return;
            Debug.Log("Raise Event");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;
            _isBattleEvent = true;
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(_playerTag)) return;
            _isBattleEvent = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _isBattleEvent = false;
        }
    }
}
