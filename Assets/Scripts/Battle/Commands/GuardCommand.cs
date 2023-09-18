using System;
using CryptoQuest.Battle.Components;
using UnityEngine;

namespace CryptoQuest.Battle.Commands
{
    public class GuardCommand : ICommand
    {
        private GameObject _heroGo;
        public GuardCommand(GameObject heroGo)
        {
            _heroGo = heroGo;
        }

        public void Execute()
        {
            _heroGo.GetComponent<GuardBehaviour>().GuardUntilEndOfTurn();
        }
    }
}