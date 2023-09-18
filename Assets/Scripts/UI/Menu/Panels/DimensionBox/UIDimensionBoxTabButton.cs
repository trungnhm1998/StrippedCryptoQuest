using System.Collections.Generic;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox
{
    public class UIDimensionBoxTabButton : MultiInputButton
    {
        public void DimensionBoxButtonOnPressed()
        {
            Debug.Log($"Dimension Box {this.name} is opened!");
        }
    }
}
