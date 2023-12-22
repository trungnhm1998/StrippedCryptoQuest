using CryptoQuest.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Ship
{
    public class ShipSpawnPoint : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _spawnAllShipsEvent;
        [SerializeField] private PositionEventChannelSO _spawnShipAtPositionEvent;

        private void OnEnable()
        {
            _spawnAllShipsEvent.EventRaised += SpawnShip;
        }

        private void OnDisable()
        {
            _spawnAllShipsEvent.EventRaised -= SpawnShip;
        }

        private void SpawnShip()
        {
            _spawnShipAtPositionEvent.RaiseEvent(transform.position);
        }
    }
}
