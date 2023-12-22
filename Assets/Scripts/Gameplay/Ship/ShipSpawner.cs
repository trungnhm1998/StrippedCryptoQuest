using System;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Ship;
using UnityEngine;

namespace CryptoQuest.Gameplay.Ship
{
    public class ShipSpawner : MonoBehaviour
    {
        [SerializeField] private ShipBehaviour _shipPrefab;
        [SerializeField] private PositionEventChannelSO _spawnShipAtPositionEvent;

        public event Action<ShipBehaviour> ShipSpawned;

        private void OnEnable()
        {
            _spawnShipAtPositionEvent.EventRaised += SpawnShipAtPosition;
        }

        private void OnDisable()
        {
            _spawnShipAtPositionEvent.EventRaised -= SpawnShipAtPosition;
        }

        private void SpawnShipAtPosition(Vector3 position)
        {
            ShipSpawned?.Invoke(SpawnShip(position));
        }

        public ShipBehaviour SpawnShip(Vector3 position)
        {
            var ship = Instantiate<ShipBehaviour>(_shipPrefab, position, Quaternion.identity);
            return ship;
        }
    }
}
