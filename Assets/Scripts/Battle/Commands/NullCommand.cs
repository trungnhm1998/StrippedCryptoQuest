using System.Collections;
using CryptoQuest.Battle.Commands;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class NullCommand : ICommand
    {
        private static NullCommand _instance;
        public static NullCommand Instance => _instance ??= new NullCommand();

        public void Execute()
        {
            Debug.LogWarning($"No command");
        }
    }
}