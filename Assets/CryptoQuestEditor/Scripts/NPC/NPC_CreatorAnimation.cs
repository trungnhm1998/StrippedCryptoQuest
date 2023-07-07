using System.Collections;
using System.Collections.Generic;
using CryptoQuest;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.NPC
{
    public class NPC_CreatorAnimation : EditorWindow
    {
        private string _spriteLocation;
        private string _spriteName;
        private string _assetPath;
        private GameObject _currentNPC;
        private SpriteRenderer _currentSprite;
        private NpcMovement _spriteNPC;

        [MenuItem("Window/NPC/Animation")]
        public static void ShowWindow()
        {
            GetWindow<NPC_CreatorAnimation>("Animation");
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Current NPC", EditorStyles.label);
            _currentNPC = (GameObject)EditorGUILayout.ObjectField(_currentNPC, typeof(GameObject), true, GUILayout.MaxWidth(264));
            GUILayout.EndHorizontal();
            GUILayout.Space(30);

            if (GUILayout.Button("Get Sprite Direction", GUILayout.Height(60)))
            {
                CreateSprite();
            }
        }

        private void CreateSprite()
        {
            InitializedSetup();
        }


        private void InitializedSetup()
        {
            if (_currentNPC.GetComponent<NpcMovement>() == null) _currentNPC.AddComponent<NpcMovement>();
            _spriteNPC = _currentNPC.GetComponent<NpcMovement>();
            _currentSprite = _currentNPC.GetComponent<SpriteRenderer>();
            string spritePath = AssetDatabase.GetAssetPath(_currentSprite.sprite);
            string spriteName = _currentSprite.sprite.name;
            _assetPath = spritePath.TrimEnd('/').Remove(spritePath.LastIndexOf('/') + 1);
            _spriteName = spriteName.TrimEnd('_').Remove(spriteName.LastIndexOf("_") + 1);
            _spriteLocation = spriteName.TrimEnd('_').Remove(spriteName.LastIndexOf("_"));
            GetSprites();

        }

        private void GetSprites()
        {
            UnityEngine.Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(_assetPath + _spriteLocation + ".png");

            var components = _currentNPC.GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                Debug.Log(component);
            }
            for (int i = 0; i < sprites.Length; i++)
            {
                foreach (UnityEngine.Object sprite in sprites)
                {
                    if (sprite.name == _spriteName + i)
                    {
                        AddSpriteToList(i, sprite);
                    }
                }
            }
        }

        private void AddSpriteToList(int currentSprite, UnityEngine.Object sprite)
        {
            switch (currentSprite)
            {
                case > 3 and <= 7:
                    _spriteNPC._moveDown.Add(sprite as Sprite);
                    break;
                case > 7 and <= 11:
                    _spriteNPC._moveLeft.Add(sprite as Sprite);
                    break;
                case > 11 and <= 15:
                    _spriteNPC._moveTop.Add(sprite as Sprite);
                    break;
                case > 15 and <= 19:
                    _spriteNPC._moveRight.Add(sprite as Sprite);
                    break;
                default:
                    break;
            }
        }
    }
}