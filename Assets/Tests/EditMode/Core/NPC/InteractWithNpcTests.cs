using System;
using System.Collections.Generic;
using Core.Runtime.Events.ScriptableObjects.Dialogs;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[TestFixture]
public class InteractWithNpcTests
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
    public void Interact_WithNewDialogSO_ShouldReturnCorrectDataFromSO()
    {
        var mockMessage = "Hello World";
        _dialogSO.Messages.Add(mockMessage);

        DialogController dialogController = _npcGameObject.GetComponent<DialogController>();
        dialogController.SetDialog(_dialogSO);

        _npcGameObject.Interact();

        Assert.AreEqual(mockMessage, _npcGameObject.DialogData);
    }

    [Test]
    public void Interact_WithNewEmptyMessageDialogSO_ShouldReturnEmptyString()
    {
        DialogController dialogController = _npcGameObject.GetComponent<DialogController>();
        dialogController.SetDialog(_dialogSO);

        _npcGameObject.Interact();

        Assert.AreEqual(String.Empty, _npcGameObject.DialogData);
    }

    [Test]
    public void Interact_WithTwoInteract_ShouldReturnCorrectMultiDataFromSO()
    {
        var mockMessage = new List<string>() { "Hello World", "Hello World 2" };
        _dialogSO.Messages.AddRange(mockMessage);

        DialogController dialogController = _npcGameObject.GetComponent<DialogController>();
        dialogController.SetDialog(_dialogSO);

        foreach (var message in _dialogSO.Messages)
        {
            _npcGameObject.Interact();
            var currentMessage = _npcGameObject.DialogData;

            Assert.AreEqual(message, currentMessage);
        }
    }
}