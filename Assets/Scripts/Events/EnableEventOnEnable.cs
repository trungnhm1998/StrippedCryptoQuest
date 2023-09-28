using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events
{
    public class EnableEventOnEnable : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onEnableEvent;
        private void OnEnable() => StartCoroutine(DelayStart());

        private IEnumerator DelayStart()
        {
            yield return new WaitForSeconds(1);
            _onEnableEvent.Invoke();
        }
    }
}