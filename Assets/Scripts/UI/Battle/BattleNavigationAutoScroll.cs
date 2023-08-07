using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using CryptoQuest.UI.Common;

namespace CryptoQuest.UI.Battle
{
    public class BattleNavigationAutoScroll : NavigationAutoScroll
    {
        [Header("Events")]
        [SerializeField] private BattleInputSO _battleInput;

        protected override void Update()
        {
            if (!_battleInput.InputActions.BattleMenu.Navigate.IsPressed()) return;
            base.Update();
        }
    }
}