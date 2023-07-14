using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

namespace CryptoQuest.Gameplay.Battle
{
    [CreateAssetMenu(fileName = "PagingDialogEventChannelSO", menuName = "Gameplay/Battle/Events/Paging Dialog Event")]
    public class PagingDialogEventChannelSO : ScriptableObject
    {
        public UnityAction<PagingDialog> EventRaised;

        public void RaiseEvent(PagingDialog pagingDialog)
        {
            if (EventRaised == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return;
            }

            EventRaised.Invoke(pagingDialog);
        }
    }

    [Serializable]
    public class PagingDialog
    {
        public List<DialogPage> Pages = new();
    }

    [Serializable]
    public class DialogPage
    {
        public List<string> Lines = new();
    }
}