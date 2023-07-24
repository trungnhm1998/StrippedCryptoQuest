using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    public abstract class AbstractBattlePanelContent : MonoBehaviour
    {
        public abstract void Init(List<ButtonInfo> info);

        public abstract void Clear();
    }
}