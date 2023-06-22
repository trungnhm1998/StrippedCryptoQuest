using System.Collections.Generic;
using Core.Runtime.Events.ScriptableObjects.Dialogs;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

[TestFixture]
public class DialogControllerTest
{
    private DialogsScriptableObject _dialogSO;
    private DialogController _dialogController;
    private NPC _npcGameObject;

    const string NPC_PREFAB_PATH = "Assets/Prefabs/Characters/NPCs/NPC.prefab";

    private List<string> DIALOG_TEST = new List<string>
    {
        "Hello World", "Hello World 2", "Hello World 3", "Hello World 4"
    };

    [SetUp]
    public void Setup()
    {
        _npcGameObject = AssetDatabase.LoadAssetAtPath(NPC_PREFAB_PATH, typeof(NPC)) as NPC;
        _npcGameObject = Object.Instantiate(_npcGameObject);

        _dialogSO = ScriptableObject.CreateInstance<DialogsScriptableObject>();
        _dialogSO.Messages = new List<string>();
        _dialogSO.Messages.AddRange(DIALOG_TEST);

        _dialogController = _npcGameObject.GetComponent<DialogController>();
        _dialogController.SetDialogData(_dialogSO);
    }

    [Test]
    public void GetNextIndex_WithEmptyDialogSO_ShouldReturnZero()
    {
        _dialogSO.Messages.Clear();
        var index = _dialogController.GetNextIndex();

        Assert.AreEqual(0, index);
    }

    [Test]
    public void GetNextIndex_WithDummyDataDialogSO_ShouldReturnCorrectIndex()
    {
        int expectedCount = DIALOG_TEST.Count;
        bool hasCorrectData = true;

        for (int expected = 1; expected < expectedCount; expected++)
        {
            var nextIndex = _dialogController.GetNextIndex();
            if (expected != nextIndex)
            {
                hasCorrectData = false;
                break;
            }
        }

        Assert.IsTrue(hasCorrectData);
    }

    [Test]
    public void GetNextIndex_Called_ShouldReturnOne()
    {
        var index = _dialogController.GetNextIndex();
        var expected = 1;

        Assert.AreEqual(expected, index);
    }

    [Test]
    public void GetCurrentDialogIndex_ShouldReturnZeroByDefault()
    {
        var currentIndex = _dialogController.GetCurrentDialogIndex();
        var expectedIndex = 0;

        Assert.AreEqual(expectedIndex, currentIndex);
    }
}