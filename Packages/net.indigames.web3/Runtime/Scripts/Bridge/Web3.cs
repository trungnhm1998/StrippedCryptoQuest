using System.Runtime.InteropServices;

namespace IndiGames.Web3.Bridge
{
    public static class Web3
    {
        /// <summary>
        /// Open Web3 wallet and signs in a user
        /// </summary>
        /// <param name="objectName"> Name of the GameObject to call the callback/fallback of </param>
        /// <param name="callback"> Name of the method to call when the operation was successful. Method must have signature: void Method(string output) </param>
        /// <param name="fallback"> Name of the method to call when the operation was unsuccessful. Method must have signature: void Method(string output). Will return a serialized FirebaseError object </param>
        /// 
    #if UNITY_WEBGL
        [DllImport("__Internal")]
        public static extern void SignIn(string objectName, string callback, string fallback);
    #else
        public static void SignIn(string objectName, string callback, string fallback) { }
    #endif
    }
}