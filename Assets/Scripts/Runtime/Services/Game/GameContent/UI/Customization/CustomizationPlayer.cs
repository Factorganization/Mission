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
        
        [Serializable]
        public class BodyPartData
        {
            public BodyPartType bodyPartType;
            public SkinnedMeshRenderer skinnedMeshRenderer;
            public CustomizeItem[] meshArray;
        }
        
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
        
        public void ApplyMaterialToHead(Material mat, int materialIndex = 0)
        {
            if (mat == null || _head == null) return;

            Material[] mats = _head.sharedMaterials;
            if (mats == null || mats.Length == 0)
            {
                _head.sharedMaterials = new Material[] { mat };
                return;
            }

            int idx = Mathf.Clamp(materialIndex, 0, mats.Length - 1);
            mats[idx] = mat;
            _head.sharedMaterials = mats;
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
            return bodyPartData != null ? bodyPartData.skinnedMeshRenderer : null;
        }

        #endregion
        

        [Serializable]
        public class BodyPartTypeIndex
        {
            public BodyPartType bodyPartType;
            public int index;
        }

        private class SaveObject
        {
            public List<BodyPartTypeIndex> bodyPartTypeIndexList;
        }

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
        
        [SerializeField] private BodyPartData[] bodyPartMeshDataArray;

        [SerializeField] private MeshRenderer _head;

        #endregion
    }
}