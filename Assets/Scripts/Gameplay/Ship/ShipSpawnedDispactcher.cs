using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Gameplay.Ship
{
    public class SpawnShipAction : ActionBase
    {
        public IShip JustSpawnedShip { get; private set; }
        public SpawnShipAction(IShip ship)
        {
            JustSpawnedShip = ship;
        }
    }

    public class ShipSpawnedDispactcher : MonoBehaviour
    {
        [SerializeField] private ShipSpawner _shipSpawner;

        private void OnEnable()
        {
            _shipSpawner.ShipSpawned += ShipSpawned;
        }

        private void OnDisable()
        {
            _shipSpawner.ShipSpawned -= ShipSpawned;
        }

        private void ShipSpawned(IShip ship)
        {
            ActionDispatcher.Dispatch(new SpawnShipAction(ship));
        }
    }
}
