using System;
using System.Collections.Generic;
using Core.Runtime.Character;
using Core.Runtime.Events.ScriptableObjects.Dialogs;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class InteractWithNpcTests
{
    private DialogsScriptableObject _dialogSO;

    [SetUp]
    public void Setup()
    {
        _dialogSO = ScriptableObject.CreateInstance<DialogsScriptableObject>();
        _dialogSO.messages = new List<string>();
    }

    [Test]
    public void Interact_WithNewDialogSO_ShouldReturnCorrectDataFromSO()
    {
        var mockMessage = "Hello World";
        _dialogSO.messages.Add(mockMessage);

        var npcGameObject = new GameObject();
        IDialog dialog = npcGameObject.AddComponent<NPC>();

        dialog.SetDialog(_dialogSO);

        IInteractable interactable = npcGameObject.GetComponent<NPC>();

        var message = interactable.Interact();

        Assert.AreEqual(mockMessage, message);
    }

    [Test]
    public void Interact_WithNewEmptyMessageDialogSO_ShouldReturnEmptyString()
    {
        var npcGameObject = new GameObject();
        IDialog dialog = npcGameObject.AddComponent<NPC>();

        dialog.SetDialog(_dialogSO);

        IInteractable interactable = npcGameObject.GetComponent<NPC>();

        var message = interactable.Interact();

        Assert.AreEqual(String.Empty, message);
    }

    [Test]
    public void Interact_WithTwoInteract_ShouldReturnLastMessage()
    {
        // add random message in the list
        var mockMessage = new List<string>() { "Hello World", "Hello World 2" };
        _dialogSO.messages.AddRange(mockMessage);

        var npcGameObject = new GameObject();
        IDialog dialog = npcGameObject.AddComponent<NPC>();

        dialog.SetDialog(_dialogSO);

        IInteractable interactable = npcGameObject.GetComponent<NPC>();

        foreach (var message in _dialogSO.messages)
        {
            var currentMessage = interactable.Interact();

            Assert.AreEqual(message, currentMessage);
        }
    }
}