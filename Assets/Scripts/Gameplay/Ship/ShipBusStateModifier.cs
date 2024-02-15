using System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Ship
{
    public class ShipBusStateModifier : MonoBehaviour
    {
        [SerializeField] private ShipBus _shipBus;

        public void SetBusState(int state)
        {
            _shipBus.CurrentSailState = (ESailState) state;
        }
    }
}