using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using TMPro;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    public class UIAttackContent : ContentItemMenu
    {
        [SerializeField] private TextMeshProUGUI _label;

        public override void Init(IBattleUnit unit)
        {
            _label.text = unit.UnitData.DisplayName;
        }
    }
}