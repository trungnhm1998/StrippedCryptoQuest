using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.ShopSystem.Helpers
{
    public static class DelaySelectGameObject
    {
        public static IEnumerator CoDelaySelect(this GameObject go)
        {
            yield return null;
            EventSystem.current.SetSelectedGameObject(go);
        }
    }
}