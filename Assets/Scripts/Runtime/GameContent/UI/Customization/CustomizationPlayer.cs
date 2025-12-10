using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.GameContent.UI.Customization
{
    public class CustomizationPlayer : MonoBehaviour
    {
        #region Properties
        public enum BodyPartType
        {
            Horns,
            Hair,
            Eyes,
            Body,
            Tail
        }

        [System.Serializable]
        public class BodyPartData
        {
            public BodyPartType bodyPartType;
            public CustomizeItem[] prefabArray;
            public Transform attachTransform;

            [NonSerialized] public GameObject currentInstance;
            [NonSerialized] public int currentIndex = 0;
            [NonSerialized] public List<GameObject> instances;
        }
        #endregion

        #region Functions
        private void Start()
        {
            if (_preloadPrefabs)
                PreloadAllPrefabs();
            ActivateCurrentInstances();
            Load();
        }

        private void PreloadAllPrefabs()
        {
            foreach (var data in bodyPartDataArray)
            {
                if (data == null || data.prefabArray == null) continue;
                data.instances = new List<GameObject>(data.prefabArray.Length);
                var parent = data.attachTransform != null ? data.attachTransform : this.transform;

                for (int i = 0; i < data.prefabArray.Length; i++)
                {
                    var prefab = data.prefabArray[i].ItemPrefab;
                    if (prefab == null)
                    {
                        data.instances.Add(null);
                        continue;
                    }

                    var inst = Instantiate(prefab, parent);
                    inst.transform.SetParent(parent, false);
                    inst.transform.localPosition = Vector3.zero;
                    inst.transform.localRotation = Quaternion.identity;
                    inst.transform.localScale = Vector3.one;

                    inst.SetActive(false); // keep inactive until selected
                    data.instances.Add(inst);
                }
            }
        }
        
        private void ActivateCurrentInstances()
        {
            foreach (var data in bodyPartDataArray)
            {
                if (data == null) continue;
                if (data.instances != null && data.currentIndex >= 0 && data.currentIndex < data.instances.Count)
                {
                    data.currentInstance = data.instances[data.currentIndex];
                    if (data.currentInstance != null)
                        data.currentInstance.SetActive(true);
                }
            }
        }

        public void SetBodyPartPrefab(BodyPartType bodyPartType, int index)
        {
            var data = GetBodyPartData(bodyPartType);
            if (data == null || data.prefabArray == null || data.prefabArray.Length == 0) return;
            int clamped = Mathf.Clamp(index, 0, data.prefabArray.Length - 1);

            // If we preloaded and have an instance ready, just toggle
            if (data.instances != null && data.instances.Count > clamped && data.instances[clamped] != null)
            {
                if (data.currentInstance != null)
                    data.currentInstance.SetActive(false);

                data.currentInstance = data.instances[clamped];
                data.currentInstance.SetActive(true);
            }
            else
            {
                // Fallback: destroy previous and instantiate the selected prefab (existing behavior)
                if (data.currentInstance != null)
                    Destroy(data.currentInstance);

                var prefab = data.prefabArray[clamped].ItemPrefab;
                if (prefab != null)
                {
                    var parent = data.attachTransform != null ? data.attachTransform : this.transform;
                    data.currentInstance = Instantiate(prefab, parent);
                    data.currentInstance.transform.SetParent(parent, false);
                    data.currentInstance.transform.localPosition = Vector3.zero;
                    data.currentInstance.transform.localRotation = Quaternion.identity;
                    data.currentInstance.transform.localScale = Vector3.one;
                }
                else
                {
                    data.currentInstance = null;
                }
            }

            data.currentIndex = clamped;
        }
        
        public void ApplyMaterialToBodyPart(BodyPartType bodyPartType, Material mat)
        {
            var data = GetBodyPartData(bodyPartType);
            if (data == null) return;

            // Try the currentInstance first, fallback to instances[currentIndex]
            GameObject target = data.currentInstance;
            if (target == null && data.instances != null && data.currentIndex >= 0 && data.currentIndex < data.instances.Count)
                target = data.instances[data.currentIndex];

            if (target == null) return;

            // Apply the material to every Renderer in the object (covers MeshRenderer and SkinnedMeshRenderer)
            var renderers = target.GetComponentsInChildren<Renderer>(true);
            foreach (var r in renderers)
            {
                var shared = r.sharedMaterials;
                if (shared == null || shared.Length == 0)
                {
                    r.material = mat;
                    continue;
                }

                // Replace all material slots with the selected material (keeps slot count)
                var newMats = new Material[shared.Length];
                for (int i = 0; i < newMats.Length; i++)
                    newMats[i] = mat;
                r.materials = newMats;
            }
        }

        public CustomizeItem[] GetPrefabs(BodyPartType bodyPartType)
        {
            var data = GetBodyPartData(bodyPartType);
            if (data == null || data.prefabArray == null) return new CustomizeItem[0];
            return data.prefabArray;
        }

        private BodyPartData GetBodyPartData(BodyPartType bodyPartType)
        {
            foreach (var bodyPartData in bodyPartDataArray)
            {
                if (bodyPartData.bodyPartType == bodyPartType) return bodyPartData;
            }

            return null;
        }

        /* Mesh changing functionality.
        public void ChangeBodyPart(BodyPartType bodyPartType)
        {
            BodyPartData bodyPartData = GetBodyPartData(bodyPartType);
            int meshIndex = Array.IndexOf(bodyPartData.meshArray, bodyPartData.skinnedMeshRenderer.sharedMesh);
            bodyPartData.skinnedMeshRenderer.sharedMesh = bodyPartData.meshArray[(meshIndex + 1) % bodyPartData.meshArray.Length];
        }

        // Set a specific mesh by index for a body part
        public void SetBodyPartMesh(BodyPartType bodyPartType, int index)
        {
            BodyPartData bodyPartData = GetBodyPartData(bodyPartType);
            if (bodyPartData == null || bodyPartData.meshArray == null || bodyPartData.meshArray.Length == 0) return;
            int clamped = Mathf.Clamp(index, 0, bodyPartData.meshArray.Length - 1);
            bodyPartData.skinnedMeshRenderer.sharedMesh = bodyPartData.meshArray[clamped];
        }

        // Get meshes for a body part (returns empty array if not found)
        public Mesh[] GetMeshes(BodyPartType bodyPartType)
        {
            BodyPartData bodyPartData = GetBodyPartData(bodyPartType);
            if (bodyPartData == null || bodyPartData.meshArray == null) return new Mesh[0];
            return bodyPartData.meshArray;
        }

        private BodyPartData GetBodyPartData(BodyPartType bodyPartType)
        {
            foreach (var bodyPartData in bodyPartDataArray)
            {
                if (bodyPartData.bodyPartType == bodyPartType)
                {
                    return bodyPartData;
                }
            }
            return null;
        }

        public SkinnedMeshRenderer GetSkinnedMeshRenderer(BodyPartType bodyPartType)
        {
            BodyPartData bodyPartData = GetBodyPartData(bodyPartType);
            return bodyPartData != null ? bodyPartData.skinnedMeshRenderer : null;
        }
        */

        [Serializable]
        public class BodyPartTypeIndex
        {
            public BodyPartType bodyPartType;
            public int index;
        }

        public class SaveObject
        {
            public List<BodyPartTypeIndex> bodyPartTypeIndexList;
        }
        
        public void Save()
        {
            List<BodyPartTypeIndex> bodyPartTypeIndexList = new List<BodyPartTypeIndex>();

            foreach (BodyPartType bodyPartType in Enum.GetValues(typeof(BodyPartType)))
            {
                BodyPartData bodyPartData = GetBodyPartData(bodyPartType);
                int meshIndex = bodyPartData != null ? bodyPartData.currentIndex : 0;
                bodyPartTypeIndexList.Add(new BodyPartTypeIndex
                {
                    bodyPartType = bodyPartType,
                    index = meshIndex
                });
            }

            SaveObject saveObject = new SaveObject
            {
                bodyPartTypeIndexList = bodyPartTypeIndexList
            };

            string json = JsonUtility.ToJson(saveObject);
            Debug.Log(json);
            PlayerPrefs.SetString(PLAYER_PREFS_KEY, json);
        }

        public void Load()
        {
            string json = PlayerPrefs.GetString(PLAYER_PREFS_KEY);
            if (string.IsNullOrEmpty(json)) return;
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(json);
            if (saveObject == null || saveObject.bodyPartTypeIndexList == null) return;

            foreach (var bodyPartTypeIndex in saveObject.bodyPartTypeIndexList)
            {
                BodyPartData bodyPartData = GetBodyPartData(bodyPartTypeIndex.bodyPartType);
                if (bodyPartData == null || bodyPartData.prefabArray == null || bodyPartData.prefabArray.Length == 0) continue;
                int clamped = Mathf.Clamp(bodyPartTypeIndex.index, 0, bodyPartData.prefabArray.Length - 1);
                SetBodyPartPrefab(bodyPartTypeIndex.bodyPartType, clamped);
            }
        }

        /* Save and Load functionality for meshes.
        private void Save()
        {
            List<BodyPartTypeIndex> bodyPartTypeIndexList = new List<BodyPartTypeIndex>();

            foreach (BodyPartType bodyPartType in Enum.GetValues(typeof(BodyPartType)))
            {
                BodyPartData bodyPartData = GetBodyPartData(bodyPartType);
                int meshIndex = Array.IndexOf(bodyPartData.meshArray, bodyPartData.skinnedMeshRenderer.sharedMesh);
                bodyPartTypeIndexList.Add(new BodyPartTypeIndex
                {
                    bodyPartType = bodyPartType,
                    index = meshIndex
                });
            }

            SaveObject saveObject = new SaveObject
            {
                bodyPartTypeIndexList = bodyPartTypeIndexList
            };

            string json = JsonUtility.ToJson(saveObject);
            Debug.Log(json);
            PlayerPrefs.SetString(PLAYER_PREFS_KEY, json);
        }

        private void Load()
        {
            string json = PlayerPrefs.GetString(PLAYER_PREFS_KEY);
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(json);

            foreach (var bodyPartTypeIndex in saveObject.bodyPartTypeIndexList)
            {
                BodyPartData bodyPartData = GetBodyPartData(bodyPartTypeIndex.bodyPartType);
                bodyPartData.skinnedMeshRenderer.sharedMesh = bodyPartData.meshArray[bodyPartTypeIndex.index];
            }
        }*/
        #endregion
        
        #region Fields
        private const string PLAYER_PREFS_KEY = "PlayerCustom";

        [SerializeField] private BodyPartData[] bodyPartDataArray;
        public BodyPartData[] BodyPartDataArray => bodyPartDataArray;

        [Tooltip("Optional mesh for the player head")]
        [SerializeField] private GameObject _playerHeadMesh;
        
        [Tooltip("If true, all prefabs will be instantiated once at Start and toggled on/off when changing.")]
        [SerializeField] private bool _preloadPrefabs = true;
        #endregion
    }
}