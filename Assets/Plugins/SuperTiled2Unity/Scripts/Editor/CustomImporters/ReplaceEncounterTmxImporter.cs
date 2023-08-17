using System.Collections.Generic;
using System.Linq;
using Plugins.SuperTiled2Unity.Scripts.Editor.Settings;
using SuperTiled2Unity;
using SuperTiled2Unity.Editor;
using UnityEngine;

namespace Plugins.SuperTiled2Unity.Scripts.Editor.CustomImporters
{
    [AutoCustomTmxImporter()]
    public class ReplaceEncounterTmxImporter : CustomTmxImporter
    {
        public override void TmxAssetImported(TmxAssetImportedArgs args)
        {
            var context = args.AssetImporter.SuperImportContext;
            var setting = context.Settings as ST2CQSettings;
            var supers = args.ImportedSuperMap.GetComponentsInChildren<SuperObject>();
            var objectsById = supers.ToDictionary(so => so.m_Id, so => so.gameObject);
            var goToDestroy = new List<GameObject>();

            SetUpEncounterArea(supers, setting, context, goToDestroy, objectsById);

            SetUpCustomProperties(supers, objectsById);

            DestroyOldReplacedObjects(goToDestroy);
        }

        #region Private methods

        private void SetUpEncounterArea(SuperObject[] supers, ST2CQSettings setting, SuperImportContext context,
            List<GameObject> goToDestroy, Dictionary<int, GameObject> objectsById)
        {
            foreach (var so in supers)
            {
                var typePrefabReplacement = setting.CrytpoQuestPrefabReplacement;
                if (so.m_Type != typePrefabReplacement.m_TypeName) continue;
                var prefab = typePrefabReplacement.m_Prefab;
                if (prefab != null)
                {
                    var instance = GameObject.Instantiate(prefab);
                    instance.transform.SetParent(so.transform.parent);
                    instance.transform.position = so.transform.position + prefab.transform.localPosition;
                    instance.transform.rotation = so.transform.rotation;
                    var width = so.m_Width;
                    var height = so.m_Height;
                    var collider = instance.AddComponent<BoxCollider2D>();
                    if (width == 0 && height == 0)
                    {
                        collider.size = context.MakeSize(10, 10);
                    }
                    else
                    {
                        collider.offset = context.MakePoint(width * 0.5f, height * 0.5f);
                        collider.size = context.MakeSize(width, height);
                    }

                    collider.isTrigger = true;

                    instance.name = so.gameObject.name;

                    goToDestroy.Add(so.gameObject);
                    objectsById[so.m_Id] = instance;
                }
            }
        }

        private void SetUpCustomProperties(SuperObject[] supers, Dictionary<int, GameObject> objectsById)
        {
            foreach (var so in supers)
            {
                var props = so.GetComponent<SuperCustomProperties>();
                if (props != null)
                {
                    foreach (var p in props.m_Properties)
                    {
                        objectsById[so.m_Id].BroadcastProperty(p, objectsById);
                    }
                }
            }
        }

        private static void DestroyOldReplacedObjects(List<GameObject> goToDestroy)
        {
            foreach (var go in goToDestroy)
            {
                Object.DestroyImmediate(go);
            }
        }

        #endregion
    }
}