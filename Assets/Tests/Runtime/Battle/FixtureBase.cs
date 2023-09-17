using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CryptoQuest.Tests.Runtime.Battle
{
    public class FixtureBase
    {
        private readonly List<GameObject> _gameObjectsToDestroy = new();

        internal GameObject CreateGameObject(string name, params Type[] components)
        {
            var go = new GameObject();
            _gameObjectsToDestroy.Add(go);
            go.name = name;

            foreach (var c in components)
                if (c.IsSubclassOf(typeof(Component)))
                    go.AddComponent(c);

            return go;
        }

        internal GameObject CreateGameObjectFromPrefab(string path, string name = null)
        {
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            go = Object.Instantiate(go);
            _gameObjectsToDestroy.Add(go);
            go.name = name ?? go.name;
            return go;
        }

        [TearDown]
        public virtual void TearDown()
        {
            foreach (var go in _gameObjectsToDestroy)
                Object.DestroyImmediate(go);

            _gameObjectsToDestroy.Clear();
        }
    }
}