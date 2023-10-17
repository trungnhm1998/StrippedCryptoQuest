using System;
using CryptoQuest.Gameplay.Encounter;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.Battle.Events
{
    [UnityEngine.CreateAssetMenu(fileName = "BattleResultEvent", menuName = "Crypto Quest/Events/Battle Result Event")]
    public class BattleResultEventSO : GenericEventChannelSO<Battlefield>
    {
    }
}