using System;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Ship;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Savers
{
    [Serializable]
    public class ShipSaver : SaverBase
    {
        [SerializeField] private ShipBus _shipBus;
        [SerializeField] private GameplayBus _gameplayBus;


        public override void RegistEvents()
        {
            _shipBus.Changed += SaveShipProgression;
        }

        public override void UnregistEvents()
        {
            _shipBus.Changed -= SaveShipProgression;
        }

        private void SaveShipProgression()
        {
            if (_gameplayBus.Hero.TryGetComponent(out HeroPositionSaver positionSaver))
            {
                // Because hero position not being saved immedietly so 
                // there's case that player sailed a ship but position in land
                // after relogin they can sail in land
                positionSaver.SaveCurrentPosition();
            }

            _saveSystem.SaveData[_shipBus.name] = 
                JsonConvert.SerializeObject(_shipBus);
            _saveHandler.Save();
        }
    }
}