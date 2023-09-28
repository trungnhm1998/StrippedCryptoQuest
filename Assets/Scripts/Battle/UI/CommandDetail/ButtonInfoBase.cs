using System;
using UnityEngine;

namespace CryptoQuest.Battle.UI.CommandDetail
{
    [Serializable]
    public abstract class ButtonInfoBase
    {
        [field: SerializeField] public string Label { get; protected set; }
        [field: SerializeField] public string Value { get; protected set; }
        [field: SerializeField] public bool IsInteractable { get; protected set; } = true;

        protected ButtonInfoBase(string label, string value = "", bool isInteractable = true)
        {
            Label = label;
            Value = value;
            IsInteractable = isInteractable;
        }

        public abstract void OnHandleClick();
    }
}