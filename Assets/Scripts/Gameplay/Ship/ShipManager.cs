using UnityEngine;
using IndiGames.Core.Events.ScriptableObjects;
using System.Collections.Generic;

namespace CryptoQuest.Gameplay.Ship
{
    public class ShipManager : MonoBehaviour
    {
        [SerializeField] private ShipSpawner _shipSpawner;
        [SerializeField] private ShipBus _shipBus;
        [SerializeField] private VoidEventChannelSO _sceneLoadedEvent;
        [SerializeField] private VoidEventChannelSO _forceSpawnAllShipsEvent;
        [SerializeField] private VoidEventChannelSO _setActiveShipEvent;
        [SerializeField] private VoidEventChannelSO _requestSpawnAllShipsEvent;
        
        private List<ShipBehaviour> _spawnedShips = new();
        private ShipBehaviour _sailedShip;

        private void OnEnable()
        {
            _shipSpawner.ShipSpawned += ShipSpawned;
            _setActiveShipEvent.EventRaised += SetShipActivated;
            _requestSpawnAllShipsEvent.EventRaised += RequestSpawnShip;
            _forceSpawnAllShipsEvent.EventRaised += OnSpawnAllShip;
        }

        private void OnDisable()
        {
            _shipSpawner.ShipSpawned -= ShipSpawned;
            _setActiveShipEvent.EventRaised -= SetShipActivated;
            _requestSpawnAllShipsEvent.EventRaised -= RequestSpawnShip;
            _forceSpawnAllShipsEvent.EventRaised -= OnSpawnAllShip;
        }

        private void OnDestroy()
        {
            DestroyAllOtherShip(_sailedShip);
            DestroySailedShip();
        }

        private void RequestSpawnShip()
        {
            _shipSpawner.enabled = _shipBus.IsShipActivated;
            if (!_shipBus.IsShipActivated || _shipBus.CurrentSailState == ESailState.Sailing) return;

            if (TrySpawnShipAtLastPosition()) return;

            _forceSpawnAllShipsEvent.RaiseEvent();
            _shipBus.CurrentSailState = ESailState.NotSail;
        }

        private bool TrySpawnShipAtLastPosition()
        {
            if (_shipBus.CurrentSailState != ESailState.Landed)
            {
                Debug.LogWarning($"There is no landed ship to spawn at LastPosition.");
                return false;
            }

            _sailedShip = _shipSpawner.SpawnShip(_shipBus.LastPosition.ToVector3());
            return true;
        }

        private void SetShipActivated()
        {
            if (_shipBus.IsShipActivated) return;
            _shipBus.IsShipActivated = true;
            _shipBus.CurrentSailState = ESailState.NotSail;
            _shipSpawner.enabled = _shipBus.IsShipActivated;
        }

        private void ShipSpawned(ShipBehaviour ship)
        {
            _spawnedShips.Add(ship);
            ship.SailedShip += SailedShip;
        }

        private void SailedShip(ShipBehaviour ship)
        {
            _sailedShip = ship;
            DestroyAllOtherShip(ship);
        }

        /// <summary>
        /// Destroy all ship except sailed ship
        /// </summary>
        /// <param name="sailedShip"></param>
        private void DestroyAllOtherShip(ShipBehaviour sailedShip)
        {
            foreach (var ship in _spawnedShips)
            {
                if (ship == null || ship == sailedShip) continue;
                ship.SailedShip -= SailedShip;
                Destroy(ship.gameObject);
            }

            _spawnedShips.Clear();
        }

        private void OnSpawnAllShip()
        {
            _shipBus.CurrentSailState = ESailState.NotSail;
            DestroySailedShip();
        }

        private void DestroySailedShip()
        {
            if (_sailedShip == null) return;
            _sailedShip.SailedShip -= SailedShip;
            Destroy(_sailedShip.gameObject);
        }
    }
}
