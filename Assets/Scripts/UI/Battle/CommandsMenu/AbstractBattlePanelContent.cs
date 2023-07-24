using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.UI.Battle.CommandsMenu
{
    public abstract class AbstractBattlePanelContent : MonoBehaviour
    {
        public abstract void Init(List<ButtonInfo> info);

        public abstract void Clear();
    }
}