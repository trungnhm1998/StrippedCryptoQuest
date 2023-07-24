using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    public abstract class ContentItemMenu : MonoBehaviour
    {
        public abstract void Init(IBattleUnit unit);
    }
}