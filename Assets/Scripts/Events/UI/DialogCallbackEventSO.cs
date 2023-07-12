using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events.UI
{
    [CreateAssetMenu(menuName = "Events/Dialog Callback Event")]
    public class DialogCallbackEventSO : ScriptableObject
    {
        public UnityAction<UnityAction> OnEventRaised;

        public void Raise(UnityAction callback = null)
        {
            OnEventRaised?.Invoke(callback);
        }
    }
}