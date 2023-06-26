using System.Reflection;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class NPC_CreatorWindow : EditorWindow
{
    [Tooltip("NPC Variables")]
    private string _nameNPC;
    private Sprite _spriteNPC;
    private bool _isCollider;
    private string _exportPath;

    [Tooltip("List NPC")]
    private GameObject _baseNPC;
    private GameObject _currentNPC;
    private List<GameObject> _listNPC;

    [Tooltip("Current NPC Variables")]
    private SpriteRenderer _spriteRenderer;


    [MenuItem("Window/NPC/NPC Creator")]
    public static void ShowWindow()
    {
        GetWindow<NPC_CreatorWindow>("NPC Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create new NPC", EditorStyles.label);
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Name", EditorStyles.label);
        _nameNPC = EditorGUILayout.TextField(_nameNPC, GUILayout.MaxWidth(264));
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Sprite", EditorStyles.label);
        _spriteNPC = (Sprite)EditorGUILayout.ObjectField(_spriteNPC, typeof(Sprite), true, GUILayout.MaxWidth(264));
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Check Collider", EditorStyles.label);
        _isCollider = EditorGUILayout.Toggle(_isCollider);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Export Path", EditorStyles.label);
        _exportPath = EditorGUILayout.TextField(_exportPath, GUILayout.MaxWidth(264));
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Base NPC", EditorStyles.label);
        _baseNPC = (GameObject)EditorGUILayout.ObjectField(_baseNPC, typeof(GameObject), true, GUILayout.MaxWidth(264));
        GUILayout.EndHorizontal();


        GUILayout.Space(10);

        if (GUILayout.Button("Create"))
        {
            CreateNPC(_nameNPC, _spriteNPC, _isCollider);
        }

        GUILayout.Space(50);
        GUILayout.Label("Update NPC", EditorStyles.label);
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Select NPC to Update", EditorStyles.label);
        _currentNPC = (GameObject)EditorGUILayout.ObjectField(_currentNPC, typeof(GameObject), true, GUILayout.MaxWidth(264));
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        if (GUILayout.Button("Update"))
        {
            UpdateNPC();
        }


        GUILayout.Space(10);
        if (GUILayout.Button("Update All"))
        {
            GetListNPC();
            UpdateAllNPC();
        }
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

    private void CheckCollider(GameObject npc)
    {
        if (npc.GetComponent<BoxCollider2D>() == null)
        {
            if (_isCollider)
            {
                npc.AddComponent<BoxCollider2D>();
                BoxCollider2D collider2D = npc.GetComponent<BoxCollider2D>();
                Vector2 spriteSize = new Vector2(_spriteRenderer.sprite.rect.width * 0.01f, _spriteRenderer.sprite.rect.height * 0.01f);
                collider2D.size = spriteSize;
                collider2D.isTrigger = true;
            }
        }
        else
        {
            BoxCollider2D collider2D = npc.GetComponent<BoxCollider2D>();
            Destroy(collider2D);
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

        CheckCollider(newNPC);

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
            _spriteRenderer = _currentNPC.GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _spriteNPC;
            _currentNPC.name = _nameNPC;

            var components = _baseNPC.GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                if (_currentNPC.GetComponent(component.GetType()) == null) _currentNPC.AddComponent(component.GetType());
            }
            CheckCollider(_currentNPC);
        }
    }

    private void UpdateAllNPC()
    {
        foreach (GameObject npc in _listNPC)
        {
            var components = _baseNPC.GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                if (npc.GetComponent(component.GetType()) == null) npc.AddComponent(component.GetType());
            }
        }
    }
}
