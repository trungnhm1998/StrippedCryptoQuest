using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Character.ScriptableObjects;
using UnityEngine;

public class MapEntrance : MonoBehaviour
{
    public CharacterBehaviour.EFacingDirection entranceFacingDirection;
    public CharacterArgsEventChannelSO updatePlayerStateEvent;
    private void Start()
    {
        CharacterArgs characterArgs = new CharacterArgs();
        characterArgs.position = transform.position;
        characterArgs.facingDirection = entranceFacingDirection;
        updatePlayerStateEvent.RaiseEvent(characterArgs);
    }
}
