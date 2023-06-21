using System.Collections.Generic;
using Core.Runtime.Events.ScriptableObjects.Dialogs;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

[TestFixture]
public class DialogControllerTest
{
    private DialogsScriptableObject _dialogSO;
    private NPC _npcGameObject;

    const string NPC_PREFAB_PATH = "Assets/Prefabs/Characters/NPCs/NPC.prefab";

    [SetUp]
    public void Setup()
    {
        _npcGameObject = AssetDatabase.LoadAssetAtPath(NPC_PREFAB_PATH, typeof(NPC)) as NPC;
        _npcGameObject = Object.Instantiate(_npcGameObject);

        _dialogSO = ScriptableObject.CreateInstance<DialogsScriptableObject>();
        _dialogSO.Messages = new List<string>();
    }

    [Test]
    public void GetNextIndex_WithEmptyDialogSO_ShouldReturnZero()
    {
        DialogController dialogController = _npcGameObject.GetComponent<DialogController>();
        dialogController.SetDialog(_dialogSO);

        var nextIndex = dialogController.GetNextIndex();

        Assert.AreEqual(0, nextIndex);
    }

    [Test]
    public void GetNextIndex_WithDummyDataDialogSO_ShouldReturnCorrectIndex()
    {
        var mockMessage = new List<string>() { "Hello World", "Hello World 2" };
        _dialogSO.Messages.AddRange(mockMessage);

        DialogController dialogController = _npcGameObject.GetComponent<DialogController>();
        dialogController.SetDialog(_dialogSO);

        var nextIndex = dialogController.GetNextIndex();

        Assert.AreEqual(1, nextIndex);
    }
}