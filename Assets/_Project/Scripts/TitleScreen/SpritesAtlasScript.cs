using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

namespace CryptoQuestClient
{
    public class SpritesAtlasScript : MonoBehaviour
    {
        [SerializeField] private SpriteAtlas _spritesAtlas;
        [SerializeField] private string _spriteName;

        void Start()
        {
            GetComponent<Image>().sprite = _spritesAtlas.GetSprite(_spriteName);
        }
    }
}