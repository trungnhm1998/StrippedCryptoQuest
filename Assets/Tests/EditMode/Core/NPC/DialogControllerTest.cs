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
        dialogController.SetDialogData(_dialogSO);

        var index = dialogController.GetNextIndex();

        Assert.AreEqual(0, index);
    }

    [Test]
    public void GetNextIndex_WithDummyDataDialogSO_ShouldReturnCorrectIndex()
    {
        var mockMessage = new List<string>() { "Hello World", "Hello World 2", "Hello World 3", "Hello World 4" };
        _dialogSO.Messages.AddRange(mockMessage);

        DialogController dialogController = _npcGameObject.GetComponent<DialogController>();
        dialogController.SetDialogData(_dialogSO);

        int expectedCount = mockMessage.Count;

        for (int expected = 0; expected < expectedCount; expected++)
        {
            var nextIndex = dialogController.GetNextIndex();
            Assert.AreEqual(expected, nextIndex);
        }
    }
}