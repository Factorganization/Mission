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
        }
        
        public void AddMalicePoints(int amount)
        {
            MalicePoints += amount;
            SaveData();
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
        
        #endregion
        
        #endregion
        
        #region fields

        [ReadOnly] public float masterVolume;
        
        [ReadOnly] public float musicVolume;
        
        [ReadOnly] public float sfxVolume;
        
        [ReadOnly] public int DevilDollars;

        [ReadOnly] public int MalicePoints;
        
        [ReadOnly] public string PlayerPrefsSave = "PlayerData";
        
        #endregion
    }
}