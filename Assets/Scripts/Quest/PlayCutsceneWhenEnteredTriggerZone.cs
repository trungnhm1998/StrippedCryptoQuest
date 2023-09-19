using UnityEngine;

namespace CryptoQuest.Quest
{
    public class PlayCutsceneWhenEnteredTriggerZone : PlayQuestCutsceneBase
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                PlayCutscene();
            }
        }
    }
}