using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.Encounter;
using SuperTiled2Unity;
using SuperTiled2Unity.Editor;
using UnityEngine;

namespace CryptoQuestEditor.SuperTiled2Unity
{
    /// <summary>
    /// To replace any tiled object with a prefab and a collider box attached to it. with correct size and offset.
    /// </summary>
    [AutoCustomTmxImporter()]
    public class PrefabWithBoxCollider2D : CustomTmxImporter
    {
        private ST2USettings _settings;
        private SuperImportContext _context;
        private const string POLY_ENCOUNTER = "unity:IsPolyEncounter";

        public override void TmxAssetImported(TmxAssetImportedArgs args)
        {
            _context = args.AssetImporter.SuperImportContext;
            _settings = _context.Settings as ST2USettings;

            var supers = args.ImportedSuperMap.GetComponentsInChildren<SuperObject>();
            var objectsById = supers.ToDictionary(so => so.m_Id, so => so.gameObject);
            var goToDestroy = new List<GameObject>();

            ReplaceTiledObjectWithPrefab(supers, _context, goToDestroy, objectsById);

            SetUpCustomProperties(supers, objectsById);

            DestroyOldReplacedObjects(goToDestroy);
        }

        #region Private methods

        private void ReplaceTiledObjectWithPrefab(SuperObject[] supers, SuperImportContext context,
            List<GameObject> goToDestroy, Dictionary<int, GameObject> objectsById)
        {
            foreach (var so in supers)
            {
                var properties = so.GetComponent<SuperCustomProperties>();
                if (properties.TryGetCustomProperty(POLY_ENCOUNTER, out CustomProperty isPolyEncounter))
                {
                    if (isPolyEncounter.GetValueAsBool())
                    {
                        if (so.m_Type == "OverlapEncounter")
                            SetupOverlapping(objectsById, so, properties);
                        else
                        {
                            objectsById[so.m_Id].AddComponent<EncounterZone>();
                        }

                        continue;
                    }
                }

                var prefab = GetPrefabReplacement(so.m_Type);
                if (prefab == null) continue;

                var instance = Object.Instantiate(prefab, so.transform.parent, true);
                var transform = so.transform;
                instance.transform.position = transform.position + prefab.transform.localPosition;
                instance.transform.rotation = transform.rotation;

                var width = so.m_Width;
                var height = so.m_Height;

                AddTrigger2DColliderBox(context, instance, width, height);

                var gameObject = so.gameObject;
                instance.name = gameObject.name;

                goToDestroy.Add(gameObject);
                objectsById[so.m_Id] = instance;
            }
        }

        private void SetupOverlapping(Dictionary<int, GameObject> objectsById, SuperObject so,
            SuperCustomProperties properties)
        {
            var zone = objectsById[so.m_Id].AddComponent<OverlappingEncounterZone>();
            if (!properties.TryGetCustomProperty("Priority", out CustomProperty priority)) return;
            zone.Priority(priority.GetValueAsInt());
        }

        private static void AddTrigger2DColliderBox(SuperImportContext context, GameObject instance, float width,
            float height)
        {
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
        }

        private void SetUpCustomProperties(SuperObject[] supers, Dictionary<int, GameObject> objectsById)
        {
            foreach (var so in supers)
            {
                if (!so.TryGetComponent<SuperCustomProperties>(out var props)) continue;
                PostProcessObject(so, objectsById);
                foreach (var p in props.m_Properties)
                {
                    objectsById[so.m_Id].BroadcastProperty(p, objectsById);
                }
            }
        }

        private void PostProcessObject(SuperObject so, Dictionary<int, GameObject> objectsById,
            bool useCustomProp = false)
        {
            var properties = so.GetComponent<SuperCustomProperties>();
            var go = objectsById[so.m_Id];
            var collider = go.GetComponent<Collider2D>();

            if (collider != null)
            {
                if (properties.TryGetCustomProperty(StringConstants.Unity_IsTrigger, out CustomProperty isTrigger))
                    collider.isTrigger = _context.GetIsTriggerOverridable(isTrigger.GetValueAsBool());
                else
                    collider.isTrigger = true;
            }
            else
            {
                Debug.LogWarning($"Object '{go.name}' has no collider.");
            }
        }

        private static void DestroyOldReplacedObjects(List<GameObject> goToDestroy)
        {
            foreach (var go in goToDestroy)
            {
                Object.DestroyImmediate(go);
            }
        }

        private GameObject GetPrefabReplacement(string type)
        {
            var replacement = _settings.CustomPrefabReplacements.FirstOrDefault(r => r.m_TypeName == type);

            return replacement?.m_Prefab;
        }

        #endregion
    }
}