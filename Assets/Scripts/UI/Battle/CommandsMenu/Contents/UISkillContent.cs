using System;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using TMPro;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    class UISkillContent : ContentItemMenu
    {
        [SerializeField] private TextMeshProUGUI _label;

        public override void Init(IBattleUnit unit)
        {
            _label.text = unit.UnitData.DisplayName;
        }
    }
}