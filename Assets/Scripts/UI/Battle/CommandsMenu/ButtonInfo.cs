using System;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using UnityEngine.Events;

namespace CryptoQuest.UI.Battle.CommandsMenu
{
    [Serializable]
    public class ButtonInfo
    {
        public string Name;
        public string Value = "";
        public UnityAction Clicked;

        /// <summary>
        /// For setting up battle target
        /// </summary>
        /// <param name="unit"></param>
        public ButtonInfo(IBattleUnit unit)
        {
            Name = unit.UnitData.DisplayName;
        }
    }
}