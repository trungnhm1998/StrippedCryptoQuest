﻿using UnityEngine;

namespace CryptoQuest.Ocarina
{
    public class OcarinaTownRegister : MonoBehaviour
    {
        [SerializeField] private OcarinaEntrance _townToRegister;
        [SerializeField] private RegisterTownToOcarinaEventChannelSO _registerTownEvent;

        private void Awake()
        {
            _registerTownEvent.RaiseEvent(_townToRegister);
        }
    }
}