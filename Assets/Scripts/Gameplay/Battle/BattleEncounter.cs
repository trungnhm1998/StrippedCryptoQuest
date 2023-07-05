using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class BattleEncounter : MonoBehaviour
    {
        [SerializeField] private TriggerBattleEncounterEventSO _triggerBattleEncounterEvent;
        [SerializeField] private BattleDataSO _battleData;
        [SerializeField] private BoxCollider2D _collider;
    }
}