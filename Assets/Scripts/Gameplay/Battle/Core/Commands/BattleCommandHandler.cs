using System;
using System.Collections.Generic;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Battle.Core.Commands.BattleCommands;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Commands
{
    public class BattleCommandHandler : MonoBehaviour, ICommandHandler<BattleCommand>
    {
        /// <summary>
        /// I used static event for faster integration
        /// And we don't have to create event channel for this 
        /// </summary>
        public static Action<BattleCommand> OnReceivedCommand;

        [Header("Raise Events")]
        [SerializeField] private LocalizedStringEventChannelSO _showBattleDialogEventChannel;

        public Queue<BattleCommand> CommandsQueue { get; private set; } = new();

        public bool IsQueueEmpty => CommandsQueue.Count <= 0;

        private void OnEnable()
        {
            OnReceivedCommand += AddCommand;
        }

        private void OnDisable()
        {
            OnReceivedCommand -= AddCommand;
            CommandsQueue.Clear();
        }

        public void AddCommand(BattleCommand command)
        {
            CommandsQueue.Enqueue(command);
        }

        public void ExecuteCommand()
        {
            if (CommandsQueue.Count <= 0)
            {
                Debug.Log($"BattleCommandHandler:: This is the end of command queue");
                return;
            }


            var command = CommandsQueue.Dequeue();
            Debug.Log($"BattleCommandHandler:: Executing command {command}");
            command.ShowDialog += _showBattleDialogEventChannel.RaiseEvent;
            command.FinishedCommand += ExecuteCommand;

            command.Execute();
        }
    }
}