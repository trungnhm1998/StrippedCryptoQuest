using System.Collections;
using CryptoQuest.Core;
using CryptoQuest.Environment;
using CryptoQuest.Networking;
using CryptoQuest.Networking.Actions;
using CryptoQuest.Sagas;
using CryptoQuest.System;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace CryptoQuest.Tests.Runtime.Networking
{
    [TestFixture]
    public class DebugLoginTests
    {
        private RestClientController _restClientController;
        private DebugLogin _debugLogin;
        private Credentials _credentials;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var env = ScriptableObject.CreateInstance<EnvironmentSO>();
            var envSo = new SerializedObject(env);
            envSo.FindProperty("_apiUrl").stringValue = "https://dev-api-game.crypto-quest.org";
            envSo.FindProperty("_apiVersion").stringValue = "/v1";
            envSo.ApplyModifiedProperties();
            ServiceProvider.Provide(env);
            
            _credentials = ScriptableObject.CreateInstance<Credentials>();
            var internalAuth = new GameObject().AddComponent<InternalAuthenticate>();
            var so = new SerializedObject(internalAuth);
            so.FindProperty("_credentials").objectReferenceValue = _credentials;
            so.ApplyModifiedProperties();
        }

        [UnitySetUp]
        public IEnumerator Setup()
        {
            _restClientController = new GameObject().AddComponent<RestClientController>();
            _debugLogin = new GameObject().AddComponent<DebugLogin>();
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator DebugLoginTest()
        {
            yield return null;
            var login = false;
            var token = ActionDispatcher.Bind<AuthenticateSucceed>(_ => login = true);
            ActionDispatcher.Dispatch(new DebugLoginAction());
            yield return new WaitUntil(() => login);
            ActionDispatcher.Unbind(token);
        }
    }
}