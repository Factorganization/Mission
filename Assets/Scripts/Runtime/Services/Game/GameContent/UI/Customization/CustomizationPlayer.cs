using System.Collections.Generic;

namespace Runtime.Services.Game.GameContent.UI.Customization
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

        /*[Serializable]
        public class BodyPartDataPrefab
        {
            public BodyPartType bodyPartType;
            public CustomizeItem[] prefabArray;
            public Transform attachTransform;

            [NonSerialized] public GameObject currentInstance;
            [NonSerialized] public int currentIndex = 0;
            [NonSerialized] public List<GameObject> instances;
        }*/
        
        [Serializable]
        public class BodyPartData
        {
            public BodyPartType bodyPartType;
            public SkinnedMeshRenderer skinnedMeshRenderer;
            public CustomizeItem[] meshArray;
        }
        
        #endregion

        #region PrefabsFunctions
        private void Start()
        {
           
        }

        /*private void PreloadAllPrefabs()
        {
            foreach (var data in bodyPartDataArray)
            {
                if (data == null || data.prefabArray == null) continue;
                data.instances = new List<GameObject>(data.prefabArray.Length);
                var parent = data.attachTransform != null ? data.attachTransform : transform;

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
            var data = GetBodyPartDataPrefab(bodyPartType);
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
                    var parent = data.attachTransform != null ? data.attachTransform : transform;
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

        public CustomizeItem[] GetItem(BodyPartType bodyPartType)
        {
            var data = GetBodyPartDataPrefab(bodyPartType);
            if (data == null || data.prefabArray == null) return new CustomizeItem[0];
            return data.prefabArray;
        }

        private BodyPartDataPrefab GetBodyPartDataPrefab(BodyPartType bodyPartType)
        {
            foreach (var bodyPartData in bodyPartDataArray)
            {
                if (bodyPartData.bodyPartType == bodyPartType) return bodyPartData;
            }

            return null;
        }*/
        #endregion

        #region Meshes

        public void ChangeBodyPart(BodyPartType bodyPartType)
        {
            BodyPartData bodyPartData = GetBodyPartData(bodyPartType);
            int meshIndex = Array.IndexOf(bodyPartData.meshArray, bodyPartData.skinnedMeshRenderer.sharedMesh);
            bodyPartData.skinnedMeshRenderer.sharedMesh = bodyPartData.meshArray[(meshIndex + 1) % bodyPartData.meshArray.Length].ItemMesh;
        }

        // Set a specific mesh by index for a body part
        public void SetBodyPartMesh(BodyPartType bodyPartType, int index)
        {
            BodyPartData bodyPartData = GetBodyPartData(bodyPartType);
            if (bodyPartData == null || bodyPartData.meshArray == null || bodyPartData.meshArray.Length == 0) return;
            int clamped = Mathf.Clamp(index, 0, bodyPartData.meshArray.Length - 1);
            bodyPartData.skinnedMeshRenderer.sharedMesh = bodyPartData.meshArray[clamped].ItemMesh;
        }

        // Get meshes for a body part (returns empty array if not found)

        public CustomizeItem[] GetItem(BodyPartType bodyPartType)
        {
            var data = GetBodyPartData(bodyPartType);
            if (data == null || data.meshArray == null) return new CustomizeItem[0];
            return data.meshArray;
        }
        
        public void ApplyMaterialToBodyPart(BodyPartType bodyPartType, Material mat, int materialIndex = 0)
        {
            if (mat == null) return;
            
            //BodyPartType targetPart = bodyPartType == BodyPartType.Eyes ? BodyPartType.Body : bodyPartType;

            Renderer renderer = GetRendererForBodyPart(bodyPartType);
            if (renderer == null) return;

            Material[] mats = renderer.sharedMaterials;
            if (mats == null || mats.Length == 0)
            {
                renderer.sharedMaterials = new Material[] { mat };
                return;
            }

            int idx = Mathf.Clamp(materialIndex, 0, mats.Length - 1);
            mats[idx] = mat;
            renderer.sharedMaterials = mats;
        }

        private Renderer GetRendererForBodyPart(BodyPartType bodyPartType)
        {
            // Try the configured SkinnedMeshRenderer first
            SkinnedMeshRenderer sk = GetSkinnedMeshRenderer(bodyPartType);
            if (sk != null) return sk;

            // Try to find an entry in bodyPartMeshDataArray and look for a child GameObject with the same name
            if (bodyPartMeshDataArray != null)
            {
                foreach (var bp in bodyPartMeshDataArray)
                {
                    if (bp == null) continue;
                    if (bp.bodyPartType == bodyPartType)
                    {
                        // If a SkinnedMeshRenderer isn't set, try to find a renderer on a child named like the enum
                        var child = transform.Find(bodyPartType.ToString());
                        if (child != null)
                        {
                            var r = child.GetComponent<Renderer>() ?? child.GetComponentInChildren<Renderer>();
                            if (r != null) return r;
                        }
                    }
                }
            }

            // Fallback: find any renderer in children (might be the base model instance)
            return GetComponentInChildren<Renderer>();
        }
        
        private BodyPartData GetBodyPartData(BodyPartType bodyPartType)
        {
            foreach (BodyPartData bodyPartData in bodyPartMeshDataArray)
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

        #endregion
        

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
        
        #region PrefabsSaveLoad
        /*
        public void Save()
        {
            List<BodyPartTypeIndex> bodyPartTypeIndexList = new List<BodyPartTypeIndex>();

            foreach (BodyPartType bodyPartType in Enum.GetValues(typeof(BodyPartType)))
            {
                BodyPartDataPrefab bodyPartDataPrefab = GetBodyPartDataPrefab(bodyPartType);
                int meshIndex = bodyPartDataPrefab != null ? bodyPartDataPrefab.currentIndex : 0;
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
                BodyPartDataPrefab bodyPartDataPrefab = GetBodyPartDataPrefab(bodyPartTypeIndex.bodyPartType);
                if (bodyPartDataPrefab == null || bodyPartDataPrefab.prefabArray == null || bodyPartDataPrefab.prefabArray.Length == 0) continue;
                int clamped = Mathf.Clamp(bodyPartTypeIndex.index, 0, bodyPartDataPrefab.prefabArray.Length - 1);
                SetBodyPartPrefab(bodyPartTypeIndex.bodyPartType, clamped);
            }
        }
        */
        #endregion

        #region MeshesSaveLoad
        public void SaveMeshes()
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

        public void LoadMeshes()
        {
            string json = PlayerPrefs.GetString(PLAYER_PREFS_KEY);
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(json);

            foreach (var bodyPartTypeIndex in saveObject.bodyPartTypeIndexList)
            {
                BodyPartData bodyPartData = GetBodyPartData(bodyPartTypeIndex.bodyPartType);
                bodyPartData.skinnedMeshRenderer.sharedMesh = bodyPartData.meshArray[bodyPartTypeIndex.index].ItemMesh;
            }
        }
        #endregion
        
        #region Fields
        private const string PLAYER_PREFS_KEY = "PlayerCustom";

        //[SerializeField] private BodyPartDataPrefab[] bodyPartDataArray;
        
        [SerializeField] private BodyPartData[] bodyPartMeshDataArray;
        
        [Tooltip("If true, all prefabs will be instantiated once at Start and toggled on/off when changing.")]
        [SerializeField] private bool _preloadPrefabs = true;
        #endregion
    }
}