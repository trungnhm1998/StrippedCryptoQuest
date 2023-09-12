using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events
{
    [CreateAssetMenu(fileName = "PagingDialogEventChannelSO", menuName = "Gameplay/Battle/Events/Paging Dialog Event")]
    public class PagingDialogEventChannelSO : GenericEventChannelSO<PagingDialog> { }

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