using CryptoQuest.Actions;
using CryptoQuest.Networking;
using IndiGames.Core.Events;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    public class EditorAuthenticate : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private string _dev = "https://games.indigames.link/crypto-quest/dev/";
        [SerializeField] private string _qa = "https://games.indigames.link/crypto-quest/qa/";
        [SerializeField] private Credentials _credentials;

        private void OnGUI()
        {
            if (GUILayout.Button("Open Dev build"))
                Application.OpenURL(_dev);

            if (GUILayout.Button("Open QA Build"))
                Application.OpenURL(_qa);

            if (GUILayout.Button("Debug Account Login"))
                ActionDispatcher.Dispatch(new DebugLoginAction());

            SkipLogin();
        }

        private void SkipLogin()
        {
            if (!GUILayout.Button("Skip Login")) return;
            if (!string.IsNullOrEmpty(_credentials.Token))
                ActionDispatcher.Dispatch(new AuthenticateSucceed());
            else
            {
                Selection.SetActiveObjectWithContext(_credentials, _credentials);
                EditorGUIUtility.PingObject(_credentials);
            }
        }
#endif
    }
}