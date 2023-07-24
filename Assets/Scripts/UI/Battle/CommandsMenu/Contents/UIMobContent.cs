using NPOI.SS.Formula.Functions;
using TMPro;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    class UIMobContent : ContentItemMenu
    {
        [SerializeField] private TextMeshProUGUI _label;

        public class Attack : BarDataStructure
        {
            public Mob mob;
        }

        public override BarDataStructure Foo()
        {
            return new Attack();
        }

        public override void Init(BarDataStructure input)
        {
            var mob = input as Attack;
            _label.text = mob.mob.name;
        }
    }
}