using CommandTerminal;
using CryptoQuest.Item.MagicStone.Sagas;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.System.Cheat
{
    public class AddStoneCheats : MonoBehaviour, ICheatInitializer
    {
        public void InitCheats()
        {
            Terminal.Shell.AddCommand("add.stone", AddStone, 1, 1, "Add stone");
        }

        private void AddStone(CommandArg[] obj)
        {
            ActionDispatcher.Dispatch(new AddMagicStoneAction()
            {
                Id = obj[0].String,
                Quantity = 1
            });
        }
    }
}