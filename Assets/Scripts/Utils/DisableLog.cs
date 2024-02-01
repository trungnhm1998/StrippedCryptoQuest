using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Utils
{
    public class DisableLog : MonoBehaviour
    {
        private void Awake() 
        {
#if !(DEVELOPMENT_BUILD || UNITY_EDITOR)
            Debug.unityLogger.filterLogType = LogType.Exception;
#endif
        }
    }
}
