using Shared.Utils.ReadOnlyCustom;

namespace Runtime.Services.Data
{
    public class DataService : AService
    {
        #region functions

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
        
        public void AddMalicePoints(int amount)
        {
            MalicePoints += amount;
            SaveData();
        }
        
        public void SaveData()
        {
            string data = JsonUtility.ToJson(this);
            Debug.Log("Saving Data: " + data);
            PlayerPrefs.SetString(PlayerPrefsSave, data);
            PlayerPrefs.Save();
        }
        
        public void LoadData()
        {
            string data = PlayerPrefs.GetString(PlayerPrefsSave);
            Debug.Log("Loading Data: " + data);
            if (!string.IsNullOrEmpty(data))
            {
                JsonUtility.FromJsonOverwrite(data, this);
            }
        }
        
        #endregion
        
        #region fields

        [ReadOnly] public int DevilDollars;

        [ReadOnly] public int MalicePoints;
        
        [ReadOnly] public string PlayerPrefsSave = "PlayerData";
        
        #endregion
    }
}