using System.Collections.Generic;
using System.Linq;

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
        
        [Serializable]
        public class BodyPartData
        {
            public BodyPartType bodyPartType;
            public SkinnedMeshRenderer skinnedMeshRenderer;
            public CustomizeItem[] meshArray;
        }
        
        #endregion

        #region Meshes

        private void Start()
        {
            string json = PlayerPrefs.GetString(PLAYER_PREFS_SAVE, string.Empty);
            if (!string.IsNullOrEmpty(json))
            {
                Load();
            }
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
        
        /*public void ApplyMaterialToHead(Material mat, int materialIndex = 0)
        {
            if (mat == null || head == null) return;

            Material[] mats = head.sharedMaterials;
            if (mats == null || mats.Length == 0)
            {
                head.sharedMaterials = new Material[] { mat };
                return;
            }

            int idx = Mathf.Clamp(materialIndex, 0, mats.Length - 1);
            mats[idx] = mat;
            head.sharedMaterials = mats;
        }*/

        public void ApplyMaterialToBodySkin(Material mat)
        {
            if (mat == null) return;

            Material[] mats = GetRendererForBodyPart(BodyPartType.Body).sharedMaterials;
            if (mats == null || mats.Length == 0)
            {
                GetRendererForBodyPart(BodyPartType.Body).sharedMaterials = new Material[] { mat };
                return;
            }

            mats[1] = mat;
            GetRendererForBodyPart(BodyPartType.Body).sharedMaterials = mats;
        }

        public void ApplyMatToSkinnedMesh(SkinnedMeshRenderer skinnedMeshRenderer, Material mat,int materialIndex = 0)
        {
            if (mat == null) return;
            
            Material[] mats = skinnedMeshRenderer.sharedMaterials;
            if (mats == null || mats.Length == 0)
            {
                skinnedMeshRenderer.sharedMaterials = new Material[] { mat };
                return;
            }
            
            mats[materialIndex] = mat;
            skinnedMeshRenderer.sharedMaterials = mats;
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

        private SkinnedMeshRenderer GetSkinnedMeshRenderer(BodyPartType bodyPartType)
        {
            BodyPartData bodyPartData = GetBodyPartData(bodyPartType);
            return bodyPartData?.skinnedMeshRenderer;
        }

        #endregion
        

        [Serializable]
        public class BodyPartTypeIndex
        {
            public BodyPartType bodyPartType;
            public int index;
        }

        [Serializable]
        public class BodyPartMaterialSave
        {
            public BodyPartType bodyPartType;
            public string[] materialNames;
        }

        [Serializable]
        private class SaveObject
        {
            public List<BodyPartTypeIndex> bodyPartTypeIndexList;
            public List<BodyPartMaterialSave> bodyPartMaterials;
        }

        #region MeshesSaveLoad
        public void Save()
        {
            List<BodyPartTypeIndex> bodyPartTypeIndexList = new List<BodyPartTypeIndex>();
            List<BodyPartMaterialSave> bodyPartMaterials = new List<BodyPartMaterialSave>();

            foreach (BodyPartType bodyPartType in Enum.GetValues(typeof(BodyPartType)))
            {
                BodyPartData bodyPartData = GetBodyPartData(bodyPartType);
                int meshIndex = 0;

                if (bodyPartData != null && bodyPartData.meshArray != null && bodyPartData.meshArray.Length > 0 &&
                    bodyPartData.skinnedMeshRenderer != null)
                {
                    var currentMesh = bodyPartData.skinnedMeshRenderer.sharedMesh;

                    int found = -1;
                    for (int i = 0; i < bodyPartData.meshArray.Length; i++)
                    {
                        var item = bodyPartData.meshArray[i];
                        if (item != null && item.ItemMesh == currentMesh)
                        {
                            found = i;
                            break;
                        }
                    }

                    meshIndex = found >= 0 ? found : 0;
                    meshIndex = Mathf.Clamp(meshIndex, 0, bodyPartData.meshArray.Length - 1);
                }

                bodyPartTypeIndexList.Add(new BodyPartTypeIndex
                {
                    bodyPartType = bodyPartType,
                    index = meshIndex
                });

                var renderer = GetRendererForBodyPart(bodyPartType);
                if (renderer != null)
                {
                    var mats = renderer.sharedMaterials;
                    if (mats != null && mats.Length > 0)
                    {
                        string[] names = mats.Select(m => NormalizeMaterialName(m)).ToArray();
                        bodyPartMaterials.Add(new BodyPartMaterialSave
                        {
                            bodyPartType = bodyPartType,
                            materialNames = names
                        });
                    }
                    else
                    {
                        bodyPartMaterials.Add(new BodyPartMaterialSave
                        {
                            bodyPartType = bodyPartType,
                            materialNames = new string[0]
                        });
                    }
                }
            }

            SaveObject saveObject = new SaveObject
            {
                bodyPartTypeIndexList = bodyPartTypeIndexList,
                bodyPartMaterials = bodyPartMaterials
            };

            string json = JsonUtility.ToJson(saveObject);
            Debug.Log(json);
            
            PlayerPrefs.SetString(PLAYER_PREFS_SAVE, json);
            PlayerPrefs.Save();
        }

        public void Load()
        {
            string json = PlayerPrefs.GetString(PLAYER_PREFS_SAVE, string.Empty);
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogWarning("No saved customization found.");
                return;
            }

            SaveObject saveObject;
            try
            {
                saveObject = JsonUtility.FromJson<SaveObject>(json);
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to parse saved customization: " + ex.Message);
                return;
            }

            if (saveObject == null)
            {
                Debug.LogWarning("Saved customization is empty or invalid.");
                return;
            }

            if (saveObject.bodyPartTypeIndexList != null)
            {
                foreach (var bodyPartTypeIndex in saveObject.bodyPartTypeIndexList)
                {
                    BodyPartData bodyPartData = GetBodyPartData(bodyPartTypeIndex.bodyPartType);
                    if (bodyPartData == null || bodyPartData.meshArray == null || bodyPartData.meshArray.Length == 0 || bodyPartData.skinnedMeshRenderer == null)
                    {
                        Debug.LogWarning($"Skipping load for {bodyPartTypeIndex.bodyPartType}: data missing.");
                        continue;
                    }
                    
                    int idx = bodyPartTypeIndex.index;
                    if (idx < 0 || idx >= bodyPartData.meshArray.Length)
                    {
                        Debug.LogWarning($"Saved index {idx} out of range for {bodyPartTypeIndex.bodyPartType}. Clamping to valid range.");
                        idx = Mathf.Clamp(idx, 0, bodyPartData.meshArray.Length - 1);
                    }

                    var item = bodyPartData.meshArray[idx];
                    if (item == null || item.ItemMesh == null)
                    {
                        Debug.LogWarning($"Mesh at index {idx} for {bodyPartTypeIndex.bodyPartType} is null. Skipping.");
                        continue;
                    }
                        
                    bodyPartData.skinnedMeshRenderer.sharedMesh = item.ItemMesh;
                }
            }

            if (saveObject.bodyPartMaterials != null && saveObject.bodyPartMaterials.Count > 0)
            {
                var allMats = Resources.FindObjectsOfTypeAll<Material>();

                foreach (var mat in saveObject.bodyPartMaterials)
                {
                    var renderer = GetRendererForBodyPart(mat.bodyPartType);
                    if (renderer == null) continue;
                    
                    if (mat.materialNames == null || mat.materialNames.Length == 0) continue;
                    
                    var restored = new List<Material>();
                    foreach (var matName in mat.materialNames)
                    {
                        if (string.IsNullOrEmpty(matName))
                        {
                            restored.Add(null);
                            continue;
                        }
                    }
                    
                    var found = allMats.FirstOrDefault(m => NormalizeMaterialName(m) == mat.materialNames[0]);
                    if (found != null)
                    {
                        restored.Add(found);
                    }
                    else
                    {
                        var current = renderer.sharedMaterials;
                        if (current != null && current.Length > restored.Count)
                            restored.Add(current[restored.Count]);
                        else
                            restored.Add(null);
                    }
                    
                    var currentMats = renderer.sharedMaterials;
                    int targetLen = Mathf.Max(currentMats != null ? currentMats.Length : 0, restored.Count);
                    Material[] finalMats = new Material[targetLen];
                    for (int i = 0; i < targetLen; i++)
                    {
                        if (i < restored.Count && restored[i] != null)
                            finalMats[i] = restored[i];
                        else if (currentMats != null && i < currentMats.Length)
                            finalMats[i] = currentMats[i];
                        else
                            finalMats[i] = null;
                    }

                    renderer.sharedMaterials = finalMats;
                }
            }
        }
        #endregion
        
        #region Fields
        private const string PLAYER_PREFS_SAVE = "PlayerCustom";
        
        [SerializeField] private BodyPartData[] bodyPartMeshDataArray;

        [SerializeField] private SkinnedMeshRenderer head, tail;

        public SkinnedMeshRenderer Head => head;
        public SkinnedMeshRenderer Tail => tail;
        
        #endregion
        
        private static string NormalizeMaterialName(Material m)
        {
            if (m == null) return string.Empty;
            // strip Unity instance suffix if present
            var name = m.name;
            const string instanceSuffix = " (Instance)";
            if (name.EndsWith(instanceSuffix, StringComparison.Ordinal))
                name = name.Substring(0, name.Length - instanceSuffix.Length);
            return name;
        }
    }
}