using IndiGames.Core.SaveSystem.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IndiGames.Core.SaveSystem
{
    public static class ScriptableObjectRegistry
    {
        private static List<SerializableScriptableObject> _scriptableObjects = new List<SerializableScriptableObject>();

        public static void AddScriptableObject(SerializableScriptableObject scriptableObject)
        {
            _scriptableObjects.Add(scriptableObject);
        }

        public static SerializableScriptableObject FindByGuid(string guid)
        {
            foreach (var item in _scriptableObjects)
            {
                if (item.Guid == guid)
                {
                    return item;
                }
            }
            return null;
        }
    }
}