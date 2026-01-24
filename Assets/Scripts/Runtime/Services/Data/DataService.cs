using Runtime.Service;

namespace Runtime.Services.Data
{
    public class DataService : AService
    {
        #region properties

        public int DevilDollars { get; set; }
        
        public int MalicePoints { get; set; }
        
        #endregion
        
        #region functions

        public void SaveData()
        {
            
        }
        
        #endregion
        
        #region fields
        
        public string PlayerPrefsSave = "PlayerData";
        
        #endregion
    }
}