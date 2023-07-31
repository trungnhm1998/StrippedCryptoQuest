using CryptoQuest.GameHandler;
using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using UnityEngine;

namespace CryptoQuest.UI.Battle.BattleActionHandler
{
    public class BattleActionHandler : MonoGameHandler<IBattleUnit>
    {
        public BattleInfo CurrentBattleInfo;
    }
}