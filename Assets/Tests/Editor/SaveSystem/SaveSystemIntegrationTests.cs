using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoQuest.Environment;
using CryptoQuest.Networking;
using CryptoQuest.System.SaveSystem;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.SaveSystem
{
    [TestFixture]
    public class SaveSystemIntegrationTests
    {
        // TODO: REFACTOR NETWORK
        // private SaveSystemSO _saveSystemSO;
        //
        // private EnvironmentSO _environmentSO;
        // private AuthorizationSO _authorizationSO;
        //
        // private async Task<bool> DebugLogin()
        // {
        //     var url = _environmentSO.BackEndUrl + "/crypto/debug/login";
        //
        //     Dictionary<string, string> headers = new();
        //     headers.Add("DEBUG_KEY", _environmentSO.DEBUG_KEY);
        //
        //     var bodyData = JsonUtility.ToJson(new DebugLoginPayLoad(_environmentSO.DEBUG_TOKEN));
        //
        //     var req = await HttpClient.PostAsync(url, bodyData, headers);
        //
        //     if (req != null && req.responseCode == 200)
        //     {
        //         var assetGuid = AssetDatabase.FindAssets("t:AuthorizationSO");
        //         var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid[0]);
        //         _authorizationSO = AssetDatabase.LoadAssetAtPath<AuthorizationSO>(assetPath);
        //         if (_authorizationSO)
        //         {
        //             _authorizationSO.Init(req.downloadHandler.text);
        //             return true;
        //         }
        //     }
        //
        //     return false;
        // }
        //
        // [SetUp]
        // public void Setup()
        // {
        //     var assetGuid = AssetDatabase.FindAssets("t:SaveSystemSO");
        //     var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid[0]);
        //     _saveSystemSO = AssetDatabase.LoadAssetAtPath<SaveSystemSO>(assetPath);
        //     Assert.IsNotNull(_saveSystemSO, "Save System must not be null.");
        //
        //     assetGuid = AssetDatabase.FindAssets("t:AuthorizationSO");
        //     assetPath = AssetDatabase.GUIDToAssetPath(assetGuid[0]);
        //     _authorizationSO = AssetDatabase.LoadAssetAtPath<AuthorizationSO>(assetPath);
        //     Assert.That(_authorizationSO, Is.Not.Null, "AuthorizationSO should not be null.");
        //
        //     assetGuid = AssetDatabase.FindAssets("t:EnvironmentSO");
        //     assetPath = AssetDatabase.GUIDToAssetPath(assetGuid[0]);
        //     _environmentSO = AssetDatabase.LoadAssetAtPath<EnvironmentSO>(assetPath);
        //     Assert.That(_environmentSO, Is.Not.Null, "EnvironmentSO should not be null.");
        //
        //     // Login in debug mode before test, ensure it works with Online Storage Manager
        //     new Task(async () => { await DebugLogin(); }).Start(TaskScheduler.FromCurrentSynchronizationContext());
        // }
        //
        // [Test]
        // public async Task SaveGame_ShouldReturnTrue()
        // {
        //     Assert.IsTrue(await _saveSystemSO.SaveGameAsync(), "SaveGame should return true.");
        // }
        //
        // [TearDown]
        // public void Teardown()
        // {
        //     _saveSystemSO = null;
        // }
    }
}