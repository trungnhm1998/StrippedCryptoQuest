using UnityEngine;
using UnityEngine.UI;
using CryptoQuest.Gameplay.Battle;
using System.Collections.Generic;
namespace CryptoQuest.UI.Battle
{
    public class MonsterList : CharacterList
    {
        [SerializeField] private LayoutGroup _layout;
        [SerializeField] private RectTransform _layoutRect;

        public override void InitUI(List<IBattleUnit> units)
        {
            base.InitUI(units);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutRect);
            _layout.enabled = false;
        }
    }
}