using UnityEngine;
using IndiGames.Core.Events.ScriptableObjects;
using CommandTerminal;
using CryptoQuest.Gameplay;

namespace CryptoQuest.System.Cheat
{
    public class ShipCheat : MonoBehaviour, ICheatInitializer
    {
        [SerializeField] private GameplayBus _gameplayBus;
        [SerializeField] private VoidEventChannelSO _setActiveShipEvent;
        [SerializeField] private VoidEventChannelSO _spawnAllShipsEvent;

        public void InitCheats()
        {
            Terminal.Shell.AddCommand("spawn.ships", RequestSpawnAllShips, 0, 0,
                "Request spawn all ships");
        }

        private void RequestSpawnAllShips(CommandArg[] args)
        {
            _setActiveShipEvent.RaiseEvent();
            _spawnAllShipsEvent.RaiseEvent();
        }
    }
}
