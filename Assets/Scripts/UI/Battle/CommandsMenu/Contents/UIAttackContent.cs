using TMPro;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    public class UIAttackContent : ContentItemMenu
    {
        [SerializeField] private TextMeshProUGUI _label;

        public class Attack : BarDataStructure
        {
            public Mob mob;
        }

        public override void Init(BarDataStructure input)
        {
            var attack = input as Attack;
            _label.text = attack.mob.name;
        }
    }
}