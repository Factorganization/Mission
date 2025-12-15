using FMODUnity;

namespace Runtime.Management.AudioManager
{
    public class FMODEvents : MonoBehaviour
    {
        [field : Header("MasterVolume")]
        [field: SerializeField] public EventReference MasterVolume { get; private set; }
        
        [field : Header("MusicVolume")]
        [field: SerializeField] public EventReference MusicVolume { get; private set; }
        
        [field : Header("SFXVolume")]
        [field: SerializeField] public EventReference SFXVolume { get; private set; }
    
        public static FMODEvents Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple instances of FMODEvents detected. Destroying the new instance.");
                return;
            }
        
            Instance = this;
        }
    }
}