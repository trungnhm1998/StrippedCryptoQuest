using System.Reflection;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System;

public class NPC_CreatorWindow : EditorWindow
{
    private NPC_Localize _localize = new NPC_Localize();
    private string _nameNPC;
    private string _updateNameNPC;
    private bool _isCollider;
    private bool _updateCollider;
    private string _exportPath;
    private Sprite _spriteNPC;
    private Sprite _updateSpriteNPC;
    private GameObject _baseNPC;
    private GameObject _currentNPC;
    private List<GameObject> _listNPC;
    private SpriteRenderer _spriteRenderer;


    [MenuItem("Window/NPC/NPC Creator")]
    public static void ShowWindow()
    {
        GetWindow<NPC_CreatorWindow>("NPC Creator");
    }

    private void OnGUI()
    {
        if (Application.systemLanguage == SystemLanguage.English)
        {
            _localize.InitializeSetup("en");
        }
        else
        {
            _localize.InitializeSetup("ja");
        }

        GUIStyle titleFontStyle = new GUIStyle();
        titleFontStyle.fontSize = 26;
        titleFontStyle.normal.textColor = Color.gray;
        titleFontStyle.fontStyle = FontStyle.Bold;
        titleFontStyle.alignment = TextAnchor.UpperLeft;
        titleFontStyle.padding.bottom = 10;

        // NPC Tools UI
        GUILayout.Label(_localize._createTitle, titleFontStyle);
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label(_localize._name, EditorStyles.label);
        _nameNPC = EditorGUILayout.TextField(_nameNPC, GUILayout.MaxWidth(264));
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label(_localize._sprite, EditorStyles.label);
        _spriteNPC = (Sprite)EditorGUILayout.ObjectField(_spriteNPC, typeof(Sprite), true, GUILayout.MaxWidth(264));
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label(_localize._collider, EditorStyles.label);
        _isCollider = EditorGUILayout.Toggle(_isCollider);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label(_localize._exportPath, EditorStyles.label);
        _exportPath = EditorGUILayout.TextField(_exportPath, GUILayout.MaxWidth(264));
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label(_localize._baseNPC, EditorStyles.label);
        _baseNPC = (GameObject)EditorGUILayout.ObjectField(_baseNPC, typeof(GameObject), true, GUILayout.MaxWidth(264));
        GUILayout.EndHorizontal();
        GUILayout.Space(30);

        if (GUILayout.Button(_localize._buttonCreate, GUILayout.Height(60)))
        {
            CreateNPC(_nameNPC, _spriteNPC, _isCollider);
        }
        GUILayout.Space(30);

        //UPDATE

        GUILayout.Label(_localize._updateTitle, titleFontStyle);
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label(_localize._selectNPC, EditorStyles.label);
        _currentNPC = (GameObject)EditorGUILayout.ObjectField(_currentNPC, typeof(GameObject), true, GUILayout.MaxWidth(264));
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label(_localize._name, EditorStyles.label);
        _updateNameNPC = EditorGUILayout.TextField(_updateNameNPC, GUILayout.MaxWidth(264));
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label(_localize._sprite, EditorStyles.label);
        _updateSpriteNPC = (Sprite)EditorGUILayout.ObjectField(_updateSpriteNPC, typeof(Sprite), true, GUILayout.MaxWidth(264));
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label(_localize._collider, EditorStyles.label);
        _updateCollider = EditorGUILayout.Toggle(_updateCollider);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.Space(10);
        if (GUILayout.Button(_localize._btnUpdate, GUILayout.Height(60)))
        {
            UpdateNPC();
        }
        if (GUILayout.Button(_localize._btnUpdateAll, GUILayout.Height(60)))
        {
            GetListNPC();
            UpdateAllNPC();
        }
        GUILayout.Space(50);
    }
    private void GetListNPC()
    {
        _listNPC = new List<GameObject>();
        string[] pathsToAssets = AssetDatabase.FindAssets("t:Prefab NPC_");
        foreach (string paths in pathsToAssets)
        {
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(paths));
            _listNPC.Add(go);
        }
    }

    private void CheckCollider(GameObject npc, bool interactable)
    {
        if (npc.GetComponent<BoxCollider2D>() == null)
        {
            if (interactable)
            {
                npc.AddComponent<BoxCollider2D>();
                BoxCollider2D collider2D = npc.GetComponent<BoxCollider2D>();
                collider2D.size = new Vector2(_spriteRenderer.sprite.rect.width * 0.01f, (_spriteRenderer.sprite.rect.height * 0.01f) / 2);
                collider2D.offset = new Vector2(collider2D.offset.x, (collider2D.offset.y) / 2);
                collider2D.isTrigger = true;
            }
        }
        else
        {
            if (!interactable)
            {
                BoxCollider2D collider2D = npc.GetComponent<BoxCollider2D>();
                DestroyImmediate(collider2D, true);
            }
        }
    }
    private void CreateNPC(string nameNPC, Sprite spriteNPC, bool checkCollider)
    {
        GetListNPC();
        foreach (GameObject npc in _listNPC)
        {
            if (npc.name == nameNPC) return;
        }
        GameObject newNPC = Instantiate(_baseNPC);
        newNPC.name = nameNPC;
        newNPC.AddComponent<SpriteRenderer>();

        _spriteRenderer = newNPC.GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = spriteNPC;

        CheckCollider(newNPC, _isCollider);

        bool prefabSuccess;
        PrefabUtility.SaveAsPrefabAsset(newNPC, _exportPath + nameNPC + ".prefab", out prefabSuccess);

        if (prefabSuccess == true)
        {
            DestroyImmediate(newNPC);
        }
        else
        {
            Debug.Log("Prefab failed to save: " + prefabSuccess);
        }
    }

    private void UpdateNPC()
    {
        if (_currentNPC != null)
        {
            string myPath = AssetDatabase.GetAssetPath(_currentNPC);
            _spriteRenderer = _currentNPC.GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _updateSpriteNPC;
            AssetDatabase.RenameAsset(myPath, _updateNameNPC);

            var components = _baseNPC.GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                if (_currentNPC.GetComponent(component.GetType()) == null) _currentNPC.AddComponent(component.GetType());
            }
            CheckCollider(_currentNPC, _updateCollider);
        }
    }

    private void UpdateAllNPC()
    {
        foreach (GameObject npc in _listNPC)
        {
            // CheckCollider(npc, _updateCollider);
            var components = _baseNPC.GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                if (npc.GetComponent(component.GetType()) == null)
                {
                    npc.AddComponent(component.GetType());
                }
            }
        }
    }
}
