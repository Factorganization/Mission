using System.Collections.Generic;
using Runtime.Services.Game.GameContent.UI.Customization;
using Runtime.Services.Game.GameSystems;
using Shared.Utils.ReadOnlyCustom;

namespace Runtime.Services.Data
{
    public class DataService : AService
    {
        #region methodes

        #region service

        public override bool Init()
        {
            LoadData();
            
            return true;
        }

        public override void Delete()
        {
            SaveData();
        }

        #endregion

        #region data
        
        public void SetMasterVolume(int value)
        {
            masterVolume = value;
            SaveData();
        }
        
        public void SetMusicVolume(int value)
        {
            musicVolume = value;
            SaveData();
        }
        
        public void SetSfxVolume(int value)
        {
            sfxVolume = value;
            SaveData();
        }
        
        public void SetDevilDollars(int amount)
        {
            DevilDollars = amount;
            SaveData();
        }
        
        public void AddMoney(int amount)
        {
            DevilDollars += amount;
            SaveData();
        }
        
        public void SubtractMoney(int amount)
        {
            DevilDollars -= amount;
            SaveData();
        }
        
        public void SetMalicePoints(int amount)
        {
            MalicePoints = amount;
            SaveData();
        }

        public void AddMalicePointsSoft(int amount)
        {
            MalicePoints += amount;

            if (GameManager.Instance.GameUIMgr != null)
            {
                GameManager.Instance.GameUIMgr.UpdateMalicePoints();
            }
        }
        
        public void AddMalicePoints(int amount)
        {
            MalicePoints += amount;
            SaveData();
        }
        
        public void AddPurchaseItem(string itemID)
        {
            if (!PurchasedItems.ContainsKey(itemID))
            {
                PurchasedItems.Add(itemID, true);
                SaveData();
            }
        }

        public void UpdatePurchaseItem(string itemID, CustomizationPlayer player)
        {
            foreach (var item in player.BodyPartMeshDataArray)
            {
                foreach (var part in item.meshArray)
                {
                    if (part.ItemName == itemID)
                    {
                        part.Locked = false;
                        SaveData();
                        return;
                    }
                }
            }
        }
        
        public void SaveData()
        {
            string data = JsonUtility.ToJson(this);
#if UNITY_EDITOR
            Debug.Log("Saving Data: " + data);
#endif
            PlayerPrefs.SetString(PlayerPrefsSave, data);
            PlayerPrefs.Save();
        }
        
        public void LoadData()
        {
            string data = PlayerPrefs.GetString(PlayerPrefsSave);
#if UNITY_EDITOR
            Debug.Log("Loading Data: " + data);
#endif
            if (!string.IsNullOrEmpty(data))
            {
                JsonUtility.FromJsonOverwrite(data, this);
            }
            else
            {
                SaveData();
            }
        }
        
        public void ResetData()
        {
            masterVolume = 1f;
            musicVolume = 1f;
            sfxVolume = 1f;
            DevilDollars = 0;
            MalicePoints = 0;
            PurchasedItems.Clear();
            SaveData();
        }
        
        #endregion
        
        #endregion
        
        #region fields

        [ReadOnly] public float masterVolume = 0.5f;
        
        [ReadOnly] public float musicVolume = 0.5f;
        
        [ReadOnly] public float sfxVolume = 0.5f;

        [ReadOnly] public float sensi = 1f;
        
        [ReadOnly] public int DevilDollars;

        [ReadOnly] public int MalicePoints;
        
        [ReadOnly] public string PlayerPrefsSave = "PlayerData";
        
        public Dictionary<string, bool> PurchasedItems = new Dictionary<string, bool>();
        
        #endregion
    }
}