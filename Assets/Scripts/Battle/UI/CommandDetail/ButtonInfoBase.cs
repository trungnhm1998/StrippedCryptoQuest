using System;
using CryptoQuest.Battle.UI.SelectItem;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace CryptoQuest.Battle.UI.CommandDetail
{
    [Serializable]
    public abstract class ButtonInfoBase
    {
        [field: SerializeField] public string Label { get; protected set; }
        [field: SerializeField] public LocalizedString LocalizedLabel { get; protected set; }
        [field: SerializeField] public string Value { get; protected set; }
        [field: SerializeField] public bool IsInteractable { get; protected set; } = true;

        public Button Button { get; set; }

        protected ButtonInfoBase(string label, string value = "", bool isInteractable = true)
        {
            Label = label;
            Value = value;
            IsInteractable = isInteractable;
        }

        public abstract void OnHandleClick();

        /// <summary>
        /// I use visitor here to have customize ui function that need specific info
        /// </summary>
        /// <param name="ui"></param>
        public virtual void Accept(IButtonUI ui) { }
    }

    public interface IButtonUI
    {
        void Visit(ItemButtonInfo itemButtonInfo);
    }
}