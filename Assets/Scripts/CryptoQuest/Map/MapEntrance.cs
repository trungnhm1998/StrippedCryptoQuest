using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Character.ScriptableObjects;
using UnityEngine;
namespace CryptoQuest.Map
{
    public class MapEntrance : MonoBehaviour
    {
        public MapTransitionSO transitionSO;
        public MapPathSO mapPath;

        public CharacterBehaviour.EFacingDirection entranceFacingDirection;
        public CharacterArgsEventChannelSO updatePlayerStateEvent;
        private void Start()
        {
            if (transitionSO.currentMapPath == null)
            {
                SetUpDefault();
                return;
            };
            if (mapPath != transitionSO.currentMapPath) return;
            CharacterArgs characterArgs = new CharacterArgs();
            characterArgs.position = transform.position;
            characterArgs.facingDirection = entranceFacingDirection;
            updatePlayerStateEvent.RaiseEvent(characterArgs);
            transitionSO.currentMapPath = null;
        }
        private void SetUpDefault()
        {
           // set up default map entrance
        }
    }
}