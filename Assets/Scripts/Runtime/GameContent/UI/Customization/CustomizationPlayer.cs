using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationPlayer : MonoBehaviour
{
    private const string PLAYER_PREFS_KEY = "PlayerCustom";
    
    [SerializeField] private BodyPartData[] bodyPartDataArray;
    public BodyPartData[] BodyPartDataArray => bodyPartDataArray;
    
    public enum BodyPartType
    {
        Horns,
        Head,
        Eyes,
        Body,
        Tail
    }
    
    [System.Serializable]
    public class BodyPartData
    {
        public BodyPartType bodyPartType;
        public Mesh[] meshArray;
        public SkinnedMeshRenderer skinnedMeshRenderer;
    }

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
    }
}
