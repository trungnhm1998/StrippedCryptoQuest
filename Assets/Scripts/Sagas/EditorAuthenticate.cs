using CryptoQuest.Actions;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    public class EditorAuthenticate : MonoBehaviour
    {
#if UNITY_EDITOR
        private void OnGUI()
        {
            if (GUILayout.Button("Debug Account Login"))
                ActionDispatcher.Dispatch(new DebugLoginAction());

            if (GUILayout.Button("Skip Login"))
                ActionDispatcher.Dispatch(new AuthenticateSucceed());
        }
#endif
    }
}