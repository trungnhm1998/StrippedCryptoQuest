using System.Collections;
using System.Collections.Generic;
using CryptoQuest.UI.Menu.MenuStates;
using CryptoQuest.UI.Menu.Panels.DimensionBox;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.DimensionBoxStates
{
    public abstract class DimensionBoxStateBase : MenuStateBase
    {
        protected UIDimensionBoxMenu DimensionBoxPanel { get; }

        protected DimensionBoxStateBase(UIDimensionBoxMenu dimensionBoxPanel)
        {
            DimensionBoxPanel = dimensionBoxPanel;
        }
    }
}
