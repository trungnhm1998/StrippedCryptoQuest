using System;
using UnityEngine;

namespace CryptoQuest.UI.Menu
{
    public abstract class UIMenuPanelBase : MonoBehaviour
    {
        public event Action Focusing;
        public void OnFocusing() => Focusing?.Invoke();
    }
}