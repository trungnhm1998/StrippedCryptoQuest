using System.Runtime.InteropServices;

namespace CryptoQuest.Bridge
{
    public static class ApplicationEventHandler
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        /// <summary>
        /// Register callback for OnClose event from browser
        /// </summary>
        /// <param name="objectName"> Name of the GameObject to call the callback of </param>
        /// <param name="callback"> Name of the method to call </param>
        ///
        [DllImport("__Internal")]
        public static extern void RegisterOnBeforeUnloadEventCallback(string objectName, string callback);

        /// <summary>
        /// Register callback for OnFocusChanged event from browser
        /// </summary>
        /// <param name="objectName"> Name of the GameObject to call the callback of </param>
        /// <param name="callback"> Name of the method to call </param>
        ///
        [DllImport("__Internal")]
        public static extern void RegisterOnFocusChangedEventCallback(string objectName, string callback);  
#else
        public static void RegisterOnBeforeUnloadEventCallback(string objectName, string callback) { }
        public static void RegisterOnFocusChangedEventCallback(string objectName, string callback) { }
#endif
    }
}