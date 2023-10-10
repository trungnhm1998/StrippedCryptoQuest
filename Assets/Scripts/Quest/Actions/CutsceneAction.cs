using System.Collections;
using CryptoQuest.System.CutsceneSystem.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Actions
{
    [CreateAssetMenu(menuName = "QuestSystem/Actions/CutsceneAction")]
    public class CutsceneAction : NextAction
    {
        public QuestCutsceneDef CutsceneDef;
        public float Delay = 0.5f;

        public override IEnumerator Execute()
        {
            yield return new WaitForSeconds(Delay);

            CutsceneDef.RaiseEvent();
        }
    }
}