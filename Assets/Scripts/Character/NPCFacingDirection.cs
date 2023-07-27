using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Gameplay;
using UnityEngine;

namespace CryptoQuest.Character
{
    public class NPCFacingDirection : CharacterBehaviour
    {
        [SerializeField] private GameplayBus _bus;

        public void FacePlayer()
        {
            SetFacingDirection(new NpcFacingStrategy().Execute(transform.position, _bus.Hero.transform.position));
        }
    }
}