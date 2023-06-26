using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class NPC_CreatorWindow : EditorWindow
{
    private string exportPath;
    private string nameNPC;
    private Sprite spriteNPC;
    private bool isCollider;


    [MenuItem("Window/NPC/NPC Creator")]
    public static void ShowWindow()
    {
        GetWindow<NPC_CreatorWindow>("NPC Creator");
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("NPC Name", EditorStyles.label);
        nameNPC = EditorGUILayout.TextField(nameNPC, GUILayout.MaxWidth(264));
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Sprite", EditorStyles.label);
        spriteNPC = (Sprite)EditorGUILayout.ObjectField(spriteNPC, typeof(Sprite), true, GUILayout.MaxWidth(264));
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Check Collider", EditorStyles.label);
        isCollider = EditorGUILayout.Toggle(isCollider);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Export Path", EditorStyles.label);
        exportPath = EditorGUILayout.TextField(exportPath, GUILayout.MaxWidth(264));
        GUILayout.EndHorizontal();


        GUILayout.Space(50);

        if (GUILayout.Button("Create"))
        {
            CreateNPC(nameNPC, spriteNPC, isCollider);
        }
    }

    private void CreateNPC(string _nameNPC, Sprite _spriteNPC, bool _checkCollider)
    {
        GameObject newNPC = new GameObject();
        newNPC.AddComponent<SpriteRenderer>();

        SpriteRenderer spriteNPC = newNPC.GetComponent<SpriteRenderer>();
        spriteNPC.sprite = _spriteNPC;

        if (_checkCollider)
        {
            newNPC.AddComponent<BoxCollider2D>();
            BoxCollider2D collider2D = newNPC.GetComponent<BoxCollider2D>();
            Vector2 spriteSize = new Vector2(spriteNPC.sprite.rect.width * 0.01f, spriteNPC.sprite.rect.height * 0.01f);
            collider2D.size = spriteSize;
            collider2D.isTrigger = true;
        }

        bool prefabSuccess;
        PrefabUtility.SaveAsPrefabAsset(newNPC, exportPath + _nameNPC + ".prefab", out prefabSuccess);

        if (prefabSuccess == true)
        {
            DestroyImmediate(newNPC);
        }
        else
        {
            Debug.Log("Prefab failed to save" + prefabSuccess);
        }
    }
}
