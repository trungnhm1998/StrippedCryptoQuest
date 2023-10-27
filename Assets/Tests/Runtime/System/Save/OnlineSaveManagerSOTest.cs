using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoQuest.Environment;
using CryptoQuest.Networking;
using CryptoQuest.System.SaveSystem;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Tests.Runtime.System.Save
{
    public class OnlineSaveManagerSOTest
    {
        // private SaveManagerSO _saveManagerSO;
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
        //     var assetGuid = AssetDatabase.FindAssets("t:OnlineSaveManagerSO");
        //     Assert.That(assetGuid.Length, Is.EqualTo(1),
        //         "There should be exactly one StorageSaveManagerSO in the project.");
        //
        //     var assetPath = AssetDatabase.GUIDToAssetPath(assetGuid[0]);
        //     _saveManagerSO = AssetDatabase.LoadAssetAtPath<OnlineSaveManagerSO>(assetPath);
        //     Assert.That(_saveManagerSO, Is.Not.Null, "StorageSaveManagerSO should not be null.");
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
        // }
        //
        // [Test]
        // public async Task SaveGame_ShouldReturnTrue()
        // {
        //     var ret = await DebugLogin();
        //     if (ret && _saveManagerSO != null)
        //     {
        //         var result = await _saveManagerSO.SaveAsync("{}");
        //         Assert.That(result, Is.EqualTo(true), "SaveGame should return true.");
        //     }
        //     else
        //     {
        //         Assert.Fail("User not logged in");
        //     }
        // }
        //
        // [Test]
        // public async Task LoadSaveGame_ShouldReturnTrue()
        // {
        //     var ret = await DebugLogin();
        //     if (ret && _saveManagerSO != null)
        //     {
        //         var saveData = await _saveManagerSO.LoadAsync();
        //         Assert.That(saveData != null,
        //             "LoadSaveGame should return non null value and must contain default player name.");
        //     }
        //     else
        //     {
        //         Assert.Fail("User not logged in");
        //     }
        // }
    }
}