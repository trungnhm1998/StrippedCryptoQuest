using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle
{
    public class UICommandContent : ContentItemMenu
    {
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private TextMeshProUGUI _value;
        [SerializeField] private Button _button;

        public override void Init(ButtonInfo info)
        {
            _label.text = info.Name;
            _value.text = info.Value;
            _button.onClick.AddListener(info.Callback.Invoke);
        }
    }
}