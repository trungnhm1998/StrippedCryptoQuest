using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    public abstract class AbstractBattlePanelContent : MonoBehaviour
    {
        public abstract void Init(IBattleUnit unit);
        
        public abstract void SetPanelActive(bool isActive);
    }
}