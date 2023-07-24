using TMPro;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    class UIItemContent : ContentItemMenu
    {
        [SerializeField] private TextMeshProUGUI _label;

        public class Item : BarDataStructure
        {
            public ItemName itemname;
        }

        public override BarDataStructure Foo()
        {
            return new Item();
        }

        public override void Init(BarDataStructure input)
        {
            var item = input as Item;
            _label.text = item.itemname.name;
        }
    }
}